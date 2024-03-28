Ext.define('B4.aspects.permission.Payment', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.paymentrfperm',

    permissions: [
        { name: 'GkhRf.Payment.Create', applyTo: 'b4addbutton', selector: '#paymentGrid' },
        { name: 'GkhRf.Payment.Edit', applyTo: 'b4savebutton', selector: '#paymentNavigationPanel' },
        { name: 'GkhRf.Payment.Delete', applyTo: 'b4deletecolumn', selector: '#paymentGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});