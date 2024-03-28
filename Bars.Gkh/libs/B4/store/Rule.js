Ext.define('B4.store.Rule', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'StateTransferRule',
        listAction: 'ListRules'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'TypeId' },
        { name: 'Description' }
    ]
});