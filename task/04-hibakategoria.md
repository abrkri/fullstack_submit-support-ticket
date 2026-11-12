# 04. Hibakategória kiválasztása

Adj az űrlaphoz **Hibakategória** mezőt. A választható értékek **Számítógép**, **Nyomtató**, **Hálózat**, **Projektor** és **Egyéb**. Kezdetben egyik se legyen kiválasztva. Beküldéskor a kiválasztott kategória szövege kerüljön a hibajegybe, például „Nyomtató”.

1. A **MyApp.Shared** projektben hozz létre egy `ReportCategories.cs` fájlt a megengedett kategóriák közös listájával. A `CreateReportRequest.cs` és a `ReportResponse.cs` fájlhoz adj `Category` nevű szöveges tulajdonságot.
2. A **MyApp.WebApi** projekt `Report.cs` entitása tárolja a kategóriát. A `ReportService.cs` fájlban a `CreateAsync()` ellenőrizze, hogy a beküldött szöveg szerepel-e a közös listában. Hiányzó vagy ismeretlen értéknél adjon hibát. A `ToResponse()` másolja át a kategóriát a válaszba.
3. A **MyApp.Client** projekt `StudentReportsViewModel.cs` fájljában tedd elérhetővé a kategórialistát és a kiválasztott kategóriát. A `SubmitReportAsync()` jelezze, ha nincs kiválasztott érték. Egyébként küldje el a kiválasztott szöveget, és sikeres mentés után törölje a kijelölést.
4. A **MyApp.Client.Wpf** projekt `MainWindow.xaml` fájljában adj az űrlaphoz egy `ComboBox`-ot. Az `ItemsSource` a közös listához, a `SelectedItem` a kiválasztott kategóriához kötődjön. A felhasználó csak a listából választhasson.
5. A **MyApp.Admin.Wpf** projekt `MainWindow.xaml` fájljában adj a táblázathoz **Kategória** oszlopot, amely a hibajegy `Category` értékét jeleníti meg.

## Ellenőrzés

Küldj be egy hibát **Nyomtató** kategóriával, majd frissítsd az adminlistát és zárd le a hibajegyet. A kategória lezárás után is látszódjon. A kliens választás nélkül jelezzen hibát. Swaggerből küldj egy listán kívüli értéket, például „Monitor”. Az API adjon `400` választ.
