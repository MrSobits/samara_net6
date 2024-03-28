Ext.define('B4.model.realityobj.housingcommunalservice.AccountMeteringDeviceValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MeterReading'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Account', defaultValue: null },
        { name: 'Service' },
        { name: 'MeterSerial' },
        { name: 'MeterType' },
        { name: 'CurrentReadingDate' },
        { name: 'PrevReadingDate' },
        { name: 'CurrentReading' },
        { name: 'PrevReading' },
        { name: 'Expense' },
        { name: 'PlannedExpense' }
    ]
});