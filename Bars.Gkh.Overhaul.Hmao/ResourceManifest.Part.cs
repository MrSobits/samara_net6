﻿namespace Bars.Gkh.Overhaul.Hmao
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeUseShortProgramPeriod.js", new ExtJsEnumResource<TypeUseShortProgramPeriod>("B4.enums.TypeUseShortProgramPeriod"));
            container.Add("libs/B4/enums/AccountOperationType.js", new ExtJsEnumResource<AccountOperationType>("B4.enums.AccountOperationType"));
            container.Add("libs/B4/enums/TypeOrganization.js", new ExtJsEnumResource<TypeOrganization>("B4.enums.TypeOrganization"));
            container.Add("libs/B4/enums/TypePriority.js", new ExtJsEnumResource<TypePriority>("B4.enums.TypePriority"));
            container.Add("libs/B4/enums/PropertyOwnerDecisionType.js", new ExtJsEnumResource<PropertyOwnerDecisionType>("B4.enums.PropertyOwnerDecisionType"));
            container.Add("libs/B4/enums/MoOrganizationForm.js", new ExtJsEnumResource<MoOrganizationForm>("B4.enums.MoOrganizationForm"));
            container.Add("libs/B4/enums/RateCalcTypeArea.js", new ExtJsEnumResource<RateCalcTypeArea>("B4.enums.RateCalcTypeArea"));
            container.Add("libs/B4/enums/TypeUseWearMainCeo.js", new ExtJsEnumResource<TypeUseWearMainCeo>("B4.enums.TypeUseWearMainCeo"));
            container.Add("libs/B4/enums/PropertyOwnerProtocolType.js", new ExtJsEnumResource<PropertyOwnerProtocolType>("B4.enums.PropertyOwnerProtocolType"));
            container.Add("libs/B4/enums/OwnerAccountDecisionType.js", new ExtJsEnumResource<OwnerAccountDecisionType>("B4.enums.OwnerAccountDecisionType"));
            container.Add("libs/B4/enums/WorkPriceCalcYear.js", new ExtJsEnumResource<WorkPriceCalcYear>("B4.enums.WorkPriceCalcYear"));
            container.Add("libs/B4/enums/WorkPriceDetermineType.js", new ExtJsEnumResource<WorkPriceDetermineType>("B4.enums.WorkPriceDetermineType"));
            container.Add("libs/B4/enums/DpkrDataSource.js", new ExtJsEnumResource<DpkrDataSource>("B4.enums.DpkrDataSource"));
            container.Add("libs/B4/enums/TypeCorrectionActualizeRecs.js", new ExtJsEnumResource<TypeCorrectionActualizeRecs>("B4.enums.TypeCorrectionActualizeRecs"));
            container.RegisterExtJsEnum<TypeCorrection>();
            container.Add("libs/B4/enums/PriorityParamAdditionFactor.js", new ExtJsEnumResource<PriorityParamAdditionFactor>("B4.enums.PriorityParamAdditionFactor"));
            container.Add("libs/B4/enums/PriorityParamFinalValue.js", new ExtJsEnumResource<PriorityParamFinalValue>("B4.enums.PriorityParamFinalValue"));
            container.RegisterExtJsEnum<ProgramCrUsage>();
            container.RegisterExtJsEnum<VersionActualizeType>();
            container.RegisterExtJsEnum<Condition>();
            container.RegisterExtJsEnum<EconFeasibilityResult>();
            container.RegisterExtJsEnum<ChangeBasisType>();
            container.RegisterExtJsEnum<CriteriaType>();
            container.RegisterExtJsEnum<DpkrDocumentState>();
        }
    }
}