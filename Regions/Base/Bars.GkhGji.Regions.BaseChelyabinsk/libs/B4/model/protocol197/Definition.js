Ext.define('B4.model.protocol197.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197Definition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol197', defaultValue: null },
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