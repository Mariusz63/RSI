import urllib3
from zeep import Client, Settings
from zeep.plugins import HistoryPlugin
from zeep.transports import Transport
from zeep.exceptions import Fault, TransportError
from zeep.helpers import serialize_object
from requests import Session
from datetime import datetime
import requests
from lxml import etree

# Ostrzeżenia o self-signed certyfikacie
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

# Konfiguracja HTTPS + MTOM
session = Session()
session.verify = False  # NIE dla produkcji! Do certyfikatów self-signed
transport = Transport(session=session)
settings = Settings(strict=False, xml_huge_tree=True)
history = HistoryPlugin()

# WSDL endpoint
wsdl_url = 'https://localhost:8443/Server?wsdl'
client = Client(wsdl=wsdl_url, transport=transport, settings=settings, plugins=[history])


# sprawdzenie połaczenia z serverem
def sprawdz_polaczenie():
    print("Sprawdzanie połączenia z serwerem Kina...")
    try:
        test_filmy = client.service.GetListaFilmow()
        print("\n📨 Odpowiedź SOAP od serwera (pretty printed):\n")
        print(etree.tostring(history.last_received["envelope"], pretty_print=True).decode())
        print("✅ Połączenie z serwerem zostało nawiązane.")
        return True
    except Fault as f:
        print("❌ Błąd ze strony serwera SOAP:")
        print(f)
    except Exception as e:
        print("❌ Nie udało się połączyć z serwerem:")
        print(e)
    return False


# Obsługa użytkownika (logowanie / tworzenie konta)
def logowanie_lub_rejestracja():
    print("Witaj w serwisie internetowym Kina!")
    imie_nazwisko = input("Podaj swoje imię i nazwisko, aby się zalogować: ").strip()

    try:
        user_id = client.service.Zaloguj(imie_nazwisko)
        print(f"Zalogowano jako: {imie_nazwisko} (ID: {user_id})\n")
        return imie_nazwisko, user_id
    except Fault as fault:
        print("Błąd SOAP:", fault.message)
        return None, None
    except Exception as e:
        print("Wystąpił problem z logowaniem:", e)
        return None, None

# Lista filmów
def lista_filmow():
    filmy = []
    try:
        filmy = client.service.GetListaFilmow()
        filmy = serialize_object(filmy)

        print("\n🎥 Lista filmów dostępnych w systemie:\n")
        for film in filmy:
            film_id = film['Id']
            szczegoly = client.service.GetFilmInfo(film_id)
            szczegoly = serialize_object(szczegoly)

            aktorzy = szczegoly['Aktorzy']
            if isinstance(aktorzy, dict) and 'string' in aktorzy:
                aktorzy_str = ", ".join(aktorzy['string'])
            else:
                aktorzy_str = ", ".join(aktorzy)

            print("🎬 Tytuł:", szczegoly['Tytul'])
            print("🎞️ Reżyser:", szczegoly['Rezyser'])
            print("👥 Aktorzy:", aktorzy_str)
            print("📝 Opis:", szczegoly['Opis'])

            seanse_raw = szczegoly.get("Seanse", [])
            if isinstance(seanse_raw, dict):
                seanse = seanse_raw.get("Seans", [])
            else:
                seanse = seanse_raw

            if not seanse:
                print("⚠️ Brak dostępnych seansów.\n")
                continue

            print("\n📅 Seanse:")
            for seans in seanse:
                seans_id = seans["Id"]
                try:
                    szczegoly_seansu = client.service.GetSeansInfo(seans_id)
                    szczegoly_seansu = serialize_object(szczegoly_seansu)

                    data = szczegoly_seansu["Data"]
                    godzina = szczegoly_seansu["Godzina"]
                    sala = szczegoly_seansu["Sala"]

                    data_str = datetime.strptime(data, "%Y-%m-%d").strftime("%d-%m-%Y") \
                        if isinstance(data, str) else data.strftime("%d-%m-%Y")

                    godzina_str = f"{godzina}:00" if isinstance(godzina, int) else str(godzina)

                    miejsca_raw = szczegoly_seansu.get("Miejsca", [])
                    if isinstance(miejsca_raw, dict):
                        miejsca = miejsca_raw.get("Miejsce", [])
                    else:
                        miejsca = miejsca_raw

                    zajete = [m['Numer'] for m in miejsca if m['Zajete']]
                    wolne = [m['Numer'] for m in miejsca if not m['Zajete']]

                    print(f"  📅 Data: {data_str}")
                    print(f"  ⏰ Godzina: {godzina_str}")
                    print(f"  🎭 Sala: {sala}")
                    print(f"  ✅ Wolne miejsca: {', '.join(map(str, wolne)) if wolne else 'Brak'}")
                    print(f"  ❌ Zajęte miejsca: {', '.join(map(str, zajete)) if zajete else 'Brak'}\n")

                except Fault as fault:
                    print("⚠️ Błąd przy pobieraniu seansu:", fault.message)

            print("-" * 50)

    except Fault as fault:
        print("🧨 Błąd SOAP:", fault.message)
    except Exception as e:
        print("⚠️ Wystąpił błąd:", str(e))
    return filmy

