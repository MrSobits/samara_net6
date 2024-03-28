Ext.define('B4.model.cashpaymentcenter.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenterMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CashPaymentCenter', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});