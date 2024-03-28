Ext.define('B4.model.program.thirddetails.WorkType', {
    extend: 'B4.base.Model',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgrammStage3',
        listAction: 'ListWorkTypes'
    },

    fields: [
        { name: 'WorkType' },
        { name: 'WorkKind' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'StructElement' }
    ]
});