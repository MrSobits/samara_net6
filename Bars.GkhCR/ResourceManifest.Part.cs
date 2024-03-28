namespace Bars.GkhCr
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Modules.ClaimWork.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Enums;

    /// <summary>
    /// The resource manifest.
    /// </summary>
    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            ResourceManifest.RegisterModels(container);
            ResourceManifest.RegisterEnums(container);
            ResourceManifest.RegisterServices(container);
        }

        private static void RegisterModels(IResourceManifestContainer container)
        {
            container.RegisterExtJsModel<ViewBuildContract>().Controller("JurJournalBuildContract").ListAction("List");
            container.RegisterExtJsModel<TerminationReason>("dict.TerminationReason").Controller("TerminationReason");
        }

        private static void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<BuildContractCreationType>();
            container.RegisterExtJsEnum<TypeWorkCrCountEstimations>();
            container.RegisterExtJsEnum<TypeWorkCrReason>();
            container.RegisterExtJsEnum<ReportPersonalAccountType>();
            container.RegisterExtJsEnum<ReportAccountType>();
            container.RegisterExtJsEnum<TypeWorkCrHistoryAction>();
            container.RegisterExtJsEnum<TypeFinance>();
            container.RegisterExtJsEnum<TypeFinanceGroup>();
            container.RegisterExtJsEnum<TypeProgramCr>();
            container.RegisterExtJsEnum<TypeVisibilityProgramCr>();
            container.RegisterExtJsEnum<TypeDocumentCr>();
            container.RegisterExtJsEnum<TypeContractBuild>();
            container.RegisterExtJsEnum<TypePaymentOrder>();
            container.RegisterExtJsEnum<TypeAcceptQualification>();
            container.RegisterExtJsEnum<TypeProgramStateCr>();
            container.RegisterExtJsEnum<AddWorkFromLongProgram>();
            container.RegisterExtJsEnum<TypeArchiveSmr>();
            container.RegisterExtJsEnum<TypeFinanceSource>();
            container.RegisterExtJsEnum<TypeChangeProgramCr>();
            container.RegisterExtJsEnum<ActPaymentType>();
            container.RegisterExtJsEnum<TypeCompetitionProtocol>();
            container.RegisterExtJsEnum<TypeBuilderSelection>();
            container.RegisterExtJsEnum<ActPaymentSrcFinanceType>();
            container.RegisterExtJsEnum<InspectionState>();
            container.RegisterExtJsEnum<FormFinanceSource>();
            container.RegisterExtJsEnum<TypeCheckWork>();
            container.RegisterExtJsEnum<TypeChecking>();
            container.RegisterExtJsEnum<TypeDefectListView>();
            container.RegisterExtJsEnum<TypeOtherFinSourceCalc>();
            container.RegisterExtJsEnum<AddTypeWorkKind>();
            container.RegisterExtJsEnum<EstimationTypeParam>();
            container.RegisterExtJsEnum<EstimationType>();
            container.RegisterExtJsEnum<DefectListUsage>();
            container.RegisterExtJsEnum<TypeDefectList>();
            container.RegisterExtJsEnum<AcceptType>();
            container.RegisterExtJsEnum<PerfWorkActPhotoType>();
            container.RegisterExtJsEnum<BuildContractState>();
        }

        private static void RegisterServices(IResourceManifestContainer container)
        {
            container.Add("WS/CrService.svc", "Bars.GkhCr.dll/Bars.GkhCr.Services.Service.svc");
            container.Add("WS/RestCrService.svc", "Bars.GkhCr.dll/Bars.GkhCr.Services.RestService.svc");
        }
    }
}