Ext.define('B4.aspects.permission.outdoor.Element', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.outdoorelementperm',

    permissions: [
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Element.Create',
            applyTo: 'b4addbutton',
            selector: 'outdoorelementgrid'
        },
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Element.Edit',
            applyTo: 'b4savebutton',
            selector: 'outdoorelementeditwindow'
        },
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Element.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'outdoorelementgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});