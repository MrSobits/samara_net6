Ext.define('B4.model.licensereissuance.LicenseReissuanceProvDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicenseReissuanceProvDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicenseReissuance' },
        { name: 'LicProvidedDoc' },
        { name: 'Number' },
        { name: 'Date' },
        { name: 'SignedInfo' },
        { name: 'File', defaultValue: null }
    ]
});