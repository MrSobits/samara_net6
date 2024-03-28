Ext.define('B4.model.payment.Item', {
    extend: 'B4.base.Model',
    requires: ['B4.enums.TypePayment'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Payment', defaultValue: null },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ManagingOrganizationName' },
        { name: 'TypePayment', defaultValue: 10 },
        { name: 'ChargeDate', defaultValue: null },
        { name: 'IncomeBalance', defaultValue: null },
        { name: 'OutgoingBalance', defaultValue: null },
        { name: 'ChargePopulation', defaultValue: null },
        { name: 'PaidPopulation', defaultValue: null },
        { name: 'Recalculation', defaultValue: null },
        { name: 'TotalArea', defaultValue: null }
    ]
});