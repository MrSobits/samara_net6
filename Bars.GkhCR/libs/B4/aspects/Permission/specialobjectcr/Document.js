Ext.define('B4.aspects.permission.specialobjectcr.Document', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.documentspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.Document.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrdocumentgrid' },
        { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.Document.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrdocumentwin' },
        {
            name: 'GkhCr.ObjectCr.Register.MonitoringSmr.Document.Delete', applyTo: 'b4deletecolumn', selector: 'specialobjectcrdocumentgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});