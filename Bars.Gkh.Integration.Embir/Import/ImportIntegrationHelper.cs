using Bars.B4.Config;
using Bars.B4.Utils;
using Bars.Gkh.Integration.DataFetcher;
using Castle.Windsor;

namespace Bars.Gkh.Integration.Embir.Import
{
    public class ImportIntegrationHelper
    {
        private IConfigProvider configProvider { get; set; }

        private string embirUrlKey = "Integration_EmbirUrl";

        public ImportIntegrationHelper(IWindsorContainer container)
        {
            configProvider = container.Resolve<IConfigProvider>();
        }

        public HttpQueryBuilder GetHttpQueryBuilder()
        {
            var config = configProvider.GetConfig();

            if (config.AppSettings.ContainsKey(embirUrlKey))
            {
                var address = config.AppSettings[embirUrlKey].ToString();

                var builder = new HttpQueryBuilder("{0}/Integration/GetData".FormatUsing(address));
                builder.Address = address;
                
                return builder;
            }

            return new HttpQueryBuilder(string.Empty);
        }
    }
}