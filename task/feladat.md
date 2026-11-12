# 01. Hibabejelentés és lezárás

A felhasználó beküld egy hibaleírást, az admin pedig listázza és lezárja a bejelentést. Csak a `shared/MyApp.Client/ViewModels/StudentReportsViewModel.cs` és `AdminReportsViewModel.cs` fájlban dolgozz! A backend, az API-service-ek és a WPF-/MAUI-/WinUI-felületek készen vannak.

A propertyk neveit a kész XAML-felület kötései határozzák meg: a `Description` a bevitelt tárolja, amelyet a backend `CreateReportRequest.Description` mezőként vár, a `Reports` pedig a visszakapott lista. A `StatusMessage` az üzenetet, a `HasError` a piros hibajelzést, az `IsBusy` a folyamatban lévő műveletet jelzi; ezek csak a felülethez kellenek, nem küldjük őket a backendnek.

Minden API-hívásnál használj `await` és `try`–`catch`–`finally` szerkezetet! A `try` elején legyen `IsBusy = true` és `HasError = false`. `ApiException` esetén a `Message` értékét, egyéb `Exception` esetén a `Nem sikerült kapcsolódni a backendhez: {hibaüzenet}` szöveget írd a `StatusMessage` tulajdonságba, és állítsd a `HasError` értékét igazra! A `finally` blokkban legyen `IsBusy = false`.

Így a felület várakozás közben is működik, hiba után pedig újra lehet próbálkozni. A metódusokban a generált publikus tulajdonságokat használd!

## 1. StudentReportsViewModel

1. Hozd létre `[ObservableProperty]` attribútummal a `string description = string.Empty`, `string statusMessage = string.Empty`, `bool hasError` és `bool isBusy` mezőket!

   Ezekhez kötődik a beviteli mező, az üzenet és a művelet állapota.

2. Készíts `CanSubmit()` metódust `!IsBusy` visszatérési értékkel! A `private async Task SubmitReportAsync()` metódus kapjon `[RelayCommand(CanExecute = nameof(CanSubmit))]` attribútumot! Az `isBusy` mezőn `[NotifyCanExecuteChangedFor(nameof(SubmitReportCommand))]` értesítse a parancsot!

   Folyamatban lévő beküldés közben ne induljon újabb kérés.

3. A `SubmitReportAsync()` elején térj vissza, ha `IsBusy` igaz! A `try` elején legyen `IsBusy = true`, `HasError = false` és `StatusMessage = "Hibabejelentés beküldése..."`! A választ mentsd `created` változóba:

   ```csharp
   var created = await studentApiService.CreateReportAsync(
       new CreateReportRequest { Description = Description });
   ```

   A backend ellenőrzi és menti a leírást; a ViewModel csak a kérést és a visszajelzést kezeli.

4. Siker esetén ürítsd ki a `Description` értékét, és a `StatusMessage` tulajdonságba írd: `Sikeres hibabejelentés. Azonosító: {id}.` Az azonosítót a `created.Id` adja; az üzenetet stringinterpolációval állítsd össze!

   A beírt szöveg csak sikeres mentés után törlődjön.

5. A `SubmitReportAsync()` `try` blokkja után írd be az alábbi két `catch` ágat és a `finally` blokkot!

   ```csharp
   catch (ApiException exception) { HasError = true; StatusMessage = exception.Message; }
   catch (Exception exception) { HasError = true; StatusMessage = $"Nem sikerült kapcsolódni a backendhez: {exception.Message}"; }
   finally { IsBusy = false; }
   ```

   A két `catch` az API-hibát és az egyéb hibákat jelzi a felületen. A `finally` hiba esetén is visszaállítja az `IsBusy` értékét, így újra lehet próbálkozni.

## 2. AdminReportsViewModel

1. Készíts `[ObservableProperty]` mezőket: `ObservableCollection<ReportResponse> reports = []`, `string statusMessage = string.Empty`, `bool hasError` és `bool isBusy`!

   A `Reports` a táblázat adatforrása. A `ReportResponse` adatai: `Id`, `Description`, `IsClosed`.

