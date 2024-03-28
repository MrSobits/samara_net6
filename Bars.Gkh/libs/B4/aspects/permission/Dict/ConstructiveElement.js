Ext.define('B4.aspects.permission.dict.ConstructiveElement', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.constructiveelementdictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.ConstructiveElement.Create', applyTo: 'b4addbutton', selector: '#constructiveElementGrid' },
        { name: 'Gkh.Dictionaries.ConstructiveElement.Edit', applyTo: 'b4savebutton', selector: '#constructiveElementEditWindow' },
        { name: 'Gkh.Dictionaries.ConstructiveElement.Delete', applyTo: 'b4deletecolumn', selector: '#constructiveElementGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});