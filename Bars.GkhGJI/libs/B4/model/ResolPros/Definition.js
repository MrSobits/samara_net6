Ext.define('B4.model.resolpros.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolProsDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ResolPros', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'SignedBy', defaultValue: null },
        { name: 'FileInfo', defaultValue: null },
        { name: 'TypeDefinition', defaultValue: 10 },
        { name: 'TimeDefinition' },
        { name: 'DateOfProceedings' }
    ]
});