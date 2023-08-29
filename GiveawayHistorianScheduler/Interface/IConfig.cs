using GiveawayHistorianScheduler.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveawayHistorianScheduler.Interface
{
    public interface IConfig
    {
        [Sensitive(25)]
        [KubeArg("SqlServerConstr")]
        public string SqlServerConstr { get; set; }

        [KubeArg("SPName")]
        public string SpName { get; set; }

        
    }
}

