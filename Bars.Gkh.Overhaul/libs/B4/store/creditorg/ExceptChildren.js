Ext.define('B4.store.creditorg.ExceptChildren', {
    extend: 'B4.base.Store',
    requires: ['B4.model.CreditOrg'],
    autoLoad: false,
    model: 'B4.model.CreditOrg',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CreditOrg',
        listAction: 'ListExceptChildren'
    }
});