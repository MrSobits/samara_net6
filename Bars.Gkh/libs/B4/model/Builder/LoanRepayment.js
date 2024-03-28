Ext.define('B4.model.builder.LoanRepayment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderLoanRepayment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BuilderLoan', defaultValue: null },
        { name: 'Description' },
        { name: 'Name' },
        { name: 'RepaymentAmount' },
        { name: 'RepaymentDate' }
    ]
});