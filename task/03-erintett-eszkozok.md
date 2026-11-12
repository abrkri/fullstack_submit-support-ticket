# 03. Érintett eszközök száma

Adj az űrlaphoz **Érintett eszközök száma** mezőt. Ha például három számítógép nem indul, a felhasználó 3-at adjon meg. Az `AffectedDeviceCount` értéke 1 és 999 közötti egész szám legyen.

1. A **MyApp.Client.Wpf** projekt `MainWindow.xaml` fájljában adj hozzá egy beviteli mezőt. A **MyApp.Client** projekt `StudentReportsViewModel.cs` fájljában tárold a begépelt szöveget. A `SubmitReportAsync()` metódusban próbáld egész számmá alakítani a kérés összeállítása előtt. Üres értéknél, betűknél, törtszámnál vagy tartományon kívüli értéknél jeleníts meg hibaüzenetet, és ne induljon kérés. Sikeres mentés után ürítsd ki a mezőt.
2. A **MyApp.Shared** projekt `CreateReportRequest.cs` és `ReportResponse.cs` fájljában, valamint a **MyApp.WebApi** projekt `Report.cs` entitásában egész számként kezeld az adatot. A `ReportService.cs` fájlban a `CreateAsync()` az API-n közvetlenül érkező számnál is ellenőrizze az 1–999 tartományt. A `ToResponse()` adja vissza a mentett számot.
3. A **MyApp.Admin.Wpf** projekt `MainWindow.xaml` fájljában jelenítsd meg a számot külön oszlopban.

## Ellenőrzés

Próbáld ki az 1, 999, 0, 1000 és 2,5 értéket. Az első kettő kerüljön be az adminlistába, a többit utasítsa el a kliens. Swaggerből küldj be 0-t is. Az API adjon `400` választ.
