namespace Bars.Gkh.Decisions.Nso
{
	using Bars.B4;
	using Bars.B4.Modules.ExtJs;
	using Bars.Gkh.Decisions.Nso.Entities;
	using Bars.Gkh.Decisions.Nso.Entities.Decisions;
	using Bars.Gkh.Decisions.Nso.Enums;
	using Bars.Gkh.Enums.Decisions;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterExtJsModel<CreditOrgDecisionNotification>();
            container.RegisterExtJsEnum<MkdManagementDecisionType>();
            container.RegisterExtJsEnum<CrFundFormationDecisionType>();
            container.RegisterExtJsEnum<AccountOwnerDecisionType>();
            container.RegisterExtJsEnum<MethodFormFund>();

            container.RegisterExtJsModel<RealityObjectDecisionProtocol>().Controller("RealityObjectDecisionProtocol");
            container.RegisterExtJsModel<GenericDecision>()
                .Controller("GenericDecision")
                .AddProperty<string>("Name")
                .AddProperty<string>("Js");
            container.RegisterExtJsModel<GovDecision>().Controller("GovDecision").AddProperty<string>("MinFund");
        }
    }
}