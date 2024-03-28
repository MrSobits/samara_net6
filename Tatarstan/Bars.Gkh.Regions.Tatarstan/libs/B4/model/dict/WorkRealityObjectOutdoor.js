Ext.define('B4.model.dict.WorkRealityObjectOutdoor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRealityObjectOutdoor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'TypeWork', defaultValue: 10 },
        { name: 'UnitMeasure' },
        { name: 'IsActual' },
        { name: 'Description' }
    ]
});