Ext.define('B4.model.program.ThirdStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgrammStage3'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObjects' },
        { name: 'Year' },
        { name: 'IndexNumber' },
        { name: 'Point' },
        { name: 'Sum' }
    ]
});