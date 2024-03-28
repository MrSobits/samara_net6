Ext.define('B4.model.realityobj.housingcommunalservice.AccountCharge', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseAccountCharge'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'Tariff' },
        { name: 'Supplier' },
        { name: 'Expense' },
        { name: 'CompleteCalc' },
        { name: 'Underdelivery' },
        { name: 'Charged' },
        { name: 'Recalc' },
        { name: 'InnerBalance' },
        { name: 'Changed' },
        { name: 'Payment' },
        { name: 'ChargedPayment' },
        { name: 'OuterBalance' },

        { name: 'RealityObject', defaultValue: null },
        { name: 'Account', defaultValue: null },
        { name: 'Description' },
        { name: 'DateCharging' },
        { name: 'Date' }
    ]
});