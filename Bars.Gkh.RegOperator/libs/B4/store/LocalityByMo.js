Ext.define('B4.store.LocalityByMo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocalityAddressByMo',
        listAction: 'ListLocalityByMo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});