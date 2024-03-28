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
        { name: 'ExecutionTime', type: 'date', format: 'MS' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'ResolutionInitAdminViolation' },
        { name: 'ReturnReason' },
        { name: 'RequestNeed' },
        { name: 'AdditionalDocuments' },
        { name: 'TypeResolProsDefinition', defaultValue: 10 },
        { name: 'DateSubmissionDocument' }
    ]
});