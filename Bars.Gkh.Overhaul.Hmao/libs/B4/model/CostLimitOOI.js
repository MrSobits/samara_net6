Ext.define('B4.model.CostLimitOOI', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimitOOI'
    },
    fields: [
        { name: 'Id' },
        { name: 'CommonEstateObject' },
        { name: 'Cost' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'FloorStart' },
        { name: 'FloorEnd' },
        { name: 'Municipality' },
    ]
});