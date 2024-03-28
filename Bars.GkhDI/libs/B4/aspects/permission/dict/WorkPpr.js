Ext.define('B4.aspects.permission.dict.WorkPpr', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.workpprperm',

    permissions: [
        { name: 'GkhDi.Dict.Ppr.Create', applyTo: 'b4addbutton', selector: '#workPprGrid' },
        { name: 'GkhDi.Dict.Ppr.Edit', applyTo: 'b4savebutton', selector: '#workPprEditWindow' },
        { name: 'GkhDi.Dict.Ppr.Delete', applyTo: 'b4deletecolumn', selector: '#workPprGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});