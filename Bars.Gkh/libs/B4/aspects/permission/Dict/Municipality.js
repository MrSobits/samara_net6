Ext.define('B4.aspects.permission.dict.Municipality', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.municipalitydictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.Municipality.Create', applyTo: 'b4addbutton', selector: '#municipalityGrid' },
        { name: 'Gkh.Dictionaries.Municipality.Edit', applyTo: 'b4savebutton', selector: '#municipalityEditWindow' },
        { name: 'Gkh.Dictionaries.Municipality.Delete', applyTo: 'b4deletecolumn', selector: '#municipalityGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.Dictionaries.Municipality.Create', applyTo: 'b4addbutton', selector: '#municipalitySourceFinancingGrid' },
        { name: 'Gkh.Dictionaries.Municipality.Edit', applyTo: 'b4savebutton', selector: '#municipalitySourceFinancingEditWindow' },
        { name: 'Gkh.Dictionaries.Municipality.Delete', applyTo: 'b4deletecolumn', selector: '#municipalitySourceFinancingGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});