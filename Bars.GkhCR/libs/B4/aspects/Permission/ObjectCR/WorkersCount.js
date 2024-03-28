Ext.define('B4.aspects.permission.objectcr.WorkersCount', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.workerscountobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount.Edit', applyTo: 'b4savebutton', selector: 'objectcrworkersgrid' }
    ]
});