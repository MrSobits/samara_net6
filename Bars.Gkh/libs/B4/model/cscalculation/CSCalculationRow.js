Ext.define('B4.model.cscalculation.CSCalculationRow', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CSCalculationRow'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CSCalculation'},
        { name: 'Code'},
        { name: 'Name' },
        { name: 'DisplayValue' },
        { name: 'Value'}        
    ]
});