Ext.define('B4.model.longtermprobject.LongTermObjectLoan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LongTermObjectLoan',
        listAction: 'List'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'File' },
        { name: 'Object' },
        { name: 'ObjectIssued' },
        { name: 'DateIssue' },
        { name: 'DateRepayment' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'LoanAmount' },
        { name: 'PeriodLoan' },
        { name: 'Address' }
    ]
});