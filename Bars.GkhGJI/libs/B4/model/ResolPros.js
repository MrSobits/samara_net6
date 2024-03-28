Ext.define('B4.model.ResolPros', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolPros'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'InspectionId', defaultValue: null },
        { name: 'BlockResolution', defaultValue: false },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'ActCheck', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'DateSupply' },
        { name: 'PhysicalPersonPosition' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'ProsecutorOffice' },

        { name: 'IssuedByPosition' },
        { name: 'IssuedByFio' },
        { name: 'IssuedByRank' },

        { name: 'TypePresence', defaultValue: 0 },
        { name: 'PersonRegistrationAddress' },
        { name: 'PersonFactAddress' },
        { name: 'Representative' },
        { name: 'ReasonTypeRequisites' },
        { name: 'PersonBirthDate' },
        { name: 'PersonBirthPlace' },
        { name: 'Surname' },
        { name: 'FirstName' },
        { name: 'Patronymic' },
        { name: 'PhysicalPersonDocumentNumber' },
        { name: 'PhysicalPersonIsNotRF', defaultValue: false },
        { name: 'PhysicalPersonDocumentSerial' },
        { name: 'PhysicalPersonDocType' },

        { name: 'Description' },
        { name: 'UIN' },
        { name: 'TypeDocumentGji', defaultValue: 80 },
        { name: 'FormatHour' },
        { name: 'FormatMinute' },
        { name: 'DateResolPros'}
    ]
});