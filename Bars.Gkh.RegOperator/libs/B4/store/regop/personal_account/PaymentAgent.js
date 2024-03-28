Ext.define('B4.store.regop.personal_account.PaymentAgent', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PaymentAgent'],
    autoLoad: false,
    model: 'B4.model.PaymentAgent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentAgent',
        listAction: 'ListForExport'
    }
});