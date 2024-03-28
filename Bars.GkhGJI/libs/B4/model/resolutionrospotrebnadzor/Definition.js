Ext.define('B4.model.resolutionrospotrebnadzor.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', useNull: false},
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 }
    ]
});