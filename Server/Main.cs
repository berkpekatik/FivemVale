using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Main : BaseScript
    {
        private static dynamic ESX;
        public Main()
        {
            EventHandlers["v_Vale:Pay"] += new Action<Player,int>(PayMoney);
            Tick += OnTick;
            Debug.WriteLine("All configuration sets!");
        }

        private void PayMoney([FromSource] Player source,int amount)
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
