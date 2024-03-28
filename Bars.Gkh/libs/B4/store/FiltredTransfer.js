Ext.define('B4.store.FiltredTransfer', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'StateTransfer',
        listAction: 'ListFiltredTransfers'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'TypeName' },
        { name: 'TypeId' },
        { name: 'Role' }
    ]
});