Ext.define('B4.model.DictionaryRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Dictionary',
        listAction: 'ListRecords'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ExternalId' },
        { name: 'ExternalName' },
        { name: 'GisCode' },
        { name: 'GisName' },
        { name: 'GisGuid' }
    ]
});