# Rezerwuj pojedyncze miejsce
def rezerwuj_miejsce(user_id, filmy):
    try:
        print("\n🎥 Dostępne filmy:")
        for i, film in enumerate(filmy):
            print(f"{i+1}. {film['Tytul']}")

        idx = int(input("\nWybierz numer filmu: ")) - 1
        film = filmy[idx]
        film_id = film['Id']

        szczegoly = serialize_object(client.service.GetFilmInfo(film_id))
        seanse_raw = szczegoly.get("Seanse", [])
        seanse = seanse_raw.get("Seans", []) if isinstance(seanse_raw, dict) else seanse_raw
        if isinstance(seanse, dict):
            seanse = [seanse]

        print("\n🎬 Dostępne seanse:")
        for i, seans in enumerate(seanse):
            print(f"{i + 1}. 📅 {seans['Data']} ⏰ {seans['Godzina']}:00 🎭 Sala: {seans['Sala']}")

        seans_idx = int(input("\nWybierz numer seansu: ")) - 1
        seans_id = seanse[seans_idx]['Id']

        miejsce = int(input("Podaj numer miejsca (np. 1): "))

        wynik = client.service.ZarezerwujMiejsce(user_id, film_id, seans_id, miejsce)
        print("✅", wynik)

    except Exception as e:
        print("🚨 Błąd rezerwacji:", e)


# Rezerwacja grupowa
def rezerwuj_wiele(user_id, filmy):
    try:
        print("\n🎥 Dostępne filmy:")
        for i, film in enumerate(filmy):
            print(f"{i+1}. {film['Tytul']}")

        idx = int(input("\nWybierz numer filmu: ")) - 1
        film_id = filmy[idx]['Id']
        szczegoly = serialize_object(client.service.GetFilmInfo(film_id))
        seanse_raw = szczegoly.get("Seanse", [])
        seanse = seanse_raw.get("Seans", []) if isinstance(seanse_raw, dict) else seanse_raw
        if isinstance(seanse, dict):  # pojedynczy seans jako dict
            seanse = [seanse]

        print("\n🎬 Dostępne seanse:")
        for i, seans in enumerate(seanse):
            print(f"{i + 1}. 📅 {seans['Data']} ⏰ {seans['Godzina']}:00 🎭 Sala: {seans['Sala']}")

        seans_idx = int(input("\nWybierz numer seansu: ")) - 1
        seans_id = seanse[seans_idx]['Id']

        miejsca_input = input("Podaj numery miejsc oddzielone przecinkami (np. 1,2,3): ")
        miejsca = [int(m.strip()) for m in miejsca_input.split(",")]

        wynik = client.service.ZarezerwujWieleMiejsc(user_id, film_id, seans_id, miejsca)
        print("✅", wynik)
    except Fault as fault:
        print("🧨 Błąd SOAP:", fault.message)
    except Exception as e:
        print("⚠️ Wystąpił błąd:", str(e))


# Lista rezerwacji
def lista_rezerwacji(user_id):
    try:
        rezerwacje = client.service.ListaRezerwacji(user_id)
        rezerwacje = serialize_object(rezerwacje)

        if not rezerwacje:
            print("📭 Brak rezerwacji.")
            return

        print("\n📋 Twoje rezerwacje:")
        for i, r in enumerate(rezerwacje):
            film_id = r['FilmId']
            seans_id = r['SeansId']
            miejsca = r.get('NumeryMiejsc', [])
            if isinstance(miejsca, dict) and 'int' in miejsca:
                miejsca = miejsca['int']

            # pobierz dane o filmie i seansie
            film = serialize_object(client.service.GetFilmInfo(film_id))
            seans = serialize_object(client.service.GetSeansInfo(seans_id))

            print(f"[{i}] 🎫 ID: {r['Id']}")
            print(f"    🎬 Film: {film['Tytul']}")
            print(f"    📅 Data: {seans['Data']} ⏰ {seans['Godzina']}:00 🎭 Sala: {seans['Sala']}")
            print(f"    🪑 Miejsca: {miejsca}")

    except Fault as fault:
        print("🧨 Błąd SOAP przy pobieraniu rezerwacji:", fault.message)
    except Exception as e:
        print("🚨 Błąd:", e)



