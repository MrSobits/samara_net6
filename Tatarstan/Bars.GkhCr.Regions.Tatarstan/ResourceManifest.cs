namespace Bars.GkhCr.Regions.Tatarstan
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/fieldrequirement/ElementOutdoor.js");
            AddResource(container, "libs/B4/aspects/fieldrequirement/WorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/aspects/permission/ElementOutdoor.js");
            AddResource(container, "libs/B4/aspects/permission/ObjectOutdoorCr.js");
            AddResource(container, "libs/B4/aspects/permission/ObjectOutdoorCrEdit.js");
            AddResource(container, "libs/B4/aspects/permission/RealityObjectOutdoorProgram.js");
            AddResource(container, "libs/B4/aspects/permission/TypeWorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/aspects/permission/TypeWorkRealityObjectOutdoorEdit.js");
            AddResource(container, "libs/B4/aspects/permission/WorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/catalogs/OutdoorProgramSelectField.js");
            AddResource(container, "libs/B4/catalogs/ProgramVersionSelectField.js");
            AddResource(container, "libs/B4/controller/dict/ElementOutdoor.js");
            AddResource(container, "libs/B4/controller/dict/RealityObjectOutdoorProgram.js");
            AddResource(container, "libs/B4/controller/dict/WorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/controller/objectoutdoorcr/Edit.js");
            AddResource(container, "libs/B4/controller/objectoutdoorcr/Navi.js");
            AddResource(container, "libs/B4/controller/objectoutdoorcr/ObjectOutdoorCr.js");
            AddResource(container, "libs/B4/controller/objectoutdoorcr/TypeWorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/model/dict/ElementOutdoor.js");
            AddResource(container, "libs/B4/model/dict/RealityObjectOutdoorProgram.js");
            AddResource(container, "libs/B4/model/dict/RealityObjectOutdoorProgramChangeJournal.js");
            AddResource(container, "libs/B4/model/dict/WorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/model/dict/WorksElementOutdoor.js");
            AddResource(container, "libs/B4/model/objectoutdoorcr/ObjectOutdoorCr.js");
            AddResource(container, "libs/B4/model/objectoutdoorcr/typeworkrealityobjectoutdoor/TypeWorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/model/objectoutdoorcr/typeworkrealityobjectoutdoor/TypeWorkRealityObjectOutdoorHistory.js");
            AddResource(container, "libs/B4/store/dict/ElementOutdoor.js");
            AddResource(container, "libs/B4/store/dict/ProgramVersionSelected.js");
            AddResource(container, "libs/B4/store/dict/RealityObjectOutdoorProgram.js");
            AddResource(container, "libs/B4/store/dict/RealityObjectOutdoorProgramChangeJournal.js");
            AddResource(container, "libs/B4/store/dict/RealityObjectOutdoorProgramSelected.js");
            AddResource(container, "libs/B4/store/dict/WorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/store/dict/WorksElementOutdoor.js");
            AddResource(container, "libs/B4/store/dict/WorksElementOutdoorSelect.js");
            AddResource(container, "libs/B4/store/dict/WorksElementOutdoorSelected.js");
            AddResource(container, "libs/B4/store/objectoutdoorcr/ObjectOutdoorCr.js");
            AddResource(container, "libs/B4/store/objectoutdoorcr/ObjectOutdoorCrNavigationMenu.js");
            AddResource(container, "libs/B4/store/objectoutdoorcr/typeworkrealityobjectoutdoor/TypeWorkRealityObjectOutdoor.js");
            AddResource(container, "libs/B4/store/objectoutdoorcr/typeworkrealityobjectoutdoor/TypeWorkRealityObjectOutdoorHistory.js");
            AddResource(container, "libs/B4/view/dict/elementoutdoor/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/elementoutdoor/Grid.js");
            AddResource(container, "libs/B4/view/dict/elementoutdoor/WorksElementOutdoorGrid.js");
            AddResource(container, "libs/B4/view/dict/realityobjectoutdoorprogram/ChangeJournalGrid.js");
            AddResource(container, "libs/B4/view/dict/realityobjectoutdoorprogram/CopyWindow.js");
            AddResource(container, "libs/B4/view/dict/realityobjectoutdoorprogram/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/realityobjectoutdoorprogram/Grid.js");
            AddResource(container, "libs/B4/view/dict/workrealityobjectoutdoor/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/workrealityobjectoutdoor/Grid.js");
            AddResource(container, "libs/B4/view/dict/workselementoutdoor/Grid.js");
            AddResource(container, "libs/B4/view/objectcr/BuildContractEditWindow.js");
            AddResource(container, "libs/B4/view/objectcr/Grid.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/AddWindow.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/DeletedObjectOutdoorCrEditWindow.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/DeletedObjectOutdoorCrGrid.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/EditPanel.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/FilterPanel.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/Grid.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/NavigationPanel.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/Panel.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/typeworkrealityobjectoutdoor/EditWindow.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/typeworkrealityobjectoutdoor/Grid.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/typeworkrealityobjectoutdoor/HistoryGrid.js");
            AddResource(container, "libs/B4/view/objectoutdoorcr/typeworkrealityobjectoutdoor/Panel.js");

			this.AdditionalInit(container);
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhCr.Regions.Tatarstan.dll/Bars.GkhCr.Regions.Tatarstan.{0}", path.Replace("/", ".")));
        }
    }
}
