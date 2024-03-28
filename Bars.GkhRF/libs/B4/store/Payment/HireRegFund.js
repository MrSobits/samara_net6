Ext.define('B4.store.payment.HireRegFund', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'hireRegFundStore',
    typePayment: 'HireRegFund',
    model: 'B4.model.payment.Item'
});