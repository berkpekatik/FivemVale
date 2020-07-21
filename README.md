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
    "MenuTitle": "Vale Hizmeti",
    "MenuSubTitle": "Sana özel hizmet için hazırız",
    "NoCarGarage": "Garajda aracınız bulunmamaktadır.",
    "StoredTextNotReady": /*PLATENUMBER*/ "Plakalı Aracınız Çekilmişler Garajında!",
    "StoredTextReady": /*PLATENUMBER*/ "Plakalı Aracınız Garajda Hazır!",
    "WhileTransferFailing": "Konum bilgisi dogrulanamadi.",
    "ComplateText": "Aracınız Teslim Edildi.",
    "ValeAldreadyUsingError": "Aktif bir Vale hizmetin bulunmakta.",
    "ValeCannotUsingThisPos": "Malasef bulundugunuz bölgeye araç teslimat servisi yok.",
    "ValeOnTheWay": "Aracınız Yolda!",
    "ValePaidedText": "Vale Ücreti Alındı!",
    "FastValeCheckBoxName": "Hızlı Vale",
    "FastValeCheckBoxDescName": "Bu Seçenek Araçları size daha hızlı ulaştırır.",
    "NotEnoughMoney": "Paran yetersiz!",
    "CarBrokenError": "Aracın Hasar Gördüğü İçin Geri Ödeme Yapıldı!",
    "AbortJobTitle": "Vazgeç",
    "AbortJob": "Mevcut vale hizmetini iptal olur",
    "AbortJobError": "Vale Servisi İptal Edildi!"
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
