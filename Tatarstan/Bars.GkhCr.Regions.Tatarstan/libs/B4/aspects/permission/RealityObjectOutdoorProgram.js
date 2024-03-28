Ext.define('B4.aspects.permission.RealityObjectOutdoorProgram', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.outdoorprogramperm',

    permissions: [
        { name: 'GkhCr.OutdoorProgram.Create', applyTo: 'b4addbutton', selector: 'outdoorprogramgrid' },
        { name: 'GkhCr.OutdoorProgram.Edit', applyTo: 'b4savebutton', selector: 'outdoorprogrameditwindow' },
        {
            name: 'GkhCr.OutdoorProgram.Delete', applyTo: 'b4deletecolumn', selector: 'outdoorprogramgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});