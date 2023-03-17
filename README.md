# FivemVale


```diff
-Valet service is only used for vehicles and motorcycles
```
Config File (Release version is Turkish)
```json
{
  "ValePrice": 5000, /* Valet Price */
  "FastValePrice": 5000, /* Fast Valet Price */
  "MenuToggleKey": 246, /* Menu Key Value currently Y */
  "PaymentMethod": "bank", /* Payment type if you want cash write money */
  "FastValeService": true, /* Fast Valet Active or Passive true/false */
  "LockVehicle": false, /* Do you want to lock spawned car */
  "Locales": {
    "MenuTitle": "Valet Service",
    "MenuSubTitle": "We are ready for special service for you",
    "NoCarGarage": "You have no vehicles in the garage.",
    "StoredTextNotReady": "Plate Vehicle in the State Garage",
    "StoredTextReady": "Plate Vehicle is Ready in the Garage",
    "WhileTransferFailing": "Location information is not verified.",
    "ComplateText": "Your vehicle has been delivered.",
    "ValeAldreadyUsingError": "You have an active valet service.",
    "ValeCannotUsingThisPos": "Sorry, no car service to your area",
    "ValeOnTheWay": "Car on the way !",
    "ValePaidedText": "Valet money paid!",
    "FastValeCheckBoxName": "Fast Valet",
    "FastValeCheckBoxDescName": "These Option Tools deliver it to you faster.",
    "NotEnoughMoney": "Not enough money!",
    "CarBrokenError": "Reimbursed for Damaged Vehicle!",
    "AbortJobTitle": "Cancel",
    "AbortJob": "Cancels the current valet service",
    "AbortJobError": "Valet Service Canceled!"
  }
}
```

# Videos
[Valet Service #1](https://streamable.com/77c65a)
[Fast Valet Service #1](https://streamable.com/vxsdvs)

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

Sometimes the doors of the vehicle in the valet service do not open

