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
        { name: 'ToCourt', defaultValue: false },
        { name: 'Description' },
        { name: 'ViolationsList', defaultValue: null },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 60 },
        { name: 'ContragentName' },
        { name: 'TypeExecutant' },
        { name: 'MunicipalityNames' },
        { name: 'CountViolation' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'ResolutionId' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'FormatPlace' },   
        { name: 'NotifDeliveredThroughOffice' },
        { name: 'FormatDate' },
        { name: 'NotifNumber' },
        { name: 'DateOfProceedings' },
        { name: 'HourOfProceedings' },
        { name: 'MinuteOfProceedings' },
        { name: 'ProceedingCopyNum' },
        { name: 'ProceedingsPlace' },
        { name: 'Remarks' },
        { name: 'PersonInspectionAddress' },
        { name: 'DocumentPlace' },
        { name: 'DateWriteOut' },
        { name: 'Surname' },
        { name: 'Name' },
        { name: 'Patronymic' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'FactAddress' },
        { name: 'Citizenship' },
        { name: 'CitizenshipType' },
        { name: 'SerialAndNumber' },
        { name: 'IssueDate' },
        { name: 'IssuingAuthority' },
        { name: 'Snils' },
        { name: 'Company' }
    ]
});