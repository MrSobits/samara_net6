Ext.define('B4.model.objectcr.performedworkact.Record', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkActRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PerformedWorkAct' },
        { name: 'Number' },
        { name: 'Reason' },
        { name: 'UnitMeasure' },
        { name: 'UnitMeasureName' },
        { name: 'Name' },
        { name: 'MechanicSalary', defaultValue: null },
        { name: 'BaseSalary', defaultValue: null },
        { name: 'MechanicWork', defaultValue: null },
        { name: 'BaseWork', defaultValue: null },
        { name: 'TotalCount', defaultValue: null },
        { name: 'TotalCost', defaultValue: null },
        { name: 'OnUnitCount', defaultValue: null },
        { name: 'OnUnitCost', defaultValue: null },
        { name: 'MaterialCost', defaultValue: null },
        { name: 'MachineOperatingCost', defaultValue: null }
    ]
});