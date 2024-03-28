Ext.define('B4.aspects.permission.ElementOutdoor', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.elementoutdoorpermission',

    permissions: [
        { name: 'GkhCr.Dict.ElementOutdoor.Create', applyTo: 'b4addbutton', selector: 'elementoutdoorpanel' },
        { name: 'GkhCr.Dict.ElementOutdoor.Edit', applyTo: 'b4savebutton', selector: 'elementoutdoorwindow' },
        {
            name: 'GkhCr.Dict.ElementOutdoor.Delete', applyTo: 'b4deletecolumn', selector: 'elementoutdoorpanel',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});