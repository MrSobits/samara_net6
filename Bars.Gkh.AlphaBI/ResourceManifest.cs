namespace Bars.Gkh.AlphaBI
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
		
            RegisterResource(container, "libs/B4/controller/AlphaBi.js");
            RegisterResource(container, "libs/B4/view/AlphaBi.js");
        }             
    }
}
