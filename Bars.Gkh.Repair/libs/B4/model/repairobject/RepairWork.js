Ext.define('B4.model.repairobject.RepairWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RepairObject', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'TypeWork' },
        { name: 'UnitMeasure' },
        { name: 'WorkName' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'VolumeOfCompletion' },
        { name: 'PercentOfCompletion' },
        { name: 'CostSum' },
        { name: 'Builder', defaultValue: null },
        { name: 'BuilderName' },
        { name: 'AdditionalDate' },
        { name: 'ControlDate' }
    ]
});