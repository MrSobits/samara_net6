Ext.define('B4.model.ProtocolGji', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'DateToCourt' },
        { name: 'BirthDay' },
        { name: 'ToCourt', defaultValue: false },
        { name: 'Description' },
        { name: 'ViolationsList', defaultValue: null },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 60 },
        { name: 'ContragentName' },
        { name: 'TypeExecutant' },
        { name: 'MunicipalityNames' },
        { name: 'CountViolation' },
        { name: 'PhysicalPersonPosition' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'ResolutionId' },
        { name: 'ControlType' },
        { name: 'MoSettlement' },
        { name: 'UIN' },
        { name: 'PlaceName' },
        { name: 'ArticleLaw' }
    ]
});