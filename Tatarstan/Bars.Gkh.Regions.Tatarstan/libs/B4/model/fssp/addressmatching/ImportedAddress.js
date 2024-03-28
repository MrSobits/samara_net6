Ext.define('B4.model.fssp.addressmatching.ImportedAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FsspAddressMatch',
        listAction: 'ImportedAddressMatchingList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FileName' },
        { name: 'FileAddress' },
        { name: 'SystemAddress' },
        { name: 'IsMatched'}
    ]
});