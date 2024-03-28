Ext.define('B4.aspects.permission.dict.Period', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.perioddictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.Period.Create', applyTo: 'b4addbutton', selector: '#periodGrid' },
        { name: 'Gkh.Dictionaries.Period.Edit', applyTo: 'b4savebutton', selector: '#periodEditWindow' },
        { name: 'Gkh.Dictionaries.Period.Delete', applyTo: 'b4deletecolumn', selector: '#periodGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});