using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Threading.Tasks;
using CitizenFX.Core.UI;
using MenuAPI;
using static MenuAPI.MenuItem;
using Newtonsoft.Json;
using static MenuAPI.MenuCheckboxItem;
using System.Collections.Generic;
using Client.Models;
using System.Linq;

namespace Client
{
    public class Main : BaseScript
    {
        private static int blip;
        private static bool eventStart = false;
        private static bool fastVale = false;
        private static Menu menu;
        private static Ped driver;
        private static int driverId;
        private static Vector3 spawnPos;
        private static Vector3 targetLoc;
        private static Vector3 testLoc;
        private static dynamic ESX;
        private static int networkCar;
        private static string plate;
        private static ConfigModel config = new ConfigModel();
        private static MenuCheckboxItem box;
        private static int price;
        public Main()
        {
            Tick += OnTick;
            Tick += OnNoDelayTick;
            Tick += OnMenuDelayTick;
            var data = LoadResourceFile(GetCurrentResourceName(), "config.json");
            try
            {
                config = JsonConvert.DeserializeObject<ConfigModel>(data);
                price = config.ValePrice;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Config cannot loaded!!");
                Debug.WriteLine("Bcs: " + e.Message);
            }
            MenuSelector();
        }
        private void MenuSelector()
        {
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

            menu = new Menu(config.Locales.MenuTitle, config.Locales.MenuSubTitle);

            menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
            {
                if (_item == box)
                {
                    if (_checked)
                    {
                        _item.Text = $"{config.Locales.FastValeCheckBoxName} +{config.FastValePrice} $";
                    }
                    else
                    {
                        _item.Text = config.Locales.FastValeCheckBoxName;
                    }
                }
            };

            menu.OnMenuOpen += (_menu) =>
            {
                _menu.ClearMenuItems();
                ESX.TriggerServerCallback("esx_advancedgarage:getOwnedCars", new Action<dynamic>(ownedCars =>
                {
                    var count = ownedCars.Count;
                    if (count == 0)
                    {
                        ShowNoti(config.Locales.NoCarGarage);
                        return;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        var v = ownedCars[i];
                        try
                        {
                            var hashCar = (uint)v.vehicle.model;
                            var aheadVehName = GetDisplayNameFromVehicleModel(hashCar);
                            var vehicleName = GetLabelText(aheadVehName);
                            var storedText = config.Locales.StoredTextNotReady;
                            if (v.stored) storedText = config.Locales.StoredTextReady;

                            var model = new Model((int)hashCar);

                            if (!model.IsBicycle)
                            {
                                var menuItem = new MenuItem(vehicleName)
                                {
                                    Description = v.plate + " " + storedText,
                                    LeftIcon = model.IsBike ? Icon.BIKE : Icon.CAR,
                                    ItemData = v,
                                    Enabled = v.stored,
                                };
                                menu.AddMenuItem(menuItem);
                            }
                        }

                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            Debug.WriteLine("Error with this vehicle: " + v.vehicle.model.ToString());
                        }
                    }

                }));
                if (config.FastValeService)
                {
                    box = new MenuCheckboxItem(config.Locales.FastValeCheckBoxName, config.Locales.FastValeCheckBoxDescName, false);
                    box.Style = CheckboxStyle.Tick;
                    menu.AddMenuItem(box);
                }

            };

            menu.OnItemSelect += (_onMenu, _item, _index) =>
            {
                if (config.FastValeService && box.Checked)
                {
                    price = config.ValePrice;
                    price += config.FastValePrice;
                    if (!ControlMoney(config.PaymentMethod, price))
                    {
                        ShowNoti(config.Locales.NotEnoughMoney);
                        return;
                    }
                    FastVale(_item.ItemData);
                }
                else
                {
                    if (!ControlMoney(config.PaymentMethod, price))
                    {
                        ShowNoti(config.Locales.NotEnoughMoney);
                        return;
                    }
                    price = config.FastValePrice;
                    Vale(_item.ItemData);
                }
            };
            MenuController.MenuToggleKey = (Control)config.MenuToggleKey;
            MenuController.AddMenu(menu);
        }

