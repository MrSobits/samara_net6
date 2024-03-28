Ext.define('B4.model.realityobj.housingcommunalservice.OverallBalance', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseOverallBalance'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Service' },
        { name: 'InnerBalance' },
        { name: 'MonthCharge' },
        { name: 'Payment' },
        { name: 'Paid' },
        { name: 'OuterBalance' },
        { name: 'CorrectionCoef' },
        { name: 'HouseExpense' },
        { name: 'AccountsExpense' },
        { name: 'DateCharging' },
        { name: 'Date' }
    ]
});