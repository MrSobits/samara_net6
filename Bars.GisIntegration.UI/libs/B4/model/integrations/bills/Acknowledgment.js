Ext.define('B4.model.integrations.bills.Acknowledgment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Bills',
        listAction: 'GetAcknowledgments'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Period' },
        { name: 'PaymentDocumentNumber' },
        { name: 'OrderNum' },
        { name: 'Address' }
    ]
});