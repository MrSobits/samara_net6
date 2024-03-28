using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Regions.Voronezh.Helpers
{
    using System.IO;
    using System.Reflection;

    public static class EmbeddedResourceHelper
    {
        public static string GetStringFromEmbedded(string fileLookup)
        {
            try
            {
                var list = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                var sqlList = list.Where(x => x.EndsWith(fileLookup));
                Stream sqlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sqlList.First());
                StreamReader sqlReader = new StreamReader(sqlStream);
                return sqlReader.ReadToEnd();
            }
            catch
            {
                //Suppress
            }

            return "";
        }
    }
}
