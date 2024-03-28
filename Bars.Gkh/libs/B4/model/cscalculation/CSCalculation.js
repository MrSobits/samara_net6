Ext.define('B4.model.cscalculation.CSCalculation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CSCalculation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name'},
        { name: 'Result' },
        { name: 'CSFormula' },
        { name: 'Description' },
        { name: 'RealityObject' },
        { name: 'CalcDate' },
        { name: 'Room' },
        { name: 'Municipality' },
        { name: 'ObjectEditDate' },
        { name: 'CalculatedVariables'}        
    ]
});