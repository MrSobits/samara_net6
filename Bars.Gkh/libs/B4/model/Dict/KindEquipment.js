Ext.define('B4.model.dict.KindEquipment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KindEquipment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'UnitMeasure', defaultValue: null }
    ]
});