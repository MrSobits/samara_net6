Ext.define('B4.store.builder.Loan', {
    extend: 'B4.base.Store',
    requires: ['B4.model.builder.Loan'],
    autoLoad: false,
    storeId: 'builderLoanStore',
    model: 'B4.model.builder.Loan'
});