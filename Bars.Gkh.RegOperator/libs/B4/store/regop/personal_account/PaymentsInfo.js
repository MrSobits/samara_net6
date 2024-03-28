Ext.define('B4.store.regop.personal_account.PaymentsInfo', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'Id' },
        { name: 'Period' },
        { name: 'PaymentDate' },
        { name: 'Reason' },
        { name: 'Amount' },
        { name: 'Source' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'ImportDate' },
        { name: 'PaymentAgentCode' },
        { name: 'PaymentAgentName' },
        { name: 'OperationDate' },
        { name: 'DistributionDate' },
        { name: 'DateReceipt' },
        { name: 'PaymentType' },
        { name: 'PaymentNumberUs' },
        { name: 'DistributionCode' },
        { name: 'DocumentId' },
        { name: 'AcceptDate' },
        { name: 'UserLogin' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'ListPaymentsInfo'
    },
    autoLoad: false
});