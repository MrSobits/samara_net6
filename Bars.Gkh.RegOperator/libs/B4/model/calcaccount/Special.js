Ext.define('B4.model.calcaccount.Special', {
    extend: 'B4.model.CalcAccount',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialCalcAccount'
    },
    fields: [
        { name: 'IsActive' },
        { name: 'ContragentCreditOrg' }
    ]
});