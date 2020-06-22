using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Account
    {
        public int money { get; set; }
        public string name { get; set; }
        public string label { get; set; }
    }

    public class Inventory
    {
        public int count { get; set; }
        public bool usable { get; set; }
        public int limit { get; set; }
        public bool canRemove { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public int weight { get; set; }
        public bool rare { get; set; }
    }

    public class LastPosition
    {
        public double z { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Job
    {
        public List<object> skin_male { get; set; }
        public int grade_salary { get; set; }
        public List<object> skin_female { get; set; }
        public string grade_name { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public int grade { get; set; }
        public string grade_label { get; set; }
    }

    public class Coords
    {
        public double z { get; set; }
        public double heading { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }

    public class AccountModel
    {
        public List<Account> accounts { get; set; }
        public string identifier { get; set; }
        public List<Inventory> inventory { get; set; }
        public List<object> loadout { get; set; }
        public LastPosition lastPosition { get; set; }
        public int money { get; set; }
        public Job job { get; set; }
        public int maxWeight { get; set; }
        public Coords coords { get; set; }
    }
}
