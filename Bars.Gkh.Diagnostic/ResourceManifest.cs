
namespace Bars.Gkh.Diagnostic
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/diagnostic/CollectedDiagnosticResult.js");
            AddResource(container, "libs/B4/model/diagnostic/CollectedDiagnosticResult.js");
            AddResource(container, "libs/B4/model/diagnostic/DiagnosticResult.js");
            AddResource(container, "libs/B4/store/diagnostic/CollectedDiagnosticResult.js");
            AddResource(container, "libs/B4/store/diagnostic/DiagnosticResult.js");
            AddResource(container, "libs/B4/view/diagnostic/DiagnosticResultGrid.js");
            AddResource(container, "libs/B4/view/diagnostic/EditWindow.js");
            AddResource(container, "libs/B4/view/diagnostic/Grid.js");


        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Diagnostic.dll/Bars.Gkh.Diagnostic.{0}", path.Replace("/", ".")));
        }
    }
}
