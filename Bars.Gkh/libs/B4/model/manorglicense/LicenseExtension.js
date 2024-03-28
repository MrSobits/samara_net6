Ext.define('B4.model.manorglicense.LicenseExtension', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseExtension'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManOrgLicense' },
        { name: 'DocType', defaultValue: 10 },
        { name: 'DocNumber' },
        { name: 'DocDate' },
        { name: 'File' }
    ]
});