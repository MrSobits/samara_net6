/*
В Сахе сущность перекрвается с новыми полями
*/
Ext.define('B4.model.ActSurvey', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurvey'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'Area' },
        { name: 'Flat' },
        { name: 'Reason' },
        { name: 'Description' },
        { name: 'FactSurveyed', defaultValue: 10 },
        { name: 'RealityObjectsList' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 40 },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'State', defaultValue: null },
        { name: 'MunicipalityNames' },
        { name: 'RealityObjectAddresses' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'DateOf' },
        { name: 'TimeStart' },
        { name: 'TimeEnd' },
        { name: 'ConclusionIssued', defaultValue: 20 }
    ]
});