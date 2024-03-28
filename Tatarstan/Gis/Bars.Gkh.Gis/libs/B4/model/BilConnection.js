Ext.define('B4.model.BilConnection', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilConnection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Connection', type: 'string' },
        { name: 'ConnectionType' },
        { name: 'AppUrl' }
    ]
});