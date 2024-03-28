Ext.define('B4.model.ProtocolMhc', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMhc'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'InspectionId' },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'DateSupply' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'TypeDocumentGji', defaultValue: 130 }
    ]
});