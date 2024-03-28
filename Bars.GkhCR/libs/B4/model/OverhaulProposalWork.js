Ext.define('B4.model.OverhaulProposalWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OverhaulProposalWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OverhaulProposal', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'WorkName' },
        { name: 'TypeWork' },
        { name: 'UnitMeasureName' },       
        { name: 'Sum' },
        { name: 'Description' },
        { name: 'DateStartWork' },
        { name: 'DateEndWork' },    
        { name: 'Volume' }       
    ]
});