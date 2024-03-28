namespace Bars.GisIntegration.CapitalRepair
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GisIntegration.CapitalRepair.DataExtractors;
    using Bars.GisIntegration.CapitalRepair.Dictionaries;
    using Bars.GkhCr.Entities;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GiCapitalRepairPermissionMap>());

            this.RegisterControllers();

            this.RegisterServices();

            this.RegisterDictionaries();

            this.RegisterDataExtractors();

            this.RegisterDataSelectors();
        }

        public void RegisterControllers()
        {
           
        }

        public void RegisterServices()
        {
            
        }

        public void RegisterDictionaries()
        {
            this.Container.RegisterDictionary<WorkTypeDictionary>();
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<ProgramCrProxy>, ProgramCrSelector>("ProgramCrSelector");
            this.Container.RegisterTransient<IDataSelector<BuildContract>, CapitalRepairContractsDataExtractor>("CapitalRepairContractsSelector");
        }

        public void RegisterDataExtractors()
        {
            this.Container.RegisterTransient<IDataExtractor<RisCrContract>, CapitalRepairContractsDataExtractor>("CapitalRepairContractsDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCrWork>, CapitalRepairWorksDataExtractor>("CapitalRepairWorksDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCrAttachContract>, CapitalRepairAttachContractDataExtractor>("CapitalRepairAttachContractDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCrAttachOutlay>, CapitalRepairAttachOutlayDataExtractor>("CapitalRepairAttachOutlayDataExtractor");
        }
    }
}
