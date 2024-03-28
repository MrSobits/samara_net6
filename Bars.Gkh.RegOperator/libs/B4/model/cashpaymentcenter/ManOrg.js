Ext.define('B4.model.cashpaymentcenter.ManOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenterManOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CashPaymentCenter', defaultValue: null },
        { name: 'ManOrg', defaultValue: null },
        { name: 'Municipality' },
        { name: 'NumberContract' },
        { name: 'DateContract' },
        { name: 'ManOrgName' },
        { name: 'Inn' },
        { name: 'HousesCount' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});