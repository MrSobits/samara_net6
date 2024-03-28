namespace Bars.Gkh.Repair
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/aspects/permission/RepairProgram.js");
            AddResource(container, "libs/B4/aspects/permission/repairobject/ProgressExecutionWork.js");
            AddResource(container, "libs/B4/aspects/permission/repairobject/RepairObject.js");
            AddResource(container, "libs/B4/aspects/permission/repairobject/RepairWork.js");
            AddResource(container, "libs/B4/controller/RepairControlDate.js");
            AddResource(container, "libs/B4/controller/RepairObject.js");
            AddResource(container, "libs/B4/controller/RepairObjectMassStateChange.js");
            AddResource(container, "libs/B4/controller/dict/RepairProgram.js");
            AddResource(container, "libs/B4/controller/repairobject/Edit.js");
            AddResource(container, "libs/B4/controller/repairobject/Navigation.js");
            AddResource(container, "libs/B4/controller/repairobject/PerformedRepairWorkAct.js");
            AddResource(container, "libs/B4/controller/repairobject/ProgressExecutionWork.js");
            AddResource(container, "libs/B4/controller/repairobject/RepairWork.js");
            AddResource(container, "libs/B4/controller/repairobject/ScheduleExecutionWork.js");
            AddResource(container, "libs/B4/controller/report/MkdRepairInfoReport.js");
            AddResource(container, "libs/B4/model/RepairControlDate.js");
            AddResource(container, "libs/B4/model/RepairObject.js");
            AddResource(container, "libs/B4/model/dict/RepairProgram.js");
            AddResource(container, "libs/B4/model/dict/RepairProgramMunicipality.js");
            AddResource(container, "libs/B4/model/repairobject/PerformedRepairWorkAct.js");
            AddResource(container, "libs/B4/model/repairobject/RepairWork.js");
            AddResource(container, "libs/B4/store/RepairControlDate.js");
            AddResource(container, "libs/B4/store/RepairObject.js");
            AddResource(container, "libs/B4/store/dict/RepairProgram.js");
            AddResource(container, "libs/B4/store/dict/RepairProgramForSelect.js");
            AddResource(container, "libs/B4/store/dict/RepairProgramForSelected.js");
            AddResource(container, "libs/B4/store/dict/RepairProgramMunicipality.js");
            AddResource(container, "libs/B4/store/dict/RepairProgramObj.js");
            AddResource(container, "libs/B4/store/dict/RepairProgramRo.js");
            AddResource(container, "libs/B4/store/repairobject/NavigationMenu.js");
            AddResource(container, "libs/B4/store/repairobject/PerformedRepairWorkAct.js");
            AddResource(container, "libs/B4/store/repairobject/ProgressExecutionWork.js");
            AddResource(container, "libs/B4/store/repairobject/RepairWork.js");
            AddResource(container, "libs/B4/store/repairobject/ScheduleExecutionWork.js");
            AddResource(container, "libs/B4/view/dict/repairprogram/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/repairprogram/Grid.js");
            AddResource(container, "libs/B4/view/dict/repairprogram/MunicipalityGrid.js");
            AddResource(container, "libs/B4/view/repaircontroldate/EditWindow.js");
            AddResource(container, "libs/B4/view/repaircontroldate/Grid.js");
            AddResource(container, "libs/B4/view/repaircontroldate/WorkEditWindow.js");
            AddResource(container, "libs/B4/view/repaircontroldate/WorkGrid.js");
            AddResource(container, "libs/B4/view/repairobject/AddWindow.js");
            AddResource(container, "libs/B4/view/repairobject/EditPanel.js");
            AddResource(container, "libs/B4/view/repairobject/FilterPanel.js");
            AddResource(container, "libs/B4/view/repairobject/Grid.js");
            AddResource(container, "libs/B4/view/repairobject/NavigationPanel.js");
            AddResource(container, "libs/B4/view/repairobject/Panel.js");
            AddResource(container, "libs/B4/view/repairobject/performedrepairworkact/EditWindow.js");
            AddResource(container, "libs/B4/view/repairobject/performedrepairworkact/Grid.js");
            AddResource(container, "libs/B4/view/repairobject/progressexecution/EditWindow.js");
            AddResource(container, "libs/B4/view/repairobject/progressexecution/Grid.js");
            AddResource(container, "libs/B4/view/repairobject/repairwork/EditWindow.js");
            AddResource(container, "libs/B4/view/repairobject/repairwork/Grid.js");
            AddResource(container, "libs/B4/view/repairobject/scheduleexecutionwork/AddDateGrid.js");
            AddResource(container, "libs/B4/view/repairobject/scheduleexecutionwork/AddDateWindow.js");
            AddResource(container, "libs/B4/view/repairobject/scheduleexecutionwork/Grid.js");
            AddResource(container, "libs/B4/view/repairobjectmassstatechange/Grid.js");
            AddResource(container, "libs/B4/view/repairobjectmassstatechange/MainPanel.js");
            AddResource(container, "libs/B4/view/report/MkdRepairInfoReport/Panel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Repair.dll/Bars.Gkh.Repair.{0}", path.Replace("/", ".")));
        }
    }
}
