Ext.define('B4.model.CostLimit', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimit'
    },
    fields: [
        { name: 'Id' },
        { name: 'Work' },
        { name: 'Cost' },
        { name: 'CostForCapGroup' },
        { name: 'UnitCostForCapGroup' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'FloorStart' },
        { name: 'CapitalGroup' },
        { name: 'FloorEnd' },
        { name: 'Municipality' },
        { name: 'Year' },
        { name: 'Rate' }
    ]
});