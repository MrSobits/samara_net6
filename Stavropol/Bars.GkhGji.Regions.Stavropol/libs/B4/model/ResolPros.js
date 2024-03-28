Ext.define('B4.model.ResolPros', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StavropolResolPros'
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
        { name: 'Official' },
        { name: 'DateSupply' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'Description' },
        { name: 'TypeDocumentGji', defaultValue: 80 }
    ]
});