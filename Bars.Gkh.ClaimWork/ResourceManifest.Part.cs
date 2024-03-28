namespace Bars.Gkh.ClaimWork
{
    using B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            // RegisterExtJsModel
            container.RegisterExtJsModel<PetitionToCourtType>("PetitionToCourt").Controller("PetitionToCourtType");
        }
    }
}