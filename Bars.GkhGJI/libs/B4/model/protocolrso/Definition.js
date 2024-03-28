Ext.define('B4.model.protocolrso.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolRSODefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolRSO', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 }
    ]
});