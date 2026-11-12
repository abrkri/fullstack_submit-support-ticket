# 01. Hibabejelentés és lezárás – megoldás

A felhasználó rövid hibaleírást küld be WPF-ből, MAUI-ból vagy WinUI-ból. Az admin WPF-ben, MAUI-ban vagy WinUI-ban letölti a bejelentéseket, majd a nyitott sor mellett a **Lezárás** gombbal lezárja az ügyet. A lezárt sor megmarad, az állapota „Lezárva”, a gomb eltűnik. Újranyitás és törlés nem része a feladatnak.

Az adatmodell három tulajdonsága: `int Id`, `string Description`, `bool IsClosed`. Nincs felhasználóazonosítás vagy belépési folyamat. Az új bejelentés `IsClosed = false` állapottal jön létre.

## Gyakorlófeladatok – a hibajegy bővítése

A működő hibajegykezelőt bővítsd az alábbi sorrendben. Az első feladat végigvezet egy új mező útján a WPF űrlaptól a backenden át az adminlistáig. A következő három feladatban ugyanezt a mintát alkalmazd önállóan.

1. [Helyiség megadása](task/01-helyiseg.md)
2. [Határidő kiválasztása](task/02-hatarido.md)
3. [Érintett eszközök száma](task/03-erintett-eszkozok.md)
4. [Hibakategória kiválasztása](task/04-hibakategoria.md)

## Projektstruktúra

```text
MyApp.sln
backend/MyApp.WebApi/       REST API, ReportService, EF Core InMemory
frontend/
  admin/
    maui/                  MAUI adminlista és lezárás (Windows)
    wpf/                   WPF adminlista és lezárás
    winui/                 WinUI 3 adminlista és lezárás
  client/
    maui/                  MAUI hibabejelentő (Windows)
    wpf/                   WPF hibabejelentő
    winui/                 WinUI 3 hibabejelentő
shared/MyApp.Shared/       CreateReportRequest és ReportResponse DTO-k
shared/MyApp.Client/       Közös API-service-ek és ViewModelek
```

A WPF-, MAUI- és WinUI-kliens közös `StudentReportsViewModel`-t használ. Az adminhoz az `AdminReportsViewModel` tartozik. A kliensoldali üzleti adatok egy leírásra, illetve egy listára korlátozódnak; az `IsBusy`, `HasError` és `StatusMessage` a műveletek visszajelzését kezeli.

## Indítás az iskolai gépen

A `global.json` a **10.0.403 SDK-t** választja. Minden projekt .NET 10-et céloz. A MAUI-projektek kizárólag Windowsra fordulnak, Android SDK, Java és `maui-android` workload nem szükséges.

A backend, WPF és WinUI futtatásához nem kell MAUI-workload. A MAUI Windows felületekhez egyszer szükséges:

```powershell
dotnet workload install maui-windows
```

Ez csak a Windows MAUI-eszközöket telepíti. Ha a gépen csak a .NET SDK van, a MAUI-projektek fordítása előtt ezt is telepíteni kell. A WinUI/MAUI fejlesztéshez Windows SDK is szükséges. Visual Studio használatakor .NET 10-et támogató Visual Studio 2026 szükséges.

A solution mappájából, külön terminálokban indítsd a backendet és a választott frontendet:

```powershell
dotnet --version
dotnet run --project backend/MyApp.WebApi/MyApp.WebApi.csproj --urls http://localhost:5222
dotnet run --project frontend/admin/wpf/MyApp.Admin.Wpf.csproj
dotnet run --project frontend/client/wpf/MyApp.Client.Wpf.csproj
```

WinUI 3 (x64):

```powershell
dotnet run --project frontend/admin/winui/MyApp.Admin.WinUI.csproj -p:Platform=x64
dotnet run --project frontend/client/winui/MyApp.Client.WinUI.csproj -p:Platform=x64
```

MAUI Windows, a `maui-windows` telepítése után:

```powershell
dotnet run --project frontend/admin/maui/MyApp.Admin.Maui.csproj
dotnet run --project frontend/client/maui/MyApp.Client.Maui.csproj
```

Teljes fordítás: `dotnet build MyApp.sln`. MAUI-workload nélkül a backend és a WPF/WinUI projektek külön is fordíthatók, például: `dotnet build frontend/client/wpf/MyApp.Client.Wpf.csproj`.

A kliensek `http://localhost:5222/` címen érik el a backendet. A MAUI köztes buildfájljai rövid, projektenként külön `%TEMP%/tc/fs01-net10-.../obj` mappába kerülnek.

Életjel: [http://localhost:5222/health](http://localhost:5222/health). Fejlesztői profilban: [Swagger](http://localhost:5222/swagger).
## REST API

| Művelet | Végpont | Válasz |
| --- | --- | --- |
| Beküldés | `POST /reports` | `201 Created`, létrehozott bejelentés |
| Adminlista | `GET /reports` | `200 OK`, legújabb rekord elöl |
| Lezárás | `POST /reports/{id}/close` | `200 OK`, frissített bejelentés |

Beküldés: `{ "description": "Nem működik a nyomtató." }`.

Válasz: `{ "id": 1, "description": "Nem működik a nyomtató.", "isClosed": false }`.

A leírás a szélső szóközök levágása után kötelező és legfeljebb 1000 karakter. Hibás adat: `400 Bad Request`. Nem létező rekord lezárása: `404 Not Found`. Az üzleti hibák `ProblemDetails` formájúak, `detail` szöveggel és `code` hibakóddal. Már lezárt rekord ismételt lezárása sikeres, az állapot továbbra is lezárt marad.

Az üres adminlista sikeres eredmény. Frissítéskor a teljes lista lecserélődik, nem duplázódik. Lezáráskor csak az érintett listaelem cserélődik a szerver válaszára. A sikeresen beküldött leírás kiürül; hiba esetén a beírt szöveg és a korábban betöltött lista megmarad.

## Gyors próba

1. Indítsd el a backendet, az admin WPF-et és valamelyik felhasználói klienst!
2. Frissítsd az adminlistát: induláskor üres, 0 bejelentéssel!
3. Küldd be: „Nem működik a nyomtató.” A kliens adjon sikerüzenetet és rekordazonosítót!
4. Az admin WPF-ben, MAUI-ban vagy WinUI-ban frissítsd a listát: az új bejelentés „Nyitott” állapotban látszik!
5. Kattints a sor **Lezárás** gombjára: az állapot azonnal változzon „Lezárva” értékre, a gomb tűnjön el!
6. Újabb frissítés után is maradjon lezárt. Küldj be másik hibát MAUI-ból is!
7. Próbálj üres leírást, illetve leállított backendet: legyen hibaüzenet, majd lehessen újra próbálkozni!

Az adatbázis memóriában tárolódik, ezért a backend újraindításakor a bejelentések és lezárások törlődnek. Feladat: [feladat.md](task/feladat.md). Folyamatábra: [draw.io](task/uzleti-folyamat.drawio), [SVG előnézet](task/uzleti-folyamat.svg).

Mindhárom adminfelület az `AdminReportsViewModel`, mindhárom felhasználói felület a `StudentReportsViewModel` osztályt használja. Próbáld ki a beküldést, a listafrissítést, a lezárást és a hibák megjelenítését mindhárom technológiával!

Microsoft források: [MAUI telepítés](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation), [MAUI 10 csomag](https://www.nuget.org/packages/Microsoft.Maui.Controls/10.0.110), [WinUI konfiguráció](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/unpackage-winui-app).
