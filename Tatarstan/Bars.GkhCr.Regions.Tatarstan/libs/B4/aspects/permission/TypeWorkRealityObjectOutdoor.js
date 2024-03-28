Ext.define('B4.aspects.permission.TypeWorkRealityObjectOutdoor', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.typeworkrealityobjectoutdoorperm',

    permissions: [
        { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Create', applyTo: '[name=AddButton]', selector: 'typeworkrealityobjectoutdoorgrid' },
        { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Edit', applyTo: 'b4savebutton', selector: 'typeworkrealityobjectoutdooreditwindow' },
        {
            name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'typeworkrealityobjectoutdoorgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});