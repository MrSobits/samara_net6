Ext.define('B4.model.actisolated.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 }
    ]
});