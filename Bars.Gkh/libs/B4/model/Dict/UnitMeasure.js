Ext.define('B4.model.dict.UnitMeasure', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UnitMeasure'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'Description' },
        { name: 'OkeiCode' }
    ]
});