namespace Bars.Gkh.Regions.Tyumen
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/ObjectCr.js");
            AddResource(container, "libs/B4/controller/import/RealityObjectExaminationImport.js");
            AddResource(container, "libs/B4/controller/suggestion/CitizenSuggestion.js");
            AddResource(container, "libs/B4/form/FiasSelectAddress.js");
            AddResource(container, "libs/B4/form/FiasSelectAddressWindow.js");
            AddResource(container, "libs/B4/model/suggestion/CitizenSuggestion.js");
            AddResource(container, "libs/B4/model/suggestion/Comment.js");
            AddResource(container, "libs/B4/model/suggestion/Rubric.js");
            AddResource(container, "libs/B4/store/realityobj/ByManOrg.js");
            AddResource(container, "libs/B4/store/suggestion/Comment.js");
            AddResource(container, "libs/B4/ux/grid/toolbar/Paging.js");
            AddResource(container, "libs/B4/view/administration/executionaction/actionwithparams/FiasAutoUpdaterAction.js");
            AddResource(container, "libs/B4/view/administration/executionaction/actionwithparams/FiasAutoUpdaterTaskAction.js");
            AddResource(container, "libs/B4/view/import/realityobj/RoExaminationImportPanel.js");
            AddResource(container, "libs/B4/view/objectcr/ImportWindow.js");
            AddResource(container, "libs/B4/view/realityobj/Grid.js");
            AddResource(container, "libs/B4/view/realityobj/structelement/Grid.js");
            AddResource(container, "libs/B4/view/suggestion/TransitionGrid.js");
            AddResource(container, "libs/B4/view/suggestion/citizensuggestion/CommentEditWindow.js");
            AddResource(container, "libs/B4/view/suggestion/citizensuggestion/CommentGrid.js");
            AddResource(container, "libs/B4/view/suggestion/citizensuggestion/EditWindow.js");
            AddResource(container, "libs/B4/view/suggestion/citizensuggestion/Grid.js");

			AdditionalInit(container);

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Tyumen.dll/Bars.Gkh.Regions.Tyumen.{0}", path.Replace("/", ".")));
        }
    }
}
