namespace Bars.Gkh.Config.Impl
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl;

    using Castle.Windsor;

    public class ConfigGkhParams : IGkhParams
    {
        public IWindsorContainer Container { get; set; }

        private IConfigProvider ConfigProvider
        {
            get
            {
                return configProvider ?? (configProvider = Container.Resolve<IConfigProvider>());
            }
        }

        public List<string> Keys
        {
            get { return keys; }
        }

        #region Fields

        private IConfigProvider configProvider;

        private string prefix = "Gkh_";

        private readonly List<string> keys = new List<string>
        {
            "MoLevel",
            "ShowStlRealityGrid",
            "ShowUrbanAreaHigh",
            "RealEstTypeMoLevel",
            "ShowStlObjectCrGrid",
            "DI_SaveReport731ToDirectory",
            "WorkPriceMoLevel",
            "RegionName",
            "RegionOKATO",
            "DontKillRedisCache",
            "UseAdminOkrug",
            "ShowStlBuildContractGrid",
            "ShowStlDebtorGrid",
            "ShowStlClaimWork",
            ExecutionActionScheduler.ThreadCountKeyName,
            ExecutionActionScheduler.ThreadPriorityKeyName,
            ExecutionActionScheduler.AutoStartMandatoryKeyName
        };

        #endregion 

        public DynamicDictionary GetParams()
        {
            var config = ConfigProvider.GetConfig();
            var result = new DynamicDictionary();

            foreach (var key in keys)
            {
                var paramKey = string.Format("{0}{1}", prefix, key);

                if (config.AppSettings.ContainsKey(paramKey))
                {
                    var value = config.AppSettings[paramKey].ToString();

                    int intValue;
                    bool boolValue;
                    if (int.TryParse(value, out intValue))
                    {
                        result.Add(key, intValue);
                    }
                    else if (bool.TryParse(value, out boolValue))
                    {
                        result.Add(key, boolValue);
                    }
                    else
                    {
                        result.Add(key, config.AppSettings[paramKey].ToStr());
                    }
                }
            }

            return result;
        }

        public DynamicDictionary SaveParams(BaseParams baseParams)
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
    }
}