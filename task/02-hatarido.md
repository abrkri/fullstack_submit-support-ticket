# 02. Határidő kiválasztása

Adj a hibajegyhez **Határidő** mezőt `DueDate` néven. A felhasználó napot válasszon. A kliensben a holnapi vagy későbbi dátum fogadható el, a mai nap még hibás. A jövőbeli dátumra vonatkozó szabályt a kliens ellenőrizze.

1. A **MyApp.Client.Wpf** projekt `MainWindow.xaml` fájljában adj az űrlaphoz egy `DatePicker` vezérlőt. A kiválasztott dátumot kösd a **MyApp.Client** projekt `StudentReportsViewModel.cs` fájljában létrehozott `DueDate` tulajdonsághoz. A tulajdonság tudja tárolni azt is, hogy még nincs kiválasztott dátum.
2. A `SubmitReportAsync()` metódusban az API-hívás előtt ellenőrizd, hogy van-e kiválasztott dátum, és az a mai helyi naptári nap utánra esik-e. Hibánál adj érthető üzenetet, és ne induljon kérés. Sikeres mentés után töröld a választást.
3. A **MyApp.Shared** projektben vedd fel a dátumot a `CreateReportRequest.cs` és a `ReportResponse.cs` fájlban. A **MyApp.WebApi** projekt `Report.cs` entitása tárolja, a `ReportService.cs` fájlban a `CreateAsync()` mentse, a `ToResponse()` adja vissza. Az API-ban az érték lehessen üres, és a backend ne ellenőrizze, hogy jövőbeli-e.
4. A **MyApp.Admin.Wpf** projekt `MainWindow.xaml` fájljában jelenítsd meg a határidőt külön oszlopban, óra és perc nélkül.

## Ellenőrzés

Üres, tegnapi vagy mai dátummal a kliens adjon hibajelzést, és ne küldjön kérést. Holnapi dátummal a beküldés sikerüljön. Az adminlistában a választott dátum jelenjen meg, és a hibajegy lezárása után is látszódjon.
