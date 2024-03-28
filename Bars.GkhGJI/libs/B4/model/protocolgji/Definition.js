Ext.define('B4.model.protocolgji.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol', defaultValue: null },
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