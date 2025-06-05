import tkinter as tk
from tkinter import messagebox, filedialog
import requests
from requests.auth import HTTPBasicAuth
from dateutil import parser
import numpy as np
import base64
import cv2
from PIL import Image, ImageTk

API_BASE = "https://localhost:44314"

class App:
    def __init__(self, root):
        self.root = root
        self.root.title("Rezerwacja biletów")
        self.auth = None
        self.tk_image = None  # referencja do obrazka (by go nie wyczyściło GC)
        self.movies = []

        self.show_login_screen()

    def show_login_screen(self):
        login_win = tk.Toplevel(self.root)
        login_win.title("Logowanie")

        tk.Label(login_win, text="Login:").grid(row=0, column=0)
        username_entry = tk.Entry(login_win)
        username_entry.grid(row=0, column=1)

        tk.Label(login_win, text="Hasło:").grid(row=1, column=0)
        password_entry = tk.Entry(login_win, show="*")
        password_entry.grid(row=1, column=1)

        def login():
            username = username_entry.get().strip()
            password = password_entry.get().strip()
            if not username or not password:
                messagebox.showwarning("Uwaga", "Wprowadź login i hasło.")
                return

            self.auth = HTTPBasicAuth(username, password)
            try:
                res = requests.get(f"{API_BASE}/movies", auth=self.auth, verify=False)
                if res.status_code == 200:
                    self.movies = res.json()
                    login_win.destroy()
                    self.show_movies_list()
                else:
                    messagebox.showerror("Błąd", "Błędne dane logowania.")
            except Exception as e:
                messagebox.showerror("Błąd", f"Błąd połączenia:\n{e}")

        tk.Button(login_win, text="Zaloguj", command=login).grid(row=2, column=0, columnspan=2, pady=5)

    def clear_root(self):
        for widget in self.root.winfo_children():
            widget.destroy()

    def show_movies_list(self):
        self.clear_root()
        tk.Label(self.root, text="Lista filmów:", font=("Arial", 14)).pack(pady=5)

        self.listbox = tk.Listbox(self.root, width=70, height=15)
        self.listbox.pack(pady=10)

        for movie in self.movies:
            showtimes_str = ", ".join([
                parser.parse(s['time']).strftime("%Y-%m-%d %H:%M")
                for s in movie.get('showtimes', [])
            ])
            self.listbox.insert(tk.END, f"{movie['title']} | Seanse: {showtimes_str}")

        btn_frame = tk.Frame(self.root)
        btn_frame.pack(pady=5)

        tk.Button(btn_frame, text="Pokaż szczegóły", command=self.show_selected_movie_details).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="Moje rezerwacje", command=self.show_my_reservations).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="Wyjście", command=self.root.quit).pack(side=tk.LEFT, padx=5)

    def show_selected_movie_details(self):
        index = self.listbox.curselection()
        if not index:
            messagebox.showwarning("Uwaga", "Wybierz film z listy.")
            return

        movie_id = self.movies[index[0]]['id']
        try:
            res = requests.get(f"{API_BASE}/movies/{movie_id}", auth=self.auth, verify=False)
            if res.status_code == 200:
                movie = res.json()
                self.show_movie_details(movie)
            else:
                messagebox.showerror("Błąd", f"Nie udało się pobrać szczegółów: {res.status_code}")
        except Exception as e:
            messagebox.showerror("Błąd", str(e))

    def show_movie_details(self, movie):
        self.clear_root()

        tk.Label(self.root, text=f"Tytuł: {movie['title']}", font=("Arial", 16, "bold")).pack(pady=5)

        # Obrazek filmu (base64)
        if 'imageBase64' in movie and movie['imageBase64']:
            try:
                image_data = base64.b64decode(movie['imageBase64'])
                np_arr = np.frombuffer(image_data, np.uint8)
                img_cv2 = cv2.imdecode(np_arr, cv2.IMREAD_COLOR)

                if img_cv2 is not None:
                    img_cv2 = cv2.resize(img_cv2, (200, 300))
                    img_rgb = cv2.cvtColor(img_cv2, cv2.COLOR_BGR2RGB)
                    img_pil = Image.fromarray(img_rgb)
                    self.tk_image = ImageTk.PhotoImage(img_pil)
                    tk.Label(self.root, image=self.tk_image).pack(pady=5)
                else:
                    print("Nie udało się załadować obrazu.")
            except Exception as e:
                print("Błąd przy dekodowaniu obrazu (cv2):", e)

        tk.Label(self.root, text=f"Reżyser: {movie['director']}").pack()
        tk.Label(self.root, text=f"Aktorzy: {', '.join(movie.get('actors', []))}").pack()
        tk.Label(self.root, text="Opis:", font=("Arial", 10, "bold")).pack(pady=(5, 0))
        tk.Label(self.root, text=movie['description'], wraplength=500, justify="left").pack()
        tk.Label(self.root, text="Seanse i miejsca:", font=("Arial", 10, "bold")).pack(pady=5)

        # Pętla po seansach i wyświetlanie info
        for showtime in movie.get('showtimes', []):
            dt = parser.parse(showtime['time'])
            day_str = dt.strftime("%Y-%m-%d")
            time_str = dt.strftime("%H:%M")

            frame = tk.LabelFrame(self.root, text=f"{day_str} {time_str}", padx=5, pady=5)
            frame.pack(fill="x", padx=10, pady=3)

            seats = showtime.get('seats', [])
            seats_str = ", ".join(
                f"{seat['number']}{' (zajęte)' if seat['isReserved'] else ''}"
                for seat in seats
            )
            tk.Label(frame, text=f"Miejsca: {seats_str}").pack(anchor="w")

            # Dodaj przycisk rezerwacji
            btn = tk.Button(frame, text="Rezerwuj miejsce",
                            command=lambda st=showtime, m=movie: self.open_reservation_window(m, st))
            btn.pack(anchor="e", pady=2)

        # Przycisk powrotu do listy filmów
        tk.Button(self.root, text="Powrót do listy filmów", command=self.show_movies_list).pack(pady=10)

    def open_reservation_window(self, movie, showtime):
        res_win = tk.Toplevel(self.root)
        res_win.title(f"Rezerwacja miejsc - {movie['title']} ({showtime['time']})")

        seats = showtime.get('seats', [])
        available_seats = [seat for seat in seats if not seat['isReserved']]

        tk.Label(res_win, text="Wybierz miejsce/miejsca do rezerwacji:").pack(pady=5)

        seat_vars = {}  # słownik seat_number -> IntVar

        for seat in available_seats:
            var = tk.IntVar()
            cb = tk.Checkbutton(res_win, text=f"Miejsce {seat['number']}", variable=var)
            cb.pack(anchor='w')
            seat_vars[seat['number']] = var

        def confirm_reservation():
            selected_seats = [int(num) for num, var in seat_vars.items() if var.get() == 1]
            if not selected_seats:
                messagebox.showwarning("Uwaga", "Wybierz przynajmniej jedno miejsce.")
                return

            reservation_data = {
                "movieId": movie['id'],
                "showtimeId": showtime['id'],
                "seatNumbers": selected_seats
            }

            import json
            print("Wysyłam rezerwację JSON:", json.dumps(reservation_data, indent=2))

            try:
                res = requests.post(f"{API_BASE}/reservations", json=reservation_data, auth=self.auth, verify=False)
                if res.status_code == 201:
                    messagebox.showinfo("Sukces", "Rezerwacja została dokonana.")
                    res_win.destroy()
                    self.show_movie_details(movie)
                elif res.status_code == 409:
                    messagebox.showerror("Błąd", "Wybrane miejsce jest już zajęte.")
                elif res.status_code == 400:
                    messagebox.showerror("Błąd", f"Nieprawidłowe dane (400): {res.text}")
                else:
                    messagebox.showerror("Błąd", f"Błąd rezerwacji: {res.status_code}\n{res.text}")
            except Exception as e:
                messagebox.showerror("Błąd", f"Błąd połączenia:\n{e}")


        tk.Button(res_win, text="Rezerwuj", command=confirm_reservation).pack(pady=10)

    # Rezerwacje
    def show_my_reservations(self):
        self.clear_root()
        tk.Label(self.root, text="Moje rezerwacje", font=("Arial", 14)).pack(pady=5)

        try:
            # Zakładam, że API ma endpoint /reservations zwracający rezerwacje zalogowanego użytkownika
            res = requests.get(f"{API_BASE}/reservations", auth=self.auth, verify=False)
            if res.status_code == 200:
                reservations = res.json()
                if not reservations:
                    tk.Label(self.root, text="Brak rezerwacji.").pack()
                    tk.Button(self.root, text="Powrót do listy filmów", command=self.show_movies_list).pack(pady=10)
                    return

                listbox = tk.Listbox(self.root, width=80, height=15)
                listbox.pack(pady=10)

                for r in reservations:
                    # Dostosuj formatowanie do tego, co zwraca Twoje API
                    item_text = f"Rezerwacja ID: {r.get('id')} | Status: {r.get('status', 'brak')} | Film ID: {r.get('movieId', 'nieznany')}"
                    listbox.insert(tk.END, item_text)

                btn_frame = tk.Frame(self.root)
                btn_frame.pack(pady=5)
                tk.Button(btn_frame, text="Odśwież", command=self.show_my_reservations).pack(side=tk.LEFT, padx=5)
                tk.Button(btn_frame, text="Powrót do listy filmów", command=self.show_movies_list).pack(side=tk.LEFT, padx=5)
                tk.Button(btn_frame, text="Szczegóły rezerwacji",
                        command=lambda: self.show_reservation_details(reservations, listbox)).pack(side=tk.LEFT, padx=5)

            else:
                messagebox.showerror("Błąd", f"Nie udało się pobrać rezerwacji: {res.status_code}")
                self.show_movies_list()

        except Exception as e:
            messagebox.showerror("Błąd", f"Błąd połączenia:\n{e}")
            self.show_movies_list()

    def show_reservation_details(self, reservations, listbox):
        selection = listbox.curselection()
        if not selection:
            messagebox.showwarning("Uwaga", "Wybierz rezerwację.")
            return

        index = selection[0]
        reservation = reservations[index]
        res_id = reservation.get("id")

        try:
            # Pobierz szczegóły rezerwacji
            res = requests.get(f"{API_BASE}/reservations/{res_id}", auth=self.auth, verify=False)
            if res.status_code == 200:
                details = res.json()

                # Pobierz info o filmie żeby wyświetlić obrazek
                movie_res = requests.get(f"{API_BASE}/movies/{details.get('movieId')}", auth=self.auth, verify=False)
                movie = movie_res.json() if movie_res.status_code == 200 else None

                details_win = tk.Toplevel(self.root)
                details_win.title(f"Szczegóły rezerwacji")

                # Nie pokazuj ID rezerwacji
                # tk.Label(details_win, text=f"ID: {details.get('id')}").pack()

                # Pokaż pozostałe dane
                tk.Label(details_win, text=f"Film ID: {details.get('movieId')}").pack()
                tk.Label(details_win, text=f"Seans ID: {details.get('showtimeId')}").pack()
                tk.Label(details_win, text=f"Miejsca: {', '.join(map(str, details.get('seatNumbers', [])))}").pack()
                tk.Label(details_win, text=f"Użytkownik: {details.get('userName')}").pack()
                tk.Label(details_win, text=f"Data rezerwacji: {details.get('createdAt')}").pack()

                # Wyświetl zdjęcie filmu, jeśli jest
                if movie and movie.get('imageBase64'):
                    try:
                        import base64
                        import numpy as np
                        import cv2
                        from PIL import Image, ImageTk

                        image_data = base64.b64decode(movie['imageBase64'])
                        np_arr = np.frombuffer(image_data, np.uint8)
                        img_cv2 = cv2.imdecode(np_arr, cv2.IMREAD_COLOR)

                        if img_cv2 is not None:
                            img_cv2 = cv2.resize(img_cv2, (200, 300))
                            img_rgb = cv2.cvtColor(img_cv2, cv2.COLOR_BGR2RGB)
                            img_pil = Image.fromarray(img_rgb)
                            self.tk_image = ImageTk.PhotoImage(img_pil)  # referencja, żeby GC nie usunął
                            tk.Label(details_win, image=self.tk_image).pack(pady=5)
                        else:
                            tk.Label(details_win, text="Brak zdjęcia filmu.").pack()
                    except Exception as e:
                        tk.Label(details_win, text=f"Błąd ładowania zdjęcia: {e}").pack()
                else:
                    tk.Label(details_win, text="Brak zdjęcia filmu.").pack()

                # Przyciski
                btn_frame = tk.Frame(details_win)
                btn_frame.pack(pady=10)

                tk.Button(btn_frame, text="Usuń rezerwację", fg="red",
                        command=lambda: self.delete_reservation_by_id(res_id, details_win)).pack(side=tk.LEFT, padx=10)
                tk.Button(btn_frame, text="Pobierz jako PDF",
                        command=lambda: self.download_reservation_pdf(res_id)).pack(side=tk.LEFT, padx=10)
                tk.Button(btn_frame, text="Powrót do listy",
                        command=details_win.destroy).pack(side=tk.LEFT, padx=10)

            elif res.status_code == 404:
                messagebox.showerror("Błąd", "Rezerwacja nie znaleziona.")
            else:
                messagebox.showerror("Błąd", f"Błąd pobierania danych: {res.status_code}")
        except Exception as e:
            messagebox.showerror("Błąd", f"Błąd połączenia:\n{e}")

    def delete_reservation_by_id(self, res_id, window_to_close=None):
        confirm = messagebox.askyesno("Potwierdzenie", "Czy na pewno chcesz usunąć tę rezerwację?")
        if not confirm:
            return

        try:
            res = requests.delete(f"{API_BASE}/reservations/{res_id}", auth=self.auth, verify=False)
            if res.status_code == 204:
                messagebox.showinfo("Sukces", "Rezerwacja została usunięta.")
                if window_to_close:
                    window_to_close.destroy()
                self.show_my_reservations()
            elif res.status_code == 404:
                messagebox.showerror("Błąd", "Rezerwacja nie znaleziona.")
            else:
                messagebox.showerror("Błąd", f"Błąd usuwania: {res.status_code}")
        except Exception as e:
            messagebox.showerror("Błąd", f"Błąd połączenia:\n{e}")
            
    def download_reservation_pdf(self, reservation_id):
        try:
            res = requests.get(f"{API_BASE}/reservations/{reservation_id}/pdf", auth=self.auth, verify=False)
            if res.status_code == 200:
                file_path = filedialog.asksaveasfilename(defaultextension=".pdf",
                                                        filetypes=[("PDF files", "*.pdf")],
                                                        title="Zapisz PDF jako...")
                if file_path:
                    with open(file_path, 'wb') as f:
                        f.write(res.content)
                    messagebox.showinfo("Sukces", f"Plik zapisany jako:\n{file_path}")
            else:
                messagebox.showerror("Błąd", f"Nie udało się pobrać PDF: {res.status_code}")
        except Exception as e:
            messagebox.showerror("Błąd", f"Błąd pobierania PDF:\n{e}")


if __name__ == "__main__":
    import urllib3
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

    root = tk.Tk()
    app = App(root)
    root.mainloop()
