Ext.define('B4.model.EconFeasibilityCalcResult', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EconFeasibilityCalcResult'
    },
    fields: [
        { name: 'Id' },
        { name: 'Adress' },
        { name: 'YearStart' },
        { name: 'YearEnd' },
        { name: 'TotatRepairSumm' },
        { name: 'SquareCost' },
        { name: 'Municipality' },
        { name: 'MoSettlement' },
        { name: 'TotalSquareCost' },
        { name: 'CostPercent' },
        { name: 'Decision' },
        { name: 'RoId' }
    ]
});