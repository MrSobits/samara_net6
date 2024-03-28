namespace Bars.GkhGji.Regions.Nso
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Nso.Entities.Protocol;
    using Bars.GkhGji.Regions.Nso.Enums;
    using Entities;

    using Gkh.Entities;
    using GkhGji.Entities.Dict;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<TypeDocumentGji>();
            container.RegisterExtJsEnum<TypeStage>();
            container.RegisterExtJsEnum<TypeReminder>();
            container.RegisterExtJsEnum<TypeRepresentativePresence>();
        }
    }
}
