Ext.define('B4.model.constructionobject.TypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructionobjecttypework',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject' },
        { name: 'Work' },
        { name: 'YearBuilding' },
        { name: 'TypeWork', defaultValue: 10 },
        { name: 'UnitMeasureName' },
        { name: 'WorkName' },
        { name: 'HasPsd' },
        { name: 'HasExpertise' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'Description' },
        { name: 'DateStartWork', type: 'date' },
        { name: 'DateEndWork', type: 'date' },
        { name: 'VolumeOfCompletion' },
        { name: 'PercentOfCompletion' },
        { name: 'CostSum' },
        { name: 'CountWorker' },
        { name: 'Deadline', type: 'date' }
    ]
});