Ext.define('B4.aspects.permission.realityobj.RealityObjectOutdoor', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.realityobjectoutdoorpermissionsaspect',

    permissions: [
        { name: 'Gkh.RealityObjectOutdoor.Create', applyTo: 'b4addbutton', selector: 'realityobjectoutdoorgrid' },
        {
            name: 'Gkh.RealityObjectOutdoor.Edit', applyTo: 'b4editcolumn', selector: 'realityobjectoutdoorgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.RealityObjectOutdoor.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjectoutdoorgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});