Ext.define('B4.model.objectcr.estimate.ResStat', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResourceStatement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EstimateCalculation' },
        { name: 'Number' },
        { name: 'UnitMeasure' },
        { name: 'UnitMeasureName' },
        { name: 'Name' },
        { name: 'Reason' },
        { name: 'TotalCount', defaultValue: null },
        { name: 'TotalCost', defaultValue: null },
        { name: 'OnUnitCost', defaultValue: null }
    ]
});