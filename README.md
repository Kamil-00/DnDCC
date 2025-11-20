# DnDCC

ASP.NET Core MVC + Web API z EF Core

## Wymagania

- .NET SDK 8.0+
- SQL Server Express LocalDB (Windows)

## Szybki start

1. Przywrócenie i kompilacja

```powershell
dotnet restore
dotnet build .\Projekt.sln
```

2. Migracje bazy danych

```powershell
dotnet ef database update --project .\Projekt.DAL\Projekt.DAL.csproj --startup-project .\Projekt.Web\Projekt.csproj
```

3. Uruchom aplikację

```powershell
dotnet run --project .\Projekt.Web\Projekt.csproj
```

## Konfiguracja

Plik: `Projekt/Projekt.Web/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DnDCCAppDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "DnDCC",
    "Audience": "DnDCC",
    "Key": "super_secret_key_change_me_please_very_long",
    "ExpiryMinutes": "120"
  }
}
```

W produkcji zmienic żeby klucz JWT był poza repozytorium (Secret Manager/KeyVault/zmienne środowiskowe).

## API (rejestracja, logowanie, autoryzacja)

1. Rejestracja użytkownika POST
   http://localhost:5000/api/auth/register

```powershell
{
    "userName": "Jan123",
    "email": "jan.ko@asd.com",
    "firstName": "Jan",
    "lastName": "Kowalski",
    "password": "asdasd12"
}
```

2. Logowanie i generowanie tokenu POST
   http://localhost:5000/api/auth/login

```powershell
{
    "userNameOrEmail": "Jan123",
    "password": "asdasd12"
}
```

Po logowaniu token zapisuje sie do cookies

3. Wywołanie chronionego endpointu GET
   http://localhost:5000/api/ping

Jeśli 401, sprawdź czy nagłówek `Authorization: Bearer <token>` jest ustawiony i czy token nie wygasł.

4. Wylogowanie i wyczyszczenie cookies GET
   http://localhost:5000/api/auth/logout

## Uwierzytelnianie w MVC (HTML)

- Akcje/kontrolery zabezpieczaj atrybutami:

```csharp
[Authorize]
public IActionResult OnlySignedIn() => View();

[Authorize(Roles = "Admin")]
public IActionResult AdminOnly() => View();
```
