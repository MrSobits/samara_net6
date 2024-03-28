Ext.define('B4.aspects.permission.repairobject.ProgressExecutionWork', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.progressexecutionrepairworkperm',

    permissions: [
        { name: 'GkhRepair.RepairObject.ProgressExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'progressexecutionworkeditwin' },
        
        {
            name: 'GkhRepair.RepairObject.ProgressExecutionWork.Field.VolumeOfCompletion_Edit',
            applyTo: '#dcfVolumeOfCompletion',
            selector: 'progressexecutionworkeditwin'
        },
        {
            name: 'GkhRepair.RepairObject.ProgressExecutionWork.Field.CostSum_Edit',
            applyTo: '#dcfCostSum',
            selector: 'progressexecutionworkeditwin'
        },
        {
            name: 'GkhRepair.RepairObject.ProgressExecutionWork.Field.PercentOfCompletion_Edit',
            applyTo: '#dcfPercentOfCompletion',
            selector: 'progressexecutionworkeditwin'
        }
    ]
});