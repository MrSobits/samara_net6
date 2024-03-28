Ext.define('B4.store.account.Bank', {
    extend: 'B4.base.Store',
    requires: ['B4.model.account.Payment'],
    autoLoad: false,
    model: 'B4.model.account.Payment',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LongTermPrObject',
        listAction: 'ListAccounts'
    }
});

