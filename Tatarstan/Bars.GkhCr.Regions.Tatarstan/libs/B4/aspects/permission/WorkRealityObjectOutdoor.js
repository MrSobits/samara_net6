Ext.define('B4.aspects.permission.WorkRealityObjectOutdoor', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.workoutdoorpermission',

    permissions: [
        { name: 'GkhCr.Dict.WorkRealityObjectOutdoor.Create', applyTo: 'b4addbutton', selector: 'workrealityobjectoutdoorpanel' },
        { name: 'GkhCr.Dict.WorkRealityObjectOutdoor.Edit', applyTo: 'b4savebutton', selector: 'workrealityobjectoutdoorwindow' },
        {
            name: 'GkhCr.Dict.WorkRealityObjectOutdoor.Delete', applyTo: 'b4deletecolumn', selector: 'workrealityobjectoutdoorpanel',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
