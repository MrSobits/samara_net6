Ext.define('B4.store.builder.LoanRepayment', {
    extend: 'B4.base.Store',
    requires: ['B4.model.builder.LoanRepayment'],
    autoLoad: false,
    storeId: 'builderLoanRepaymentStore',
    model: 'B4.model.builder.LoanRepayment'
});