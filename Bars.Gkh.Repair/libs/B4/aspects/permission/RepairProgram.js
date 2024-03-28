Ext.define('B4.aspects.permission.RepairProgram', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.repairprogramperm',

    permissions: [
        { name: 'GkhRepair.RepairProgram.Create', applyTo: 'b4addbutton', selector: 'repairProgramGrid' },
        { name: 'GkhRepair.RepairProgram.Edit', applyTo: 'b4savebutton', selector: 'repairProgramEditWindow' },
        { name: 'GkhRepair.RepairProgram.Edit', applyTo: 'b4addbutton', selector: 'repprogrammunicipalitygrid' },
        { name: 'GkhRepair.RepairProgram.Delete', applyTo: 'b4deletecolumn', selector: 'repairProgramGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    
    ]
});