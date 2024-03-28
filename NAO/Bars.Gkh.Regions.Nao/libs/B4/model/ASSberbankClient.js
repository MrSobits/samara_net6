Ext.define('B4.model.ASSberbankClient', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'ClientCode' },
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ASSberbankClient'
    },
});