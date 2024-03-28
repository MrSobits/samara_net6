Ext.define('B4.aspects.permission.typeworkcr.WorkersCount', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.workerscounttypeworkcrperm',
    permissions: [
               { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount.Edit', applyTo: 'b4savebutton', selector: 'objectcrworkersgrid' }
    ]
});