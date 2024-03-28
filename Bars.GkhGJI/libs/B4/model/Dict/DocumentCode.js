Ext.define('B4.model.dict.DocumentCode', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentCode'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Type' },
        { name: 'Code' }
    ]
});