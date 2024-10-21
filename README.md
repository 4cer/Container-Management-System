[[_TOC_]]

------
# Opis projekt
Rolą aplikacji jest udostępnienie użytkownikowi interfejsu webowego, za pomocą którego będzie mógł manipulować kontenerami osadzonymi na wybranych maszynach wirtualnych (Linux oraz Windows). Użytkownik będzie miał możliwość monitorowania, tworzenia, usuwania, modyfikacji kontenerów itd. Rozwiązanie zostanie zrealizowane z wykorzystaniem platformy Azure.

Repozytorium stanowi archiwum projektu wykonanego w ramach przedmiotu "Projekt Zespołowy" przez grupę WCYII181S1.

# Stos technologiczny

| Element |Technologia  |Wersja  |
|--|--|--|
| Środowisko |Microsoft Azure  | N/A |
|  Backend| C# | 8.0 |
|  Frontend| ASP.NET 5.0 + Blazor WebAssembly (z wykorzystaniem MudBlazor i Razor Pages) | 6.0.101 |
|  Baza Danych| Azure SQL Database | N/A |
| Host Linux |Ubuntu   | 20.04 LTS |
| Host Windows | Windows | 10 |
|Repozytorium kodu| Azure DevOps |N/A|
|Serwer aplikacji Web|Nginx + Kestrel| 6.0.101 |
|Diagramy UML |draw.io| N/A |
| Konteneryzacja na Windowsie | Docker Desktop | 4.2.0|
| Konteneryzacja na Linuxie | Docker | 20.10.11 |

# Opis szczegółowy
Stworzona aplikacja powinna w prosty oraz scentralizowany sposób pozwolić użytkownikowi na zarządzanie kontenerami na minimum dwóch serwerach (1 linux oraz 1 windows).

Użytkownik obsługuje aplikację poprzez przeglądarkę internetową. Podczas logowania wymagane jest od niego podanie hasła oraz loginu. Każdy z użytkowników ma dostęp tylko do kontenerów stworzonych przez siebie. Użytkownik przed zalogowaniem się do systemu musi się zarejestrować jeżeli wcześniej tego nie zrobił. 

Użytkownik po wejściu do aplikacji przekierowywany jest na ekran główny (dashboard). Po nawiązaniu połączenia z hostami na ekranie zaprezentowany jest mu w sposób kompaktowy stan hostów macierzystych oraz liczba i stan wszystkich kontenerów. Użytkownik może wyświetlić szczegóły wszystkich posiadanych kontenerów. Użytkownik ma dostęp do menu, z którego może wykonywać operacje na kontenerach. Istnieje możliwość połączenia nowego hosta macierzystego w sposób manualny. Aplikacja łączy się automatycznie z hostami po załadowaniu strony i stan kontenerów na nich odświeża się manualnie.

Przez **"Host Macierzysty"** rozumie się maszynę serwera, na której zainstalowany jest docker.

Przez **Kontener** rozumie się standardową jednostkę oprogramowania, która zawiera kod i wszystkie jego zależności, aby aplikacja działała szybko i niezawodnie w każdym środowisku obliczeniowym. 

# Opis Aktorów
- Operator:
>Operator po zalogowaniu ma możliwości zarządzania oraz monitorowania kontenerów programu Docker przypisanych do swojego konta. W ramach zarządzania operator może wykonywać operacje takie jak tworzenie, usuwanie, startowanie, zatrzymywanie kontenerów. Ponadto ma on możliwość podgląd statusu kontenerów.
- Administrator: 
>Administrator jest odpowiedzialny za prawidłowe działanie systemu. Ma on możliwość bezpośredniej komunikacji z hostami które wchodzą w skład systemu oraz posiada na nich uprawnienia użytkownika uprzywilejowanego. Administrator powinien dokonywać wszelkich modyfikacji konfiguracji oraz stanu hostów w przypadku nieprawidłowości w działaniu systemu. Jest on również odpowiedzialny za wdrażanie aktualizacji hostów oraz systemu.    

   

# Analiza wymagań

W systemie istnieją dwie role:

- Operator — będący podstawowym użytkownikiem w systemie.
- Administrator — będący osobą odpowiedzialną za utrzymanie systemu. Konfigurację komponentów w środowisku Azure.


##Wymagania funkcjonalne

Operator ma następujące możliwości:
- Tworzenie kontenerów 
- Usuwanie swoich kontenerów 
- Monitorowanie stanu swoich kontenerów (np. włączony/wyłączony)
- Wyłączanie swoich kontenerów
- Włączanie swoich kontenerów
- Przeglądania logów z każdego swojego kontenera


Dodatkowe:
- zapis szablonu obrazu dockera do bazy
- odczyt szablonu obrazu dockera z bazy
- utworzenie kontenera za pomocą szablonu

Administrator, ponadto co może użytkownik, ma w aplikacji następujące możliwości:
- Dodawania hostów macierzystych
- Usuwania hostów macierzystych
- Usunięcie użytkownika
- Blokowanie użytkownika
- Odblokowanie użytkownika
- Ma kontrolę nad zasobami każdego użytkownika.

Administrator rozszerza funkcjonalności operatora, ma bezpośredni dostęp do hostów macierzystych i subskrypcji na Azure.

## Wymagania niefunkcjonalne

### Dostępność
- System będzie dostępny aż do terminu obrony projektu.
- System będzie dostępny dla użytkownika przez przeglądarkę w formie aplikacji internetowej.
- Minimalnie system musi działać w pełni poprawnie na 1 przeglądarce internetowej (Google Chrome).
- Każda opcja w interfejsie graficznym musi być dostępna po maksymalnie 3 zagnieżdżeniach.
### Wydajność
- System ma pozwalać na istnienie minimum 3 lekkich kontenerów na każdym z hostów macierzystych w tym samym czasie.
- System ma obsługiwać minimum jednego użytkownika.
- Czas reakcji systemu nie powinien przekraczać 10 sekund.

### Bezpieczeństwo
- System będzie wymagał uwierzytelnienia użytkownika.
- Hasło będzie przechowywane zaszyfrowane przy użyciu kryptograficznie bezpiecznego mechanizmu szyfrowania.
- Żadna usługa (np. ssh, rdp) dostępna z publicznej sieci nie będzie działała na domyślnym porcie (z wyjątkiem http i https).
- Każdy nowy użytkownik jest zablokowany i musi zostać odblokowany przez Administratora, aby korzystać z aplikacji.


# Architektura
![image.png](/.attachments/image-473a3067-f5a3-4b2c-941e-90774c1148ca.png)

Głównymi elementami rozwiązania są 3 maszyny wirtualne:
- Ubuntu Docker Host
- Windows Docker Host
- Serwer aplikacji 

Maszyny docker hostuja usługę docker do nich łączy się aplikacja i na nich utrzymywane są kontenery.
Operator zarządza kontenerami przez aplikacje, nie ma bezpośredniego dostępu do maszyn wirtualnych.
Operacje wykonane w aplikacji są przetwarzane na Serwerze Aplikacji oraz przekazywane na odpowiednie maszyny macierzyste dockerów.
Komunikacja pomiędzy maszynami przebiega dwustronnie. 
Każda z maszyn jest osadzona na innej subskrypcji platformy Azure.
