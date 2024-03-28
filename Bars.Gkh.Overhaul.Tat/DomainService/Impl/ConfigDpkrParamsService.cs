namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;

    using Castle.Windsor;
    using Overhaul.DomainService;

    public class ConfigDpkrParamsService : IDpkrParamsService
    {
        private readonly List<string> keys = new List<string>
                                                 {
                                                     "ActualizePeriodStart", 
                                                     "ActualizePeriodEnd",
                                                     "ProgrammPeriodStart", 
                                                     "ProgrammPeriodEnd", 
                                                     "GroupByCeoPeriod", 
                                                     "GroupByRoPeriod", 
                                                     "ServiceCost", 
                                                     "YearPercent", 
                                                     "ShortTermProgPeriod", 
                                                     "PublicationPeriod"
                                                 };

        private IConfigProvider configProvider;

        public IWindsorContainer Container { get; set; }

        private IConfigProvider ConfigProvider
        {
            get
            {
                return configProvider ?? (configProvider = Container.Resolve<IConfigProvider>());
            }
        }

        public List<string> Keys {
            get
            {
                return keys;
            }
        }

        private string prefix = "Overhaul_";

        public Dictionary<string, string> SaveParams(BaseParams baseParams)
        {
            var config = ConfigProvider.GetConfig();

            var oldparams = GetParams();

            foreach (var key in keys)
            {
                if (baseParams.Params.ContainsKey(key))
                {
                    var paramKey = string.Format("{0}{1}", prefix, key);

                    if (config.AppSettings.ContainsKey(paramKey))
                    {
                        config.AppSettings[paramKey] = baseParams.Params.Get(key);
                    }
                    else
                    {
                        config.AppSettings.Add(paramKey, baseParams.Params.Get(key));
                    }
                }
            }

            ConfigProvider.SaveConfig(config);

            return oldparams;
        }

        public Dictionary<string, string> GetParams()
        {
            var config = ConfigProvider.GetConfig();
            var result = new Dictionary<string, string>();

            foreach (var key in keys)
            {
                var paramKey = string.Format("{0}{1}", prefix, key);

                if (config.AppSettings.ContainsKey(paramKey))
                {
                    result.Add(key, config.AppSettings[paramKey].ToStr());
                }
            }

            return result;
        }
    }
}