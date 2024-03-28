namespace Bars.Gkh.ClaimWork.Regions.Smolensk
{
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CourtClaimInfoPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CollectionPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CourtClaimInfoPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/MainInfoPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPEditWindow.js");
            this.RegisterResource(container, "libs/B4/controller/dict/PaymentDocInfo.js");
            this.RegisterResource(container, "libs/B4/controller/claimwork/Lawsuit.js");
            this.RegisterResource(container, "libs/B4/view/dict/paymentdocinfo/Grid.js");
            this.RegisterResource(container, "libs/B4/view/ExtractEgrn/Grid.js");
        }
    }
    
}