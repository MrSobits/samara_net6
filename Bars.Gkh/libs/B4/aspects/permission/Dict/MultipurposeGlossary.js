Ext.define('B4.aspects.permission.dict.MultipurposeGlossary', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.multipurposedictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.Multipurpose.Create', applyTo: 'b4addbutton', selector: '#multiGlossaryMenuGrid' },
        { name: 'Gkh.Dictionaries.Multipurpose.Delete', applyTo: 'b4deletecolumn', selector: '#multiGlossaryMenuGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.Dictionaries.Multipurpose.Create', applyTo: 'b4addbutton', selector: '#multipurposeItemsGrid' },
        { name: 'Gkh.Dictionaries.Multipurpose.Edit', applyTo: 'b4savebutton', selector: '#multipurposeItemsGrid' },
        { name: 'Gkh.Dictionaries.Multipurpose.Delete', applyTo: 'b4deletecolumn', selector: '#multipurposeItemsGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});