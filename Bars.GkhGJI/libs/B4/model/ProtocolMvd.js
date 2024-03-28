Ext.define('B4.model.ProtocolMvd', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMvd'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'InspectionId', defaultValue: null },
        { name: 'BlockResolution', defaultValue: false },
        { name: 'TypeExecutant', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'DateSupply' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'Description' },
        { name: 'TypeDocumentGji', defaultValue: 120 },
        { name: 'OrganMvd' },
        { name: 'DateOffense' },
        { name: 'TimeOffense' },
        { name: 'SerialAndNumber' },
        { name: 'BirthDate' },
        { name: 'IssueDate' },
        { name: 'BirthPlace' },
        { name: 'IssuingAuthority' },
        { name: 'Company' }
    ]
});