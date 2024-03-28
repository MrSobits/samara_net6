Ext.define('B4.model.ProtocolRSO', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolRSO'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'InspectionId' },
        { name: 'Executant', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'GasSupplier', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'DateSupply' },
        { name: 'PhysicalPerson' },
        { name: 'DocUIN' },
        { name: 'PhysicalPersonInfo' },
        { name: 'TypeDocumentGji', defaultValue: 190 },
        { name: 'TypeSupplierProtocol', defaultValue: 0 },
        { name: 'Koap' },
    ]
});