# FivemVale


```diff
-Valet service is only used for vehicles and motorcycles
```
Config File
```json
{
  "ValePrice": 5000, //Valet Fee
  "FastValePrice": 5000, //Fast Valet Fee
  "MenuToggleKey": 246, //Menu Key you can search here https://docs.fivem.net/docs/game-references/controls/
  "PaymentMethod": "bank", //if you want cash write your payment type example money, black_money
  "FastValeService": true, //do quick valet service be active?
  "Locales": { //Please some one translate English LUL
    "MenuTitle": "Vale Hizmeti" //Valet Service
    "MenuSubTitle": "Sana özel hizmet için hazırız" // We are ready for special service for you
    "NoCarGarage": "Garajda aracınız bulunmamaktadır." //You have no vehicles in the garage
    "StoredTextNotReady": /*PLATENUMBER*/ "Plakalı Aracınız Çekilmişler Garajında!" //Your Plate Vehicle in the State Garage
    "StoredTextReady": /*PLATENUMBER*/ "Plakalı Aracınız Garajda Hazır!" //Your Plate Vehicle is Ready in the Garage
    "WhileTransferFailing": "Konum bilgisi dogrulanamadi." //Location information is not verified.
    "ComplateText": "Aracınız Teslim Edildi." //Your vehicle has been delivered
    "ValeAldreadyUsingError": "Aktif bir Vale hizmetin bulunmakta." //You have an active valet service
    "ValeCannotUsingThisPos": "Malasef bulundugunuz bölgeye araç teslimat servisi yok." //Sorry, no car service to your area
    "ValeOnTheWay": "Aracınız Yolda!" //Car on the way !
    "ValePaidedText": "Vale Ücreti Alındı!" // Valet money paid
    "FastValeCheckBoxName": "Hızlı Vale" //Speed Valet
    "FastValeCheckBoxDescName": "Bu Seçenek Araçları size daha hızlı ulaştırır." // These Option Tools deliver it to you faster.
    "NotEnoughMoney": "Paran yetersiz!" //Not enough money !
    "CarBrokenError": "Aracın Hasar Gördüğü İçin Geri Ödeme Yapıldı!" // Reimbursed for Damaged Vehicle!
    "AbortJobTitle": "Vazgeç" //Give up
    "AbortJob": "Mevcut vale hizmetini iptal olur" //Cancels the current valet service
    "AbortJobError": "Vale Servisi İptal Edildi!" //Valet Service Canceled!
  }
}

```


# Installation
[Download](https://github.com/vnoisy/FivemVale/releases) the latest version from the release section

Add your resources folder the file you downloaded

Change the config file

and then start the *v_vale* script

# Required:
[es_extended](https://github.com/esx-framework/es_extended)
[esx_advancedgarage](https://github.com/search?q=esx_advancedgarage)
* Why do i have to use it ESX_ADNVANCEDGARAGE  
if you don't want to use, Change the following in the code

```c#
TriggerServerEvent("esx_advancedgarage:setVehicleState", v.vehicle.plate, false);//370 Line in Vale/Main.cs
TriggerServerEvent("esx_advancedgarage:setVehicleState", v.vehicle.plate, false); //421 Line in Vale/Main.cs
TriggerServerEvent("esx_advancedgarage:setVehicleState", plate, true); //319 Line in Vale/Main.cs
ESX.TriggerServerCallback("esx_advancedgarage:getOwnedCars", new Action<dynamic>(ownedCars => //75 Line in Vale/Main.cs
```

# Problems
Sometimes Newtonsoft.Json.dll giving error or doesn't work, if you have a problem with Newtonsoft.Json.dll use [vMenu Newtonsoft.Json.dll](https://github.com/tomgrobbe/vMenu) 
