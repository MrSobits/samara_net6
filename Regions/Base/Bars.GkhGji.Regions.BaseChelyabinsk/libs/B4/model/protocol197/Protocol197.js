﻿Ext.define('B4.model.protocol197.Protocol197', {
    extend: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197'
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
        { name: 'PhysicalPersonPosition' },
        { name: 'CountViolation' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'ResolutionId' },
        { name: 'DateOfProceedings' },
        { name: 'HourOfProceedings' },
        { name: 'MinuteOfProceedings' },
        { name: 'FormatDate' },
        { name: 'NotifNumber' },
        { name: 'ProceedingsPlace' },
        { name: 'Remarks' },
        { name: 'PersonRegistrationAddress' },
        { name: 'PersonFactAddress' },
        { name: 'PersonJob' },
        { name: 'PersonPosition' },
        { name: 'PersonBirthDatePlace' },
        { name: 'PersonDoc' },
        { name: 'PersonSalary' },
        { name: 'PersonRelationship' },
        { name: 'FormatPlace' },
        { name: 'FormatHour' },
        { name: 'FormatMinute' },
        { name: 'TypePresence' },
        { name: 'Representative' },
        { name: 'ReasonTypeRequisites' },
        { name: 'NotifDeliveredThroughOffice' },
        { name: 'Familrefusal', defaultValue: false },
        { name: 'ProceedingCopyNum' },
        { name: 'DateOfViolation' },
        { name: 'HourOfViolation' },
        { name: 'MinuteOfViolation' },
        { name: 'ResolveViolationClaim' },
        { name: 'NormativeDoc' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'UIN' },
        { name: 'PhysicalPersonDocumentNumber', defaultValue: 'sdfsdfdf' },
        { name: 'PhysicalPersonIsNotRF', defaultValue: false },
        { name: 'PhysicalPersonDocumentSerial' },
        { name: 'PhysicalPersonDocType' },
        { name: 'ArticleLaw' },
        { name: 'ControlType' },
        { name: 'HasResolution' },
        { name: 'FiasPlaceAddress' },
        { name: 'TypeAnnex' },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'TypeAddress', defaultValue: 0 },
        { name: 'PlaceOffense', defaultValue: 0 },
        { name: 'JudSector' },
        { name: 'AddressPlace' }
    ]
});