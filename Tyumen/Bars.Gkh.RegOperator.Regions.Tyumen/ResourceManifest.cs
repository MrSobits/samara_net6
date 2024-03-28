namespace Bars.Gkh.RegOperator.Regions.Tyumen
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/RealityObject.js");
            AddResource(container, "libs/B4/controller/RequestState.js");
            AddResource(container, "libs/B4/controller/RequestStatePerson.js");
            AddResource(container, "libs/B4/model/RequestState.js");
            AddResource(container, "libs/B4/model/RequestStatePerson.js");
            AddResource(container, "libs/B4/store/RequestState.js");
            AddResource(container, "libs/B4/store/RequestStatePerson.js");
            AddResource(container, "libs/B4/view/SendAccessRequestWindow.js");
            AddResource(container, "libs/B4/view/realityobj/Grid.js");
            AddResource(container, "libs/B4/view/requeststateperson/Grid.js");
            AddResource(container, "libs/B4/view/requeststateperson/RSGrid.js");
            AddResource(container, "libs/content/css/IconButton.css");
            AddResource(container, "libs/content/img/emailstatus.png");

			AdditionalInit(container);
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.RegOperator.Regions.Tyumen.dll/Bars.Gkh.RegOperator.Regions.Tyumen.{0}", path.Replace("/", ".")));
        }
    }
}
