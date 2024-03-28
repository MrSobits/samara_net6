Ext.define('B4.aspects.permission.specialobjectcr.PersonalAccount', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.personalaccountspecialobjectcrperm',

    permissions: [ 
        { name: 'GkhCr.ObjectCr.Register.PersonalAccount.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrpersonalaccountgrid' },
        { name: 'GkhCr.ObjectCr.Register.PersonalAccount.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrpersonalaccounteditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.PersonalAccount.Delete', applyTo: 'b4deletecolumn', selector: 'specialobjectcrpersonalaccountgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});