        private async Task OnMenuDelayTick()
        {
            if (MenuController.IsAnyMenuOpen())
            {
                var pos = Game.PlayerPed.Position;
                var pHeading = Game.PlayerPed.Heading;
                testLoc = new Vector3();
                GetRoadSidePointWithHeading(pos.X, pos.Y, pos.Z, pHeading, ref testLoc);
                await Delay(500);
            }
        }
        private async Task OnNoDelayTick()
        {
            if (MenuController.IsAnyMenuOpen())
            {
                DrawMarker(36, testLoc.X, testLoc.Y, testLoc.Z + 1f, 0f, 0f, 0f, 0f, 0f, 0f, 2f, 2f, 2f, 255, 255, 255, 255, false, false, 2, true, null, null, false);
            }
            if (eventStart && driver != null)
            {
                //driverType = new Model(GetEntityModel(networkCar)).IsBike ? 37 : 36;
                DrawMarker(36, targetLoc.X, targetLoc.Y, targetLoc.Z + 1f, 0f, 0f, 0f, 0f, 0f, 0f, 2f, 2f, 2f, 255, 255, 255, 255, false, false, 2, true, null, null, false);
                if (driver != null && driver.IsInVehicle())
                {
                    DrawMarker(0, driver.CurrentVehicle.Position.X, driver.CurrentVehicle.Position.Y, driver.CurrentVehicle.Position.Z + 1.5f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 1f, 1f, 255, 255, 255, 255, true, false, 2, true, null, null, false);
                }
            }
            if (eventStart && fastVale)
            {
                DrawMarker(36, targetLoc.X, targetLoc.Y, targetLoc.Z + 1f, 0f, 0f, 0f, 0f, 0f, 0f, 2f, 2f, 2f, 255, 255, 255, 255, false, false, 2, true, null, null, false);
            }
        }
        private async Task OnTick()
        {
            while (ESX == null)
            {
                TriggerEvent("esx:getSharedObject", new object[] { new Action<dynamic>(esx => {
                    ESX = esx;
                })});
                await Delay(1000);
            }

            if (eventStart && driver != null && driver.IsInVehicle())
            {
                await Delay(500);
                Status();
            }

            if (eventStart && fastVale)
            {
                await Delay(500);
                FastStatus();
            }
        }
        private void Status()
        {
            var pos = Game.PlayerPed.Position;
            if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, driver.Position.X, driver.Position.Y, driver.Position.Z, false) > 150f)
            {
                ShowNoti(config.Locales.WhileTransferFailing);
                Abort();
            }

            if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, driver.Position.X, driver.Position.Y, driver.Position.Z, false) < 2f)
            {
                //driver.Task.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
                ShowNoti(config.Locales.ComplateText);
                TriggerServerEvent("v_Vale:Pay", config.PaymentMethod, price);
                ShowNoti(price + "$ " + config.Locales.ValePaidedText);
                Complate();
            }
        }
        private void FastStatus()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                HalfAbort();
            }
        }
        private void Abort()
        {
            RemoveBlip(ref blip);
            eventStart = false;
            DeleteEntity(ref networkCar);
            DeleteEntity(ref driverId);
            driver = null;
            TriggerServerEvent("esx_advancedgarage:setVehicleState", plate, true);
            return;
        }
        private void Complate()
        {

            SetVehicleDoorsLocked(driver.CurrentVehicle.Handle, 1);
            RemoveBlip(ref blip);
            eventStart = false;
            DeleteEntity(ref driverId);
            driver = null;
            return;
        }
        private void HalfAbort()
        {
            RemoveBlip(ref blip);
            eventStart = false;
            fastVale = false;
            return;
        }
        private void FastVale(dynamic v)
        {
            if (eventStart)
            {
                ShowNoti(config.Locales.ValeAldreadyUsingError);
                return;
            }
            var pos = Game.PlayerPed.Position;
            var pHeading = Game.PlayerPed.Heading;
            targetLoc = new Vector3();
            //GetClosestVehicleNode(pos.X, pos.Y, pos.Z, ref targetLoc, 1, 0, 2.5f);
            GetRoadSidePointWithHeading(pos.X, pos.Y, pos.Z, pHeading, ref targetLoc);
            //GetClosestVehicleNodeWithHeading(pos.X, pos.Y, pos.Z, ref targetLoc, ref pHeading, 1, 3.0F, 0);
            ESX.Game.SpawnVehicle(v.vehicle.model, targetLoc, pHeading, new Action<dynamic>(callback_vh =>
            {
                networkCar = (int)callback_vh;
                blip = AddBlipForEntity((int)callback_vh);
                SetBlipColour(blip, 40);
                BeginTextCommandSetBlipName("STRING");
                AddTextComponentString("Vale");
                EndTextCommandSetBlipName(blip);
                SetVehicleOnGroundProperly(callback_vh);
            }));

            TriggerServerEvent("esx_advancedgarage:setVehicleState", v.vehicle.plate, false);
            ShowNoti(config.Locales.ComplateText);
            TriggerServerEvent("v_Vale:Pay", config.PaymentMethod, price);
            ShowNoti(price + "$ " + config.Locales.ValePaidedText);
            menu.CloseMenu();
            eventStart = true;
            fastVale = true;
        }
        private async void Vale(dynamic v)
        {
            if (eventStart)
            {
                ShowNoti(config.Locales.ValeAldreadyUsingError);
                return;
            }

            var pos = Game.PlayerPed.Position;
            var pHeading = Game.PlayerPed.Heading;
            spawnPos = new Vector3();
            var spawnHeading = 0F;
            var unused = 0;
            GetNthClosestVehicleNodeWithHeading(pos.X, pos.Y, pos.Z, 80, ref spawnPos, ref spawnHeading, ref unused, 9, 3.0F, 2.5F);


            var pedModel = new Model(PedHash.Andreas);
            driver = await World.CreatePed(pedModel, spawnPos, spawnHeading);
            driverId = driver.Handle;
            targetLoc = new Vector3();
            GetRoadSidePointWithHeading(pos.X, pos.Y, pos.Z, pHeading, ref targetLoc);
            //GetClosestVehicleNodeWithHeading(pos.X, pos.Y, pos.Z, ref targetLoc, ref pHeading, 1, 3.0F, 0);
            //GetClosestVehicleNode(pos.X, pos.Y, pos.Z, ref targetLoc, 1, 0, 2.5f);
            await Delay(100);

            ESX.Game.SpawnVehicle(v.vehicle.model, spawnPos, spawnHeading, new Action<dynamic>(async callback_vh =>
            {
                SetVehicleDoorsLocked((int)callback_vh, 2);
                ESX.Game.SetVehicleProperties(callback_vh, v.vehicle);
                blip = AddBlipForEntity(networkCar);
                SetBlipColour(blip, 40);
                BeginTextCommandSetBlipName("STRING");
                AddTextComponentString("Vale");
                EndTextCommandSetBlipName(blip);
                SetVehicleOnGroundProperly(networkCar);
                await Delay(100);
                //TaskEnterVehicle(driver.Handle, networkCar, -1, -1, 1f, 16, 0);
                TaskWarpPedIntoVehicle(driverId, networkCar, -1);
                await Delay(100);
                TaskVehicleDriveToCoord(driverId, networkCar, targetLoc.X, targetLoc.Y, targetLoc.Z, 15f, 1, 0, 782, 2f, 1f);
            }));

            TriggerServerEvent("esx_advancedgarage:setVehicleState", v.vehicle.plate, false);
            plate = v.vehicle.plate.ToString();
            menu.CloseMenu();

            if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, driver.Position.X, driver.Position.Y, driver.Position.Z, false) > 150f)
            {
                ShowNoti(config.Locales.ValeCannotUsingThisPos);
                Abort();
                return;
            }
            ShowNoti(config.Locales.ValeOnTheWay);
            //networkCar = GetVehiclePedIsIn(driver.Handle, false);
            eventStart = true;
        }

        private bool ControlMoney(string type, int amount)
        {
            try
            {
                string accountData = JsonConvert.SerializeObject(ESX.GetPlayerData());
                var account = JsonConvert.DeserializeObject<AccountModel>(accountData);
                return account.accounts.Where(x => x.name == type && x.money >= amount).Any();
            }
            catch (Exception e)
            {
                return false;
            }

        }
        private void ShowNoti(string text)
        {
            Screen.ShowNotification(text);
            //ESX.ShowNotification(text);
        }
    }
}
