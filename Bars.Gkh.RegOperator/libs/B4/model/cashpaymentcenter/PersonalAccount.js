Ext.define('B4.model.cashpaymentcenter.PersonalAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenterPersAcc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CashPaymentCenter', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PersonalAccount', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Name' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'ContragentState' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});