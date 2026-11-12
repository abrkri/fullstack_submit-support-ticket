# 01. Helyiség megadása

A hibaleírás mellett add meg, hol történt a hiba, például „B épület, 204-es terem”. A **Helyiség** legyen külön mező a beküldő űrlapon és az adminlistában. Az új tulajdonság neve `Location`. Az érték kötelező, a szélső szóközök levágása után 1–100 karakter lehet.

| Projekt és fájl | Mit csinálj? |
|---|---|
| **MyApp.Shared** · [CreateReportRequest.cs](../shared/MyApp.Shared/Models/CreateReportRequest.cs), [ReportResponse.cs](../shared/MyApp.Shared/Models/ReportResponse.cs) | Add hozzá a `Location` szöveges tulajdonságot mindkét DTO-hoz. Az elsővel érkezik be az adat, a másodikkal jut vissza a felületekre. |
| **MyApp.WebApi** · [Report.cs](../backend/MyApp.WebApi/Models/Report.cs), [ReportService.cs](../backend/MyApp.WebApi/Services/ReportService.cs) | Add hozzá a `Location` tulajdonságot a `Report` entitáshoz. A `CreateAsync()` metódusban vágd le a szélső szóközöket, ellenőrizd a hosszt, és az érvényes értéket mentsd a hibajegyhez. Hibánál kövesd a leírás ellenőrzésének mintáját, saját hibakóddal és üzenettel. A `ToResponse()` metódusban másold át a helyiséget a válaszba. |
| **MyApp.Client** · [StudentReportsViewModel.cs](../shared/MyApp.Client/ViewModels/StudentReportsViewModel.cs) | A `Description` mintájára hozz létre felülethez köthető `Location` tulajdonságot. A `SubmitReportAsync()` metódusban add át az értékét a kérésben. Sikeres beküldés után ürítsd ki, hiba esetén hagyd meg a beírt értéket. |
| **MyApp.Client.Wpf** · [MainWindow.xaml](../frontend/client/wpf/MainWindow.xaml) | Adj az űrlaphoz egy egysoros, **Helyiség** feliratú `TextBox`-ot. A meglévő szövegmező mintájára kösd a ViewModel `Location` tulajdonságához. |
| **MyApp.Admin.Wpf** · [MainWindow.xaml](../frontend/admin/wpf/MainWindow.xaml) | Adj a `DataGrid`-hez **Helyiség** oszlopot. A kötés az aktuális `ReportResponse` sor `Location` értékét olvassa. Az `AdminReportsViewModel` már teljes válaszobjektumokat kezel, ezért a megjelenítéshez elég az új oszlop. |

## Ellenőrzés

1. A WPF kliensből küldj be egy hibajegyet szóközökkel körülvett helyiségnévvel. Frissítsd az adminlistát. A helyiség a szélső szóközök nélkül jelenjen meg.
2. Zárd le a hibajegyet, majd frissítsd újra a listát. A helyiség a lezárás után azonnal és a listafrissítés után is maradjon látható.
3. Swaggerből próbálj beküldeni üres, csak szóközökből álló és 100 karakternél hosszabb helyiségnevet. Az API mindhárom esetben `400` választ adjon, és az adminlistában ne jelenjen meg új hibajegy.
