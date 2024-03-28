using Bars.B4.Config;
using Bars.B4.Utils;
using Castle.Windsor;
using SMEV3Library.Entities;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SMEV3Library.Providers
{
    internal class ConfigOptionsProvider : IOptionsProvider
    {
        #region Fields

        IWindsorContainer _container;

        #endregion

        #region Constructors

        public ConfigOptionsProvider(IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("SMEV3Service не зарегистрирован в Windsor, но используется");

            _container = container;
        }

        #endregion

        public SMEVOptions GetOptions()
        {
            var configProvider = _container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("SMEV3Library");

            return new SMEVOptions
            {
                //TestMode
                TestMode = config.GetAs("TestMode", true, true),
                //Thumbprint
                Thumbprint = config.GetAs("Thumbprint", "‎be6bfa913149e26cc4ce3e1531f9fcc547302f2e", true).Replace("\u200B", ""),

                HeaderThumbprint = config.GetAs("HeaderThumbprint", "‎be6bfa913149e26cc4ce3e1531f9fcc547302f2e", true).Replace("\u200B", ""),

                ThumbprintPers = config.GetAs("EmployerThumbprint", "‎be6bfa913149e26cc4ce3e1531f9fcc547302f2e", true).Replace("\u200B", ""),
                //StoreLocation
                StoreLocation = GetStoreLocation(config),
                //Store
                Storage = config.GetAs("Store", "‎MY", true).Replace("\u200B", ""),
                //Endpoint
                Endpoint = config.GetAs("Endpoint", @"http://smev3-n0.test.gosuslugi.ru:7500/smev/v1.2/ws", true).Replace("\u200B", "")
            };
        }

        StoreLocation GetStoreLocation(DynamicDictionary config)
        {
            string storeLocation = config.GetAs("StoreLocation", "‎localmachine", true).ToLower().Trim();
            if (storeLocation.Contains("localmachine"))
                return StoreLocation.LocalMachine;
            else if (storeLocation.Contains("currentuser"))
                return StoreLocation.CurrentUser;
            else throw new ApplicationException($"Тип /'{storeLocation}/' не распознан, параметр StoreLocation секции конфига SMEV3Library");
        }
    }
}
