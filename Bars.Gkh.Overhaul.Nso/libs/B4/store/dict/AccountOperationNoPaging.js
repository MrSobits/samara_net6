Ext.define('B4.store.dict.AccountOperationNoPaging', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.AccountOperation'],
    autoLoad: false,
    model: 'B4.model.dict.AccountOperation',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccountOperation',
        listAction: 'ListNoPaging'
    }
});