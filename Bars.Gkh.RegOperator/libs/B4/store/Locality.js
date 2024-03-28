Ext.define('B4.store.Locality', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocalityAddressByMo',
        listAction: 'ListLocality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Settlement' },
        { name: 'Municipality' }
    ]
});