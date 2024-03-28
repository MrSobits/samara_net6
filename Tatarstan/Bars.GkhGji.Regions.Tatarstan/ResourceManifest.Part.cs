namespace Bars.GkhGji.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.B4.Utils;
    // TODO : Расскоментировать после реализации GisIntegration.Tor
   //using Bars.GisIntegration.Tor.Enums;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using System.Linq;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeExecutantProtocol.js", new ExtJsEnumResource<TypeDocObject>("B4.enums.TypeExecutantProtocol"));
            container.Add("libs/B4/enums/DecisionTypeAgreementProsecutor.js",
                new ExtJsEnumResource<TypeAgreementProsecutor>("B4.enums.DecisionTypeAgreementProsecutor"));

            container.RegisterGkhEnum("TypeJurPersonAction",
                TypeJurPerson.Builder,
                TypeJurPerson.LocalGovernment,
                TypeJurPerson.PoliticAuthority,
                TypeJurPerson.RegOp,
                TypeJurPerson.RenterOrg,
                TypeJurPerson.ServOrg,
                TypeJurPerson.ServiceCompany);
            container.RegisterGkhEnum("TypeObjectAction", TypeDocObject.Entrepreneur);
            container.RegisterGkhEnum("TatarstanInspectionFormType", TypeFormInspection.Visual, TypeFormInspection.ExitAndDocumentary);
            container.RegisterGkhEnum("TypeFormAppealCitsTat", TypeFormInspection.ExitAndDocumentary);
            container.RegisterGkhEnum("ActActionType", ActCheckActionType.GettingWrittenExplanations, ActCheckActionType.RequestingDocuments, ActCheckActionType.Survey);

            container.RegisterExtJsEnum<ProsecutorOfficeType>();
            container.RegisterExtJsEnum<NotificationType>();
            container.RegisterExtJsEnum<ControlLevel>();
            // TODO : Расскоментировать после реализации GisIntegration.Tor
           // container.RegisterExtJsEnum<TypeObject>();
           // container.RegisterExtJsEnum<TypeRequest>();
           // container.RegisterExtJsEnum<TorTaskState>();
            container.RegisterExtJsEnum<DictTypes>();
            container.RegisterExtJsEnum<WitnessType>();
            container.RegisterExtJsEnum<KindAction>();
            container.RegisterExtJsEnum<TypeBaseAction>();
            container.RegisterExtJsEnum<ActCheckActionType>();
            container.RegisterExtJsEnum<ActCheckActionCarriedOutEventType>();
            container.RegisterExtJsEnum<PreventiveActionType>();
            container.RegisterExtJsEnum<PreventiveActionVisitType>();
            container.RegisterExtJsEnum<PreventiveActionCounselingType>();
            container.RegisterExtJsEnum<RequestInfoType>();
            container.RegisterExtJsEnum<ActActionIsolatedDefinitionType>();
            container.RegisterExtJsEnum<InspectionCreationBasis>();
            container.RegisterExtJsEnum<MotivatedPresentationType>();
            container.RegisterExtJsEnum<MotivatedPresentationResultType>();

            container.RegisterBaseDictController<ObjectivesPreventiveMeasure>("Цели профилактических мероприятий", "GkhGji.Dict.ObjectivesPreventiveMeasures");
            container.RegisterBaseDictController<PreventiveActionItems>("Предметы профилактических мероприятий", "GkhGji.Dict.PreventiveActionItems");
            container.RegisterBaseDictController<TasksPreventiveMeasures>("Задачи профилактических мероприятий", "GkhGji.Dict.TasksPreventiveMeasures");
            container.RegisterBaseDictController<TatRiskCategory>("Категории риска", "GkhGji.Dict.RiskCategory");

            container.RegisterVirtualExtJsEnum("BaseTypeForMotivatedPresentation",
                new[]
                {
                    TypeBase.ActionIsolated,
                    TypeBase.PreventiveAction
                }.Select(x =>
                    new EnumMemberView
                    {
                        Name = x.ToString(),
                        Display = x.GetDisplayName(),
                        Description = x.GetDescriptionName(),
                        Value = (int)x
                    }));
        }
    }
}