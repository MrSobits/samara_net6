namespace Bars.GkhDi.Regions.Tatarstan
{
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {
            this.AddResource(container, "libs/B4/controller/DocumentsRealityObj.js");
            this.AddResource(container, "libs/B4/controller/NonResidentialPlacement.js");
            this.AddResource(container, "libs/B4/controller/InfoAboutUseCommonFacilities.js");
            this.AddResource(container, "libs/B4/controller/LoadWorkController.js");
            this.AddResource(container, "libs/B4/controller/PlanReductionExpense.js");
            this.AddResource(container, "libs/B4/controller/PlanWorkServiceRepair.js");
            this.AddResource(container, "libs/B4/controller/Service.js");
            this.AddResource(container, "libs/B4/controller/dict/MeasuresReduceCosts.js");
            this.AddResource(container, "libs/B4/controller/CopyUninhabitablePremisesController.js");
            this.AddResource(container, "libs/B4/controller/CopyCommonAreasController.js");

            this.AddResource(container, "libs/B4/model/PlanWorkServiceRepair.js");
            this.AddResource(container, "libs/B4/model/dict/MeasuresReduceCosts.js");
            this.AddResource(container, "libs/B4/model/service/LoadWorkPprRepair.js");
            this.AddResource(container, "libs/B4/model/service/ProviderService.js");
            this.AddResource(container, "libs/B4/model/service/Repair.js");
            this.AddResource(container, "libs/B4/model/service/WorkRepairDetail.js");
            this.AddResource(container, "libs/B4/model/service/WorkRepairList.js");

            this.AddResource(container, "libs/B4/store/MeasuresReduceCostsSelect.js");
            this.AddResource(container, "libs/B4/store/MeasuresReduceCostsSelected.js");
            this.AddResource(container, "libs/B4/store/dict/MeasuresReduceCosts.js");
            this.AddResource(container, "libs/B4/store/service/WorkRepairTechService.js");

            this.AddResource(container, "libs/B4/view/dict/measuresreducecosts/Grid.js");
            this.AddResource(container, "libs/B4/view/infoaboutusecommonfacilities/EditWindow.js");
            this.AddResource(container, "libs/B4/view/planreductionexpense/WorksGrid.js");
            this.AddResource(container, "libs/B4/view/planworkservicerepair/EditWindow.js");
            this.AddResource(container, "libs/B4/view/planworkservicerepair/RepairServicesGrid.js");
            this.AddResource(container, "libs/B4/view/planworkservicerepair/WorksEditWindow.js");
            this.AddResource(container, "libs/B4/view/planworkservicerepair/WorksGrid.js");
            this.AddResource(container, "libs/B4/view/realityobjectpassport/ViewPanel.js");
            this.AddResource(container, "libs/B4/view/service/Grid.js");
            this.AddResource(container, "libs/B4/view/service/LoadWorkEditPanel.js");
            this.AddResource(container, "libs/B4/view/service/repair/EditWindow.js");
            this.AddResource(container, "libs/B4/view/service/repair/ProviderServiceEditWindow.js");
            this.AddResource(container, "libs/B4/view/service/repair/ProviderServiceGrid.js");
            this.AddResource(container, "libs/B4/view/service/repair/WorkRepairDetailGrid.js");
            this.AddResource(container, "libs/B4/view/service/repair/WorkRepairListGrid.js");
            this.AddResource(container, "libs/B4/view/service/repair/WorkRepairTechServGrid.js");
            this.AddResource(container, "libs/B4/view/service/CopyUninhabitablePremisesEditPanel.js");
            this.AddResource(container, "libs/B4/view/service/CopyCommonAreasEditPanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhDi.Regions.Tatarstan.dll/Bars.GkhDi.Regions.Tatarstan.{0}", path.Replace("/", ".")));
        }
    }
}
