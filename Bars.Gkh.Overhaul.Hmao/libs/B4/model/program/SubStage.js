Ext.define('B4.model.program.SubStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgrammStage3',
        listAction: 'SubList',
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObjects' },
        { name: 'YearCalculated' },
        { name: 'Year' },
        { name: 'IndexNumber' },
        { name: 'Point' },
        { name: 'Sum' }
    ]
});