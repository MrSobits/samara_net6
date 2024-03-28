Ext.define('B4.model.wastecollection.WasteCollectionPlace', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WasteCollectionPlace'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Address' },
        { name: 'Customer', defaultValue: null },
        { name: 'TypeWaste' },
        { name: 'TypeWasteCollectionPlace' },
        { name: 'PeopleCount' },
        { name: 'ContainersCount' },
        { name: 'WasteAccumulationDaily' },
        { name: 'LandfillDistance' },
        { name: 'Comment' },
        { name: 'ExportDaysWinter' },
        { name: 'ExportDaysSummer' },
        { name: 'Contractor', defaultValue: null },
        { name: 'JuridicalAddress' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'NumberContract' },
        { name: 'DateContract' },
        { name: 'FileContract', defaultValue: null },
        { name: 'LandfillAddress' }
    ]
});