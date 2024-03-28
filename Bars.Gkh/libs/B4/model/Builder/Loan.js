Ext.define('B4.model.builder.Loan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderLoan'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Lender', defaultValue: null },
        { name: 'Amount' },
        { name: 'DateIssue' },
        { name: 'DatePlanReturn' },
        { name: 'Description' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' }
    ]
});