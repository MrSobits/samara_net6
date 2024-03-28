Ext.define('B4.model.longtermprobject.Loan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LongTermObjectLoan'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Object', defaultValue: null },
        { name: 'ObjectIssued', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'LoanAmount' },
        { name: 'DateIssue' },
        { name: 'DateRepayment' },
        { name: 'PeriodLoan' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'ObjectAddress' }
    ]
});