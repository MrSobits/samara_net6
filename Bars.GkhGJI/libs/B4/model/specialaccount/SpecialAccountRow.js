Ext.define('B4.model.specialaccount.SpecialAccountRow', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountRow'
    },
    fields: [
        { name: 'Id' },
        { name: 'ContragentName' },
        { name: 'ContragentInn' },
        { name: 'AmountDebtForPeriod' },
        { name:'AmountDebtCredit'},
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AmmountDebt', defaultValue: 0 },
        { name: 'Ballance', defaultValue: 0 },
        { name: 'Incoming', defaultValue: 0 },
        { name: 'SpecialAccountNum'},
        { name: 'SpecialAccountReport' },
        { name: 'AccuracyArea' },
        { name: 'StartDate' },
        { name: 'Tariff' },
        { name: 'Accured' },
        { name: 'TotalDebt' },
        { name: 'Contracts' },
        { name: 'TransferTotal' },
        { name: 'AccuredTotal' },
        { name: 'IncomingTotal' },
        { name: 'Transfer', defaultValue: 0 }, 
        { name: 'Perscent' },
    ]
});