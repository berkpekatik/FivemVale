using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;
using Debug = CitizenFX.Core.Debug;

namespace Server
{
    public class Main : BaseScript
    {
        private static dynamic ESX;
        public Main()
        {

            EventHandlers["v_Vale:Pay"] += new Action<Player, int>(PayMoney);
            Tick += OnTick;
            Debug.WriteLine("#####################################");
            Debug.WriteLine("#");
            if (API.GetCurrentResourceName().ToLower() != "v_vale")
            {
                Debug.WriteLine("#Please fix the resource name as v_vale or functions may not work");
            }
            else
            {
                Debug.WriteLine("#All configuration sets!");
            }
            Debug.WriteLine("#Resource created by vNoisy");
            Debug.WriteLine("#");
            Debug.WriteLine("#####################################");
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
