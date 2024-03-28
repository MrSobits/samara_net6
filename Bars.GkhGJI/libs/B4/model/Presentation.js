Ext.define('B4.model.Presentation', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Presentation'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Official', defaultValue: null },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'TypeInitiativeOrg', defaultValue: 10 },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 90 },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'MunicipalityName' },
        { name: 'ContragentName' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'RequirementText' },
        { name: 'DescriptionSet' },
        { name: 'ExecutantPost' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});