Ext.define('B4.aspects.permission.typeworkcr.Document', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.documenttypeworkcrperm',

    permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.Document.Create', applyTo: 'b4addbutton', selector: '#documentWorkCrGrid' },
                { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.Document.Edit', applyTo: 'b4savebutton', selector: '#documentWorkCrEditWindow' },
        {
            name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.Document.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#documentWorkCrGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});