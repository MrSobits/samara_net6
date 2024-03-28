Ext.define('B4.aspects.permission.dict.Institution', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.institutiondictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.Institutions.Create', applyTo: 'b4addbutton', selector: '#institutionsGrid' },
        { name: 'Gkh.Dictionaries.Institutions.Edit', applyTo: 'b4savebutton', selector: '#institutionsEditWindow' },
        { name: 'Gkh.Dictionaries.Institutions.Delete', applyTo: 'b4deletecolumn', selector: '#institutionsGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});