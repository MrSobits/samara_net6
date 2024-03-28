Ext.define('B4.model.OverhaulProposal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OverhaulProposal'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'Municipality' },
        { name: 'ProgramCr' },       
        { name: 'ProgramNum' },
        { name: 'DateEndBuilder' },
        { name: 'DateStartWork' },     
        { name: 'Description' },
        { name: 'Apartments' },
        { name: 'Index' },
        { name: 'Entryes' },
        { name: 'State', defaultValue: null },
      
    ]
});