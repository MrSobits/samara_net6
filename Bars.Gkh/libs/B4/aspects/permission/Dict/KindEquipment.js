Ext.define('B4.aspects.permission.dict.KindEquipment', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.kindequipmentdictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.KindEquipment.Create', applyTo: 'b4addbutton', selector: '#kindEquipmentGrid' },
        { name: 'Gkh.Dictionaries.KindEquipment.Edit', applyTo: 'b4savebutton', selector: '#kindEquipmentEditWindow' },
        { name: 'Gkh.Dictionaries.KindEquipment.Delete', applyTo: 'b4deletecolumn', selector: '#kindEquipmentGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});