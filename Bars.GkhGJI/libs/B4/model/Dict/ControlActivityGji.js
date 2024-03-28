Ext.define('B4.model.dict.ControlActivityGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlActivity'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ERKNMGuid' },
        { name: 'Code' }
    ]
});