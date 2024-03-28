namespace Bars.Gkh.ClaimWork.Regions.Smolensk
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Utils;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterGkhEnum(LawsuitResultConsideration.PartiallySatisfied, LawsuitResultConsideration.Satisfied);
            container.RegisterExtJsEnum<LawsuitCollectionDebtDocumentType>();
            container.RegisterGkhEnum("LawsuitCollectionDebtDocumentTypeWithoutCourtOrder", LawsuitCollectionDebtDocumentType.CourtOrder);
        }
    }
}