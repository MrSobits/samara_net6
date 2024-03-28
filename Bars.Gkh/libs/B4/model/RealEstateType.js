Ext.define('B4.model.RealEstateType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateType'
    },
    fields: [
        { name: 'Id', type: 'number', defaultValue: 0 },
        { name: 'Name', type: 'string' },
        { name: 'Code', type: 'string' },
        { name: 'IndCount', type: 'number' },
        { name: 'MarginalRepairCost' }
    ]

});