Ext.define('B4.model.objectcr.Estimate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Estimate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EstimateCalculation' },
        { name: 'Number' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'UnitMeasureName', defaultValue: null },
        { name: 'Name' },
        { name: 'Reason' },
        { name: 'MechanicSalary', defaultValue: null },
        { name: 'BaseSalary', defaultValue: null },
        { name: 'MechanicWork', defaultValue: null },
        { name: 'BaseWork', defaultValue: null },
        { name: 'TotalCount', defaultValue: null },
        { name: 'TotalCost', defaultValue: null },
        { name: 'OnUnitCount', defaultValue: null },
        { name: 'OnUnitCost', defaultValue: null },
        { name: 'MaterialCost', defaultValue: null },
        { name: 'MachineOperatingCost', defaultValue: null },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});