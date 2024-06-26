Ext.define('B4.model.Prescription', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Prescription'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'Description' },
        { name: 'ViolationsList' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 50 },
        { name: 'ContragentName' },
        { name: 'TypeExecutant' },
        { name: 'MunicipalityNames' },
        { name: 'CountRealityObject' },
        { name: 'CountViolation' },
        { name: 'DocumentNumber' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'DateRemoval' },
        { name: 'DisposalId' },
        { name: 'Closed' },
        { name: 'CloseReason' },
        { name: 'CloseNote' },
        { name: 'IsFamiliar', defaultValue: 20 },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'PersonInspectionAddress' }
    ]
});