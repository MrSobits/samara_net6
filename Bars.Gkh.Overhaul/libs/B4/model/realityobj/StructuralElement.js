Ext.define('B4.model.realityobj.StructuralElement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElement',
        timeout: 5 * 60 * 1000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Multiple' },
        { name: 'Object' },
        { name: 'ObjectId' },
        { name: 'Group' },
        { name: 'ElementName' },
        { name: 'UnitMeasure' },
        { name: 'Volume' },
        { name: 'Wearout' },
        { name: 'WearoutActual' },
        { name: 'Repaired' },
        { name: 'LastOverhaulYear' },
        { name: 'RequiredFieldsFilled' },
        { name: 'RealityObject', useNull: true, defaultValue: null },
        { name: 'StructuralElement', useNull: true, defaultValue: null },
        { name: 'State' },
        { name: 'Values' },
        { name: 'PlanRepairYear' },
        { name: 'Condition' },
        { name: 'SystemType' },
        { name: 'NetworkLength' },
        { name: 'NetworkPower' },
        { name: 'AdjustedYear' },
        { name: 'FileInfo' }
    ]
});