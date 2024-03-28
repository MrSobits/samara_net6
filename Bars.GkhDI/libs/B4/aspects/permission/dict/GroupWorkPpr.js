Ext.define('B4.aspects.permission.dict.GroupWorkPpr', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.groupworkpprperm',

    permissions: [
        { name: 'GkhDi.Dict.GroupPpr.Create', applyTo: 'b4addbutton', selector: '#groupWorkPprGrid' },
        { name: 'GkhDi.Dict.GroupPpr.Edit', applyTo: 'b4savebutton', selector: '#groupWorkPprEditWindow' },
        { name: 'GkhDi.Dict.GroupPpr.Delete', applyTo: 'b4deletecolumn', selector: '#groupWorkPprGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});