Ext.define('B4.model.appealcits.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'FileInfo', defaultValue: null },
        { name: 'TypeDefinition', defaultValue: 5 }
    ]
});