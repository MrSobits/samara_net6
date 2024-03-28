
namespace Bars.GkhDi.Regions.Chelyabinsk
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
            AddResource(container, "libs/B4/view/GeneralInfoRealityObj/EditPanel.js");
        }             

		private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhDi.Regions.Chelyabinsk.dll/Bars.GkhDi.Regions.Chelyabinsk.{0}", path.Replace("/", ".")));
        }
    }
}
