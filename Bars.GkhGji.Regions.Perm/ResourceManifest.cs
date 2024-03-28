namespace Bars.GkhGji.Regions.Perm
{
    using Bars.B4;

    /// <summary>
    /// Манифест ресурсов.
    /// Используется для регистрации ресурсов модуля в общем контейере ресурсов.
    /// </summary>
    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {

            this.RegisterResource(container, "libs/B4/controller/ActCheck.js");
            this.RegisterResource(container, "libs/B4/view/disposal/EditPanel.js");
            this.RegisterResource(container, "libs/B4/view/documentsgjiregister/DisposalGrid.js");

        }
    }
}
