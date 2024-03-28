namespace Bars.GisIntegration.Inspection
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Gkh.Service.Impl;
    using Bars.GisIntegration.Inspection.DataExtractors.Examination;
    using Bars.GisIntegration.Inspection.DataExtractors.InspectionPlan;
    using Bars.GisIntegration.Inspection.Dictionaries;
    using Bars.GisIntegration.UI.Service;
    using Bars.GkhGji.Entities;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GiInspectionPermissionMap>());

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
            this.Container.RegisterTransient<IInspectionService, InspectionService>();
        }

        public void RegisterDictionaries()
        {
            this.Container.RegisterDictionary<ExaminationFormDictionary>();
            this.Container.RegisterDictionary<ExaminationJurPersonFormDictionary>();
            this.Container.RegisterDictionary<ExaminationObjectDictionary>();
            this.Container.RegisterDictionary<ExaminationResultDocTypeDictionary>();
            this.Container.RegisterDictionary<OversightActivityTypeDictionary>();
            this.Container.RegisterDictionary<PrescriptionCloseReasonDictionary>();
            this.Container.RegisterDictionary<TypeBaseJurPersonDictionary>();
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<PlanJurPersonGji>, InspectionPlanExtractor>("InspectionPlanSelector");
            this.Container.RegisterTransient<IDataSelector<Disposal>, ExaminationExtractor>("ExaminationSelector");
        }

        public void RegisterDataExtractors()
        {
            this.Container.RegisterTransient<IDataExtractor<Examination>, InspectionPlanExaminationExtractor>("InspectionPlanExaminationExtractor");
            this.Container.RegisterTransient<IDataExtractor<InspectionPlan>, InspectionPlanExtractor>("InspectionPlanExtractor");

            this.Container.RegisterTransient<IDataExtractor<Examination>, ExaminationExtractor>("ExaminationExtractor");
            this.Container.RegisterTransient<IDataExtractor<ExaminationPlace>, ExaminationPlaceExtractor>("ExaminationPlaceExtractor");
            this.Container.RegisterTransient<IDataExtractor<ExaminationOtherDocument>, OtherActDocumentExtractor>("OtherActDocumentExtractor");
            this.Container.RegisterTransient<IDataExtractor<ExaminationOtherDocument>, OtherProtocolDocumentExtractor>("OtherProtocolDocumentExtractor");
            this.Container.RegisterTransient<IDataExtractor<Precept>, PreceptExtractor>("PreceptExtractor");
            this.Container.RegisterTransient<IDataExtractor<PreceptAttachment>, PreceptAttachmentExtractor>("PreceptAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<CancelPreceptAttachment>, CancelPreceptAttachmentExtractor>("CancelPreceptAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<Offence>, OffenceExtractor>("OffenceExtractor");
            this.Container.RegisterTransient<IDataExtractor<OffenceAttachment>, OffenceAttachmentExtractor>("OffenceAttachmentExtractor");
        }
    }
}