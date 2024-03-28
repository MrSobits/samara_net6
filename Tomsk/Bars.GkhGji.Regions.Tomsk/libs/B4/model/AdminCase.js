Ext.define('B4.model.AdminCase', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCase'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase'},
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Inspector', defaultValue: null },
        { name: 'TypeAdminCaseBase', defaultValue: 10 },
        { name: 'DescriptionQuestion' },
        { name: 'DescriptionQuestion' },
        { name: 'DescriptionSet' },
        { name: 'DescriptionDefined' },
        { name: 'Contragent', defaultValue: null },
        { name: 'TypeDocumentGji', defaultValue: 110 },
        { name: 'ParentDocument', defaultValue: null }
    ]
});