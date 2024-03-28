Ext.define('B4.model.program.SecondStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgrammStage2'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObject' },
        { name: 'StructuralElements' },
        { name: 'Year' },
        { name: 'Sum' }
    ]
});