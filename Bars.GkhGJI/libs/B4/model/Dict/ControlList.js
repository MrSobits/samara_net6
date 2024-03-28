Ext.define('B4.model.dict.ControlList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'KindKNDGJI', defaultValue: 0 },
        { name: 'ActualFrom' },
        { name: 'ERKNMGuid' },
        { name: 'ActualTo' }
    ]
});