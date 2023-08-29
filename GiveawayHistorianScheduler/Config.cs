using GiveawayHistorianScheduler.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveawayHistorianScheduler
{
    public class Config : IConfig
    {
        public string SqlServerConstr { get; set; }
        public string SpName { get; set; }
    }
}
