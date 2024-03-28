Ext.define('B4.aspects.permission.repairobject.RepairWork', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.repairworkperm',

    permissions: [
        { name: 'GkhRepair.RepairObject.RepairWork.Create', applyTo: 'b4addbutton', selector: 'repairWorkGrid' },
        { name: 'GkhRepair.RepairObject.RepairWork.Edit', applyTo: 'b4savebutton', selector: 'repairworkEditWindow' },
        { name: 'GkhRepair.RepairObject.RepairWork.Delete', applyTo: 'b4deletecolumn', selector: 'repairWorkGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhRepair.RepairObject.RepairWork.Field.Work_Edit', applyTo: '#sfWork', selector: 'repairworkEditWindow' },
        { name: 'GkhRepair.RepairObject.RepairWork.Field.Volume_Edit', applyTo: '#dcfVolume', selector: 'repairworkEditWindow' },
        { name: 'GkhRepair.RepairObject.RepairWork.Field.Sum_Edit', applyTo: '#dcfSum', selector: 'repairworkEditWindow' },
        { name: 'GkhRepair.RepairObject.RepairWork.Field.Builder_Edit', applyTo: '#sfBuilder', selector: 'repairworkEditWindow' }
    ]
});