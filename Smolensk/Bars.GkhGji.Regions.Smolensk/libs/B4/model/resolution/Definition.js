//ToDo данная модель расширена полями 
Ext.define('B4.model.resolution.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 },
        { name: 'DefinitionResult' },
        { name: 'DescriptionSet' }
    ]
});