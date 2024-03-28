Ext.define('B4.model.manorglicense.RequestProvDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgRequestProvDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicRequest' },
        { name: 'LicProvidedDoc' },
        { name: 'Number' },
        { name: 'Date' },
        { name: 'SignedInfo' },
        { name: 'File', defaultValue: null }
    ]
});