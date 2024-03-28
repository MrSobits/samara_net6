Ext.define('B4.model.Dictionary', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Dictionary'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'GisRegistryNumber' },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'State' },
        { name: 'LastRecordsCompareDate' },
        { name: 'Group' },
        { name: 'CompareDictionaryEnabled' },
        { name: 'CompareRecordsEnabled' }
    ]
});
