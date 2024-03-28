Ext.define('B4.model.actionisolated.MotivatedPresentation', {
    extend: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentation'
    },
    fields: [
        { name: 'TypeDocumentGji', defaultValue: 230 },
        { name: 'Stage', defaultValue: 260 },
        { name: 'CreationPlace' },
        { name: 'IssuedMotivatedPresentation' },
        { name: 'ResponsibleExecution' },
        { name: 'Inspectors' },
        { name: 'InspectorIds' },
        { name: 'ParentDocumentType' }
    ]
});