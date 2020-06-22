using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using Debug = CitizenFX.Core.Debug;

namespace Server
{
    public class Main : BaseScript
    {
        private static dynamic ESX;
        private static ConfigModel config = new ConfigModel();
        private static VersionModel updateModel = new VersionModel();
        private string version = "0.1";
        public Main()
        {
            EventHandlers["v_Vale:Pay"] += new Action<Player, int>(PayMoney);
            Tick += OnTick;
            Debug.WriteLine("#####################################");
            Debug.WriteLine("#All configuration sets!");
            Debug.WriteLine("#Version Controlling");
            VersionController();
            Debug.WriteLine("#####################################");
        }
        private void VersionController()
        {
            var data = LoadResourceFile(GetCurrentResourceName(), "config.json");
            try
            {
                config = JsonConvert.DeserializeObject<ConfigModel>(data);
                if (config.UpdateService)
                {
                    var ver = new WebClient().DownloadString("http://berkpekatik.com/app/version.json");
                    updateModel = JsonConvert.DeserializeObject<VersionModel>(ver);
                    if (version != updateModel.Version)
                    {
                        Debug.WriteLine("#Your plugin is outdate needs to update " + version);
                        if (config.AutoUpdateService)
                        {
                            Debug.WriteLine("#Dont worry i will update for you ;)");
                            DownloadFile();
                        }
                    }
                    else
                    {
                        Debug.WriteLine("#All thing uptodate!");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("#Config cannot loaded!!");
                Debug.WriteLine("#Bcs: " + e.Message);
            }
        }
        public void DownloadFile()
        {
            var wc = new WebClient();
            wc.DownloadFile(updateModel.DownloadLink, GetResourcePath(GetCurrentResourceName()) + @"/vale_" + updateModel.Version + ".zip");
        }
        private void PayMoney([FromSource] Player source, int amount)
        {
            try
            {
                var xPlayer = ESX.GetPlayerFromId(source.Handle);
                xPlayer.removeMoney(amount);
            }
            catch
            {
                Debug.WriteLine("This server cannot support ESX");
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
        }
    }
}
