Ext.define('B4.aspects.permission.dict.WorkTo', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.worktoperm',

    permissions: [
        { name: 'GkhDi.Dict.WorkTo.Create', applyTo: 'b4addbutton', selector: '#workToGrid' },
        { name: 'GkhDi.Dict.WorkTo.Edit', applyTo: 'b4savebutton', selector: '#workToEditWindow' },
        { name: 'GkhDi.Dict.WorkTo.Delete', applyTo: 'b4deletecolumn', selector: '#workToGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});