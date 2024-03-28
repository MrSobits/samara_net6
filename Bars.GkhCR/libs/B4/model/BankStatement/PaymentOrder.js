Ext.define('B4.model.bankstatement.PaymentOrder', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePaymentOrder'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BankStatement' },
        { name: 'BankStatementName' },
        { name: 'TypeFinanceSource', defaultValue: 60 },
        { name: 'PayerContragent', defaultValue: null },
        { name: 'PayerContragentName' },
        { name: 'ReceiverContragent', defaultValue: null },
        { name: 'ReceiverContragentName' },
        { name: 'PayPurpose' },
        { name: 'BidNum' },
        { name: 'DocumentNum' },
        { name: 'BidDate', defaultValue: null },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'Sum', defaultValue: null },
        { name: 'RedirectFunds', defaultValue: null },
        { name: 'TypePaymentOrder' },
        { name: 'DocId' },
        { name: 'RepeatSend' },
        { name: 'MunicipalityName' },
        { name: 'RealObjName' }
    ]
});