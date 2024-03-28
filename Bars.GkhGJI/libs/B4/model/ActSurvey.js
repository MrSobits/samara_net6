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
        { name: 'ControlType' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});