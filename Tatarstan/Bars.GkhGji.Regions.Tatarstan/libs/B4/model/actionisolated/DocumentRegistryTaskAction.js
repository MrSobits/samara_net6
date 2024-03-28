Ext.define('B4.model.actionisolated.DocumentRegistryTaskAction', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolated',
        listAction: 'ListForDocumentRegistry'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'TypeBase' },
        { name: 'TypeBaseAction' },
        { name: 'TypeDocumentGji' },
        { name: 'InspectionId' },
        { name: 'Inspectors' },
        { name: 'KindAction' },
        { name: 'ControlType' },
        { name: 'Address' },
        { name: 'CountOfHouses' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'JurPerson' },
        { name: 'PhysicalPerson' },
        { name: 'DateStart' },
        { name: 'Done' }
    ]
});