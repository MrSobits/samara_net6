Ext.define('B4.model.integrations.payment.NotificationOfOrderExecution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentService',
        listAction: 'GetNotificationsToDelete'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OrderDate' },
        { name: 'Address' },
        { name: 'AccountNumber' },
        { name: 'OrderNum' }
    ]
});