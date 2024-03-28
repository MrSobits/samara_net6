Ext.define('B4.aspects.permission.dict.Inspector', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.inspectordictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.Inspector.Create', applyTo: 'b4addbutton', selector: '#inspectorGrid' },
        { name: 'Gkh.Dictionaries.Inspector.Edit', applyTo: 'b4savebutton', selector: '#inspectorEditWindow' },
        { name: 'Gkh.Dictionaries.Inspector.Delete', applyTo: 'b4deletecolumn', selector: '#inspectorGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});