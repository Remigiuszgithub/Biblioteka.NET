# Biblioteka - System Zarządzania Biblioteką

## Opis Projektu / Project Description

**PL:**\
Aplikacja "Biblioteka" to system zarządzania zasobami bibliotecznymi stworzony w technologii **ASP.NET MVC** z wykorzystaniem **C#** i **Entity Framework**. Aplikacja umożliwia pełne zarządzanie książkami, czytelnikami, kategoriami oraz aktualnymi wypożyczeniami. System zabezpieczony jest mechanizmem logowania, a interfejs użytkownika bazuje na frameworku **Bootstrap** z motywem **Lumen**. Po zalogowaniu użytkownik widzi stronę z logiem biblioteki, powitaniem, stanowiskiem oraz informacją o pomyślnym zalogowaniu i gotowości do pracy w aplikacji.

**EN:**\
The "Biblioteka" application is a library management system developed using **ASP.NET MVC**, **C#**, and **Entity Framework**. It allows complete management of books, readers, categories, and current loans. The system is secured with a login mechanism, and the user interface is styled with **Bootstrap** using the **Lumen** theme. After logging in, the user is greeted with a library logo, a welcome message, their position, and a confirmation of successful login with readiness for work.

---

## Funkcje Aplikacji / Application Features

### Zarządzanie Książkami / Book Management

- Dodawanie, edytowanie i usuwanie książek.
- Możliwość przypisania książki do kategorii.
- Status dostępności (zielony: dostępna, czerwony: wypożyczona).
- Wypożyczenie i zwrot książki (automatyczna zmiana statusu).

### Zarządzanie Czytelnikami / Reader Management

- Dodawanie, edytowanie i usuwanie czytelników.
- Automatyczne generowanie numeru czytelnika.
- Generowanie karty bibliotecznej w formacie PDF (iText7).

### Zarządzanie Kategoriami / Category Management

- Dodawanie, edytowanie i usuwanie kategorii książek.
- Kategorie są powiązane bezpośrednio z książkami.

### Wypożyczenia / Loan Management

- Lista aktualnie wypożyczonych książek wraz z datami wypożyczenia i zwrotu.
- Informacja o osobie, która wypożyczyła książkę.

### Użytkownicy Aplikacji / Application Users

- Wyświetlanie listy użytkowników z dostępem do systemu.
- Dodawanie nowych użytkowników.

---

## Wykorzystane Technologie / Technologies Used

- **ASP.NET MVC (C#)** - Logika aplikacji i kontrolery.
- **Entity Framework** - Warstwa dostępu do danych.
- **SQL Server** - Baza danych o nazwie **BibliotekaDB**.
- **iText7** - Generowanie kart bibliotecznych w formacie PDF.
- **Bootstrap (Lumen)** - Stylizacja interfejsu użytkownika.

---

## Jak uruchomić projekt lokalnie? / How to Run the Project Locally?

### Wymagania wstępne / Prerequisites

- [Visual Studio](https://visualstudio.microsoft.com/) (zalecana wersja 2022 / recommended version 2022)
- .NET SDK 8.0
- SQL Server

### Kroki uruchomienia / Steps to Run the Project

1. **Pobranie projektu / Download the Project:**
   - Możesz pobrać projekt bezpośrednio z GitHuba za pomocą opcji "Download ZIP" lub korzystając z klienta GitHub Desktop.
2. **Otwórz projekt w Visual Studio / Open the Project in Visual Studio.**
3. **Przygotowanie bazy danych / Prepare the Database:**
   - Utwórz bazę danych o nazwie **BibliotekaDB** w SQL Server.
   - Otwórz **Package Manager Console** w Visual Studio / Open the **Package Manager Console** in Visual Studio.
   - Uruchom migracje i zaktualizuj bazę danych / Run migrations and update the database:
     ```bash
     Add-Migration InitialCreate
     Update-Database
     ```
4. **Uruchomienie aplikacji / Run the Application:**
   - Kliknij „Start” w Visual Studio lub uruchom aplikację poleceniem / Click "Start" in Visual Studio or run the following command:
     ```bash
     dotnet run
     ```
5. **Zalogowanie do systemu / Logging into the System:**
   - Po uruchomieniu aplikacji przejdź do `http://localhost:5000`.
   - Zaloguj się przy użyciu wbudowanego konta:
     - **Login:** `admin`
     - **Hasło:** `admin123`

---

## Struktura Bazy Danych / Database Structure

- **Książki (Books):** Tytuł, Autor, Kategoria, Dostępność.
- **Czytelnicy (Readers):** Imię, Nazwisko, Adres, Email, Numer czytelnika.
- **Kategorie (Categories):** Nazwa kategorii.
- **Wypożyczenia (Loans):** Książka, Czytelnik, Data wypożyczenia, Data zwrotu.
- **Użytkownicy (Users):** Nazwa użytkownika, Hasło, Stanowisko.

---

## Licencja / License

Projekt dostępny na licencji MIT.\
This project is licensed under the MIT License.

---

## Autor / Author

- Remigiusz Nowakowski

Jeśli masz pytania, zapraszam do kontaktu!\
If you have any questions, feel free to contact me!

