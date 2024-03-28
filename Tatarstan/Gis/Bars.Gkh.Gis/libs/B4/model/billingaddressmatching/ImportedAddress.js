Ext.define('B4.model.billingaddressmatching.ImportedAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddressMatching',
        listAction: 'GetImportAddresses'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ImportType' },
        { name: 'ImportFilename' },
        { name: 'AddressCodeRemote' },
        { name: 'AddressRemote' },
        { name: 'AddressCode' },
        { name: 'Address' },
        { name: 'IsMatched' }
    ]
});
