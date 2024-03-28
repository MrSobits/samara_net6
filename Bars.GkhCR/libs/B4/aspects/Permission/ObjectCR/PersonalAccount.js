Ext.define('B4.aspects.permission.objectcr.PersonalAccount', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.personalaccountobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.PersonalAccount.Create', applyTo: 'b4addbutton', selector: 'personalaccountgrid' },
        { name: 'GkhCr.ObjectCr.Register.PersonalAccount.Edit', applyTo: 'b4savebutton', selector: 'objectcrpersonalaccounteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.PersonalAccount.Delete', applyTo: 'b4deletecolumn', selector: 'personalaccountgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }

    ]
});