Ext.define('B4.aspects.permission.ObjectOutdoorCr', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.objectoutdoorcrperm',

    permissions: [
        { name: 'GkhCr.ObjectOutdoorCr.Create', applyTo: 'b4addbutton', selector: 'objectoutdoorcrgrid' },
        {
            name: 'GkhCr.ObjectOutdoorCr.Edit', applyTo: 'b4editcolumn', selector: 'objectoutdoorcrgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectOutdoorCr.Delete', applyTo: 'b4deletecolumn', selector: 'objectoutdoorcrgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});