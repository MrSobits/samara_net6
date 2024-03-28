Ext.define('B4.model.InfoAboutPaymentCommunal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InfoAboutPaymentCommunal'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseServiceName' },
        { name: 'ProviderName' },
        { name: 'CounterValuePeriodStart', defaultValue: null },
        { name: 'CounterValuePeriodEnd', defaultValue: null },
        { name: 'TotalConsumption', defaultValue: null },
        { name: 'Accrual', defaultValue: null },
        { name: 'Payed', defaultValue: null },
        { name: 'Debt', defaultValue: null },
        { name: 'AccrualByProvider', defaultValue: null },
        { name: 'PayedToProvider', defaultValue: null },
        { name: 'DebtToProvider', defaultValue: null },
        { name: 'ReceivedPenaltySum', defaultValue: null }
    ]
});