# Anulowanie rezerwacji
def anuluj(user_id):
    rez_id = input("Podaj ID rezerwacji do anulowania: ")
    try:
        wynik = client.service.AnulujRezerwacje(user_id, rez_id)
        print("Rezerwacja anulowana" if wynik else "Nie znaleziono")
    except Fault as fault:
        print("Błąd anulowania rezerwacji:", fault.message)

# Modyfikacja rezerwacji
def modyfikuj(user_id):
    rez_id = input("Podaj ID rezerwacji do zmiany: ")
    nowe = int(input("Nowy numer miejsca: "))
    try:
        wynik = client.service.ModyfikujRezerwacje(rez_id, nowe)
        print("Zmieniono miejsce" if wynik else "Niepowodzenie")
    except Fault as fault:
        print("Błąd modyfikacji rezerwacji:", fault.message)

# Pobierz PDF
def pobierz_pdf(user_id):
    rez_id = input("Podaj ID rezerwacji do pobrania PDF: ")

    try:
        # Sprawdzenie listy rezerwacji
        rezerwacje = client.service.ListaRezerwacji(user_id)
        if not rezerwacje:
            raise ValueError("🔍 Brak rezerwacji dla tego użytkownika.")

        if not any(str(rez['Id']) == rez_id for rez in rezerwacje):
            raise ValueError("❌ Rezerwacja o podanym ID nie istnieje dla tego użytkownika.")

        # Pobierz PDF
        rezerwacja = client.service.PobierzPotwierdzenieRezerwacji(user_id, rez_id)

        if rezerwacja and rezerwacja['PotwierdzeniePdf']:
            filename = f"Potwierdzenie_{rez_id}.pdf"
            with open(filename, "wb") as f:
                f.write(rezerwacja['PotwierdzeniePdf'])
            print(f"✅ Zapisano jako {filename}")
        else:
            print("⚠️ Nie znaleziono PDF-a albo pusta odpowiedź")

    except ValueError as ve:
        print(ve)

    except Fault as fault:
        print("🧨 Błąd SOAP:", fault.message)

    except TransportError as te:
        print("🌐 Błąd transportu (np. połączenie z serwerem):", te)

    except requests.exceptions.ConnectionError as ce:
        print("❌ Połączenie zostało zamknięte przez serwer lub przerwane:", ce)

    except Exception as e:
        print("🚨 Nieoczekiwany błąd:", e)


# Menu główne
def menu():
    while True:
        print("\n--- MENU ---")
        print("1. Lista filmów")
        print("2. Rezerwuj miejsce")
        print("3. Rezerwuj grupowo")
        print("4. Lista rezerwacji")
        print("5. Anuluj rezerwację")
        print("6. Modyfikuj rezerwację")
        print("7. Pobierz PDF")
        print("0. Wyjście")

        wybor = input("Wybierz opcję: ")
        if wybor == "1":
            filmy = lista_filmow()
        elif wybor == "2":
            rezerwuj_miejsce(user_id, lista_filmow())
        elif wybor == "3":
            rezerwuj_wiele(user_id, lista_filmow())
        elif wybor == "4":
            lista_rezerwacji(user_id)
        elif wybor == "5":
            anuluj(user_id)
        elif wybor == "6":
            modyfikuj(user_id)
        elif wybor == "7":
            pobierz_pdf(user_id)
        elif wybor == "0":
            print("Do zobaczenia!")
            break
        else:
            print("Nieprawidłowy wybór.")

# Start aplikacji
if __name__ == "__main__":
    while not sprawdz_polaczenie():
        wybor = input("Czy chcesz spróbować ponownie? (t/n): ").strip().lower()
        if wybor != 't':
            print("Zamykam program.")
            exit()

    imie_nazwisko, user_id = logowanie_lub_rejestracja()
    if imie_nazwisko and user_id:
        menu() 