Ext.define('B4.store.AddressByLocality', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocalityAddressByMo',
        listAction: 'ListAddressByLocality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});