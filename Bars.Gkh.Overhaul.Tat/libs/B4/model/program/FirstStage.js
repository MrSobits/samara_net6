Ext.define('B4.model.program.FirstStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgramm'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObject' },
        { name: 'StructuralElement' },
        { name: 'UnitMeasure' },
        { name: 'Volume' },
        { name: 'Year' },
        { name: 'Sum' },
        { name: 'ServiceCost' }
    ]
});