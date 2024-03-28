namespace Bars.GisIntegration.Gkh.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие создания настроек интеграции с ГИС
    /// </summary>
    public class GisIntegrationConfigsCreateAction : BaseExecutionAction
    {
        /// <summary>
        /// Контейнер IoC
        /// </summary>
        /// <summary>
        /// Домен-сервис "Настройка сервиса"
        /// </summary>
        public IDomainService<ServiceSettings> ServiceSettingsDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Настройка контекста"
        /// </summary>
        public IDomainService<ContextSettings> ContextSettingsDomain { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Сброс настроек сервисов интеграции с ГИС РФ";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Сброс настроек сервисов интеграции с ГИС РФ";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Migration;

        /// <summary>
        /// Метод миграции
        /// </summary>
        /// <returns>результат</returns>
        public BaseDataResult Migration()
        {
            this.Container.InTransaction(
                () =>
                {
                    this.UpdateServiceSettings();
                    this.UpdateContextSettings();
                });

            return new BaseDataResult();
        }

        private void UpdateServiceSettings()
        {
            this.ServiceSettingsDomain.GetAll().ToList().ForEach(x => this.ServiceSettingsDomain.Delete(x.Id));
            var items = this.ServiceSettingsDomain.GetAll().ToList();

            var gisIntegrationConfigs = new List<ServiceSettings>
            {
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Bills,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-bills-service/services/Bills",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-bills-service/services/BillsAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.DeviceMetering,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-device-metering-service/services/DeviceMetering",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-device-metering-service/services/DeviceMeteringAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.HouseManagement,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-home-management-service/services/HomeManagement",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-home-management-service/services/HomeManagementAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Nsi,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-nsi-service/services/Nsi",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-nsi-service/services/NsiAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.NsiCommon,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-nsi-common-service/services/NsiCommon",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-nsi-common-service/services/NsiCommonAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Organization,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-organization-service/services/Organization",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-organization-service/services/OrganizationAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.OrgRegistry,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-org-registry-service/services/OrgRegistry",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-org-registry-service/services/OrgRegistryAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.OrgRegistryCommon,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-org-registry-common-service/services/OrgRegistryCommon",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-org-registry-common-service/services/OrgRegistryCommonAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Inspection,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-inspection-service/services/Inspection",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-inspection-service/services/InspectionAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Infrastructure,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-rki-service/services/Infrastructure",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-rki-service/services/InfrastructureAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Licenses,
                    ServiceAddress = "",
                    AsyncServiceAddress = ""
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Payment,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-payment-service/services/Payment",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-payment-service/services/PaymentAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.CapitalRepair,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-capital-repair-programs-service/services/CapitalRepair",
                    AsyncServiceAddress = "http://127.0.0.1:8083/ext-bus-capital-repair-programs-service/services/CapitalRepairAsync"
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Disclosure,
                    ServiceAddress = "",
                    AsyncServiceAddress = ""
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.Fas,
                    ServiceAddress = "",
                    AsyncServiceAddress = ""
                },
                new ServiceSettings
                {
                    IntegrationService = IntegrationService.File,
                    ServiceAddress = "http://127.0.0.1:8083/ext-bus-file-store-service/rest",
                    AsyncServiceAddress = ""
                }
            };

            gisIntegrationConfigs.ForEach(x => this.ServiceSettingsDomain.Save(x));
        }

        private void UpdateContextSettings()
        {
            this.ContextSettingsDomain.GetAll().ToList().ForEach(x => this.ContextSettingsDomain.Delete(x.Id));
            var items = this.ContextSettingsDomain.GetAll().ToList();

            var defaultSettings = new List<ContextSettings>
            {
                new ContextSettings
                {
                    FileStorageName = FileStorageName.HomeManagement,
                    Context = "homemanagement"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Rki,
                    Context = "rki"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Voting,
                    Context = "voting"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Inspection,
                    Context = "inspection"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Informing,
                    Context = "informing"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Bills,
                    Context = "bills"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Licenses,
                    Context = "licenses"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Agreements,
                    Context = "agreements"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Nsi,
                    Context = "nsi"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.Disclosure,
                    Context = "disclosure"
                },
                new ContextSettings
                {
                    FileStorageName = FileStorageName.CapitalRepairPrograms,
                    Context = "capitalrepairprograms"
                }
            };

            defaultSettings.ForEach(x => this.ContextSettingsDomain.Save(x));
        }
    }
}