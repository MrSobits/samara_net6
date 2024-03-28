using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh.UserActionRetention
{
    using B4;
    using B4.Modules.ExtJs;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/ActionKind.js", new ExtJsEnumResource<ActionKind>("B4.enums.ActionKind"));
        }
    }
}
