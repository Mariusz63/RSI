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

        tk.Button(self.root, text="Powrót do listy filmów", command=self.show_movies_list).pack(pady=10)


if __name__ == "__main__":
    import urllib3
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

    root = tk.Tk()
    app = App(root)
    root.mainloop()
