Ext.define('B4.model.protocolmhc.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMhcDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolMhc', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 }
    ]
});