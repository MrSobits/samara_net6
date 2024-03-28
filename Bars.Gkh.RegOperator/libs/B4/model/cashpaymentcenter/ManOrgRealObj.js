Ext.define('B4.model.cashpaymentcenter.ManOrgRealObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenterManOrgRo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CashPaymentCenterManOrg' },
        { name: 'RealityObject' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});