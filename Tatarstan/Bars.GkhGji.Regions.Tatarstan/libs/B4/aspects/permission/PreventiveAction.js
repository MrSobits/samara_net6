Ext.define('B4.aspects.permission.PreventiveAction', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.preventiveactionpermissions',

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.PreventiveActions.Edit',
            applyTo: 'b4savebutton',
            selector: '#preventiveActionEditPanel'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActions.Create',
            applyTo: 'b4addbutton',
            selector: 'preventiveactiongrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActions.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'preventiveactiongrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActions.ShowClosedActions',
            applyTo: '[name=ShowClosed]',
            selector: 'preventiveactiongrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
    ]
});