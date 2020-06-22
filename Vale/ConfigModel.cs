using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Locales
    {
        public string MenuTitle { get; set; }
        public string MenuSubTitle { get; set; }
        public string NoCarGarage { get; set; }
        public string StoredTextNotReady { get; set; }
        public string StoredTextReady { get; set; }
        public string WhileTransferFailing { get; set; }
        public string ComplateText { get; set; }
        public string ValeAldreadyUsingError { get; set; }
        public string ValeCannotUsingThisPos { get; set; }
        public string ValeOnTheWay { get; set; }
        public string ValePaidedText { get; set; }
        public string FastValeCheckBoxName { get; set; }
        public string FastValeCheckBoxDescName { get; set; }
    }

    public class ConfigModel
    {
        public int ValePrice { get; set; }
        public int FastValePrice { get; set; }
        public int MenuToggleKey { get; set; }
        public bool FastValeService { get; set; }
        public bool UpdateService { get; set; }
        public bool AutoUpdateService { get; set; }
        public Locales Locales { get; set; }
    }
}
