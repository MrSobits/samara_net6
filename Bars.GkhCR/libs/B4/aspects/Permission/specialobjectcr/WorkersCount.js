Ext.define('B4.aspects.permission.specialobjectcr.WorkersCount', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.workerscountspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrworkersgrid' }
    ]
});