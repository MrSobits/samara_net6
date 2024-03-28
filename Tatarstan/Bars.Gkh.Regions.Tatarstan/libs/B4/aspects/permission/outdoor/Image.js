Ext.define('B4.aspects.permission.outdoor.Image', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.outdoorimageperm',

    permissions: [
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Image.Create',
            applyTo: 'b4addbutton',
            selector: 'outdoorimagegrid'
        },
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Image.Edit',
            applyTo: 'b4savebutton',
            selector: 'outdoorimageeditwindow'
        },
        {
            name: 'Gkh.RealityObjectOutdoor.Register.Image.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'outdoorimagegrid',
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