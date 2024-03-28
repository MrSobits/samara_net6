Ext.define('B4.model.preventiveaction.DocumentRegistryPreventiveAction',{
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveAction',
        listAction: 'ListForDocumentRegistry'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'State' },
        { name: 'TypeDocumentGji' },
        { name: 'TypeBase' },
        { name: 'InspectionId' },
        { name: 'Municipality' },
        { name: 'ControlType' },
        { name: 'ActionType' },
        { name: 'VisitType' },
        { name: 'ControlledOrganization' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'ActionStartDate' },
        { name: 'Inspectors' }
    ]
});