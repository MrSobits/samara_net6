Ext.define('B4.store.cashpaymentcenter.RealObjForAdd', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'PersonalAccountNum' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenter',
        listAction: 'ListObjForCashPaymentCenter'
    }
});