2. A `CanLoad()` értéke legyen `!IsBusy`. A `CanClose(ReportResponse? report)` csak akkor legyen igaz, ha nincs folyamatban művelet, a paraméter nem `null`, és az `IsClosed` hamis. Készíts `private async Task LoadReportsAsync()` és `private async Task CloseReportAsync(ReportResponse? report)` metódust! Az első `[RelayCommand(CanExecute = nameof(CanLoad))]`, a második `[RelayCommand(CanExecute = nameof(CanClose))]` attribútumot kapjon! Az `isBusy` változásakor értesítsd a `LoadReportsCommand` és `CloseReportCommand` parancsot!

   Lezárni csak kiválasztott, nyitott bejelentést lehessen.

3. A `LoadReportsAsync()` elején térj vissza, ha `IsBusy` igaz! A `try` elején legyen `IsBusy = true`, `HasError = false`, az üzenet pedig `Hibabejelentések betöltése...`! A listát így cseréld le:

   ```csharp
   Reports = new ObservableCollection<ReportResponse>(await adminApiService.GetReportsAsync());
   ```

   Sikerüzenet: `{Reports.Count} hibabejelentés betöltve.`, stringinterpolációval.

   A teljes lista cseréje megakadályozza a sorok duplázódását. Az üres lista is sikeres eredmény.

4. A `CloseReportAsync()` elején térj vissza, ha a `CanClose(report)` hamis! A `try` elején legyen `IsBusy = true`, `HasError = false`, az üzenet pedig `Hibabejelentés lezárása...`! A választ mentsd el:

   ```csharp
   var closed = await adminApiService.CloseReportAsync(report!.Id);
   ```

   A `!` itt jelzi a fordítónak, hogy a korábbi ellenőrzés után a `report` nem `null`.

   A lezárást a backend menti; a megjelenítést a szerver válaszából frissítjük.

5. A `try` blokkon belül cseréld le a megfelelő listaelemet:

   ```csharp
   var previous = Reports.FirstOrDefault(r => r.Id == closed.Id);
   if (previous is not null) Reports[Reports.IndexOf(previous)] = closed;
   ```

   A `StatusMessage` legyen `A(z) {closed.Id}. hibabejelentés lezárva.`, stringinterpolációval! A `LoadReportsAsync()` és `CloseReportAsync()` metódus `try` blokkja után is az **1.5. pontban megadott `catch`–`finally` kódot** használd!

   A listaelem cseréjét a táblázat észleli: a sor „Lezárva” állapotot mutat, a Lezárás gomb eltűnik.

## 3. Próbáld ki!

1. Indítsd el a backendet és a két WPF-alkalmazást! Frissítsd az adminlistát: kezdetben 0 bejelentés legyen!
2. Küldd be: `Nem működik a nyomtató.` Az admin WPF-ben, MAUI-ban vagy WinUI-ban frissítsd a listát, és ellenőrizd a sikeres beküldést: a sor jelenjen meg „Nyitott” állapotban!
3. Zárd le a sort! Az állapot legyen „Lezárva”, a gomb tűnjön el, újabb listafrissítés után is maradjon lezárt!
4. Küldj be üres leírást, majd állítsd le a backendet, és próbálj újra műveletet indítani! A hibaüzenet jelenjen meg, a korábbi adatok maradjanak meg!
5. Próbáld ki a beküldést MAUI-ból és WinUI-ból is, majd a listázást és lezárást mindhárom adminfelületen! Ugyanazokat a ViewModelleket és API-t használják, mint a WPF-alkalmazások.

A leírás Trim után 1–1000 karakter. A backend újraindításakor az InMemory adatok törlődnek. Már lezárt rekord ismételt API-lezárása sikeres marad; a felület ezt a gomb elrejtésével előzi meg.

Folyamatábra: [szerkeszthető draw.io](uzleti-folyamat.drawio) · [előnézet](uzleti-folyamat.svg).
