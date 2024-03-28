Ext.define('B4.model.objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkRealityObjectOutdoor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectOutdoorCr', defaultValue: null },
        { name: 'WorkRealityObjectOutdoor', defaultValue: null },
        { name: 'WorkRealityObjectOutdoorName' },
        { name: 'TypeWork' },
        { name: 'UnitMeasure' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'Description' },
        { name: 'IsActive', defaultValue: true }
    ]
});