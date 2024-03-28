Ext.define('B4.model.MotivatedPresentationAppealCits', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationAppealCits'
    },
    fields: [
        { name: 'MunicipalityNames' },
        { name: 'InspectorNames' },
        { name: 'Address' },
        { name: 'PresentationType' },
        { name: 'ResultType' },
        { name: 'TypeBase' },
        { name: 'InspectionId' }
    ]
});