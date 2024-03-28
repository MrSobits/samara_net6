Ext.define('B4.model.Disposal', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Disposal'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'IssuedDisposal', defaultValue: null },
        { name: 'ResponsibleExecution', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TypeDisposal', defaultValue: 10 },
        { name: 'TypeAgreementProsecutor', defaultValue: 10 },
        { name: 'TypeAgreementResult', defaultValue: 10 },
        { name: 'KindCheck', defaultValue: null },
        { name: 'ActCheckGeneralId' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 10 },
        { name: 'IsActCheckExist' },
        { name: 'RealityObjectCount' },
        { name: 'TypeSurveyNames' },
        { name: 'TypeBase' },
        { name: 'ContragentName' },
        { name: 'MunicipalityNames' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'InspectionType', defaultValue: null },
        { name: 'Description' },
        { name: 'ObjectVisitStart' },
        { name: 'ObjectVisitEnd' },
        { name: 'OutInspector', defaultValue: false },
        { name: 'HasChildrenActCheck', defaultValue: false },
        { name: 'HasActSurvey', defaultValue: false },
        { name: 'VerificationPurpose' }
    ]
});