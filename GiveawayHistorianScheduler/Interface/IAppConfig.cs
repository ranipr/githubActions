using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveawayHistorianScheduler.Interface
{
    public interface IAppConfig<I>
    {
        I GetAppConfig(string[] configArg, I config);

        string GetPropertyInfoString(I config);
    }
}
