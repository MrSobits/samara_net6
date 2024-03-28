Ext.define('B4.model.program.PriorityParam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementInProgrammStage3',
        listAction: 'GetParams'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name'}
    ]
});