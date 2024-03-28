Ext.define('B4.store.cashpaymentcenter.ManOrgRealityObjectForAdd', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'Address' },
        { name: 'ManOrg' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenter',
        listAction: 'ListRealObjForCashPaymentCenterManOrg'
    }
});