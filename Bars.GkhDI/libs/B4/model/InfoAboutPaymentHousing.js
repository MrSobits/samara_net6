Ext.define('B4.model.InfoAboutPaymentHousing', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InfoAboutPaymentHousing'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseServiceName' },
        { name: 'ProviderName' },
        { name: 'CounterValuePeriodStart', defaultValue: null },
        { name: 'CounterValuePeriodEnd', defaultValue: null },
        { name: 'GeneralAccrual', defaultValue: null },
        { name: 'Collection', defaultValue: null }
    ]
});