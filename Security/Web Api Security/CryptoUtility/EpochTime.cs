using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAFactory.Fx.Samples.WebApiSecurity.CryptoUtility
{
    public class EpochTime
    {
        public static string GetNow()
        {
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            return Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
        }
    }
}
