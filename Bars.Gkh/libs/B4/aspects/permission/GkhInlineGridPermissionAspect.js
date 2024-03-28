Ext.define('B4.aspects.permission.GkhInlineGridPermissionAspect', {
    extend: 'B4.aspects.permission.GkhGridPermissionAspect',
    alias: 'widget.inlinegridpermissionaspect',

    permissions: [
        {
            name: 'Create',
            applyTo: 'b4addbutton',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'Delete',
            applyTo: 'b4deletecolumn',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Edit',
            applyTo: 'b4savebutton',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            }
        }
    ]
});