Ext.define('B4.aspects.permission.repairobject.RepairObject', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.repairobjectstateperm',

    permissions: [
        { name: 'GkhRepair.RepairObject.Edit', applyTo: 'b4addbutton', selector: '#repairWorkGrid' },
        { name: 'GkhRepair.RepairObject.Edit', applyTo: 'b4editcolumn', selector: '#repairWorkGrid' }
    ]
});