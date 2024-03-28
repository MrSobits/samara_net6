Ext.define('B4.model.CostLimitTypeWorkCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimitTypeWorkCr'
    },
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'Cost' },
        { name: 'Volume' },
        { name: 'Year' },
        { name: 'UnitMeasure' },
        { name: 'UnitCost' }
    ]
});