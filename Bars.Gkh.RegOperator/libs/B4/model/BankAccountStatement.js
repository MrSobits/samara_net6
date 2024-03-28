Ext.define('B4.model.BankAccountStatement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BankAccountStatement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OperationDate' },
        { name: 'DocumentDate' },
        { name: 'DistributionDate', type: 'date' },
        { name: 'DateReceipt', type: 'date' },

        { name: 'Sum' },
        { name: 'RemainSum' },
        { name: 'IsROSP', defaultValue: false },
        { name: 'DocumentNum' },
        { name: 'MoneyDirection', defaultValue: 0 },
        
        { name: 'PaymentDetails' },
        { name: 'LinkedDocuments', defaultValue: null },
        { name: 'State', defaultValue: 20 },
        { name: 'FileName' },
        { name: 'File', defaultValue: null },
        { name: 'Document', defaultValue: null },
        { name: 'UserLogin' },
        
        { name: 'Payer', defaultValue: null },
        { name: 'PayerFull' },
        { name: 'PayerName' },
        { name: 'PayerAccountNum' },
        { name: 'PayerInn' },
        { name: 'PayerKpp' },
        { name: 'PayerBik' },
        { name: 'PayerBank' },
        { name: 'PayerCorrAccount' },
        
        { name: 'Recipient', defaultValue: null },
        { name: 'RecipientName' },
        { name: 'RecipientAccountNum' },
        { name: 'RecipientInn' },
        { name: 'RecipientKpp' },
        { name: 'RecipientBik' },
        { name: 'RecipientBank' },
        { name: 'RecipientCorrAccount' },
        
        { name: 'Group', defaultValue: null },
        { name: 'DistributeState', defaultValue: 20 },
        { name: 'DistributionCode' },
        { name: 'IsDistributable', defaultValue: 10 },
        { name: 'SuspenseAccount' }
    ]
});