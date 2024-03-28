Ext.define('B4.model.appealcits.MotivatedPresentation', {
    extend: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationAppealCits'
    },
    fields: [
        { name: 'TypeDocumentGji', defaultValue: 250 },
        { name: 'Stage', defaultValue: 290 },
        { name: 'AppealCitsFormatted' },
        { name: 'AppealCits' },
        { name: 'PresentationType' },
        { name: 'Inspectors' },
        { name: 'InspectorIds' },
        { name: 'Official' },
        { name: 'ResultType' },
        { name: 'InspectionId' }
    ]
});