Ext.define('B4.aspects.permission.objectcr.Qualification', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.qualificationobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.Qualification.Create', applyTo: 'b4addbutton', selector: 'qualgrid' },
        { name: 'GkhCr.ObjectCr.Register.Qualification.Edit', applyTo: 'b4savebutton', selector: 'qualificationeditwindow' },
        { name: 'GkhCr.ObjectCr.Register.Qualification.Delete', applyTo: 'b4deletecolumn', selector: 'qualgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.Qualification.Edit', applyTo: 'b4deletecolumn', selector: 'qualvoicemembergrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.Qualification.Edit', applyTo: 'b4savebutton', selector: 'qualvoicemembergrid' }

    ]
});