namespace Bars.B4.Modules.Analytics.Web
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
            RegisterResource(container, "libs/B4/controller/al/DataSource.js");
            RegisterResource(container, "libs/B4/view/al/DataSourceEdit.js");
            RegisterResource(container, "libs/B4/view/al/DataSourceGrid.js");
        }             
    }
}
