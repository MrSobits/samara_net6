Ext.define('B4.model.dict.PaymentSizeCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentSizeCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeIndicator', defaultValue: 10 },
        { name: 'PaymentSize' },
        { name: 'DateStartPeriod' },
        { name: 'DateEndPeriod' },
        { name: 'MunicipalityCount' },
        { name: 'MunicipalityNames' }
    ]
});