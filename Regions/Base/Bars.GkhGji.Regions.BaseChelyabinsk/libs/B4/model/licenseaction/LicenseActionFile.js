Ext.define('B4.model.licenseaction.LicenseActionFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicenseActionFile'
    },
    fields: [
        { name: 'Id' },
        { name: 'FileName' },
        { name: 'SignedInfo' },
        { name: 'FileInfo' }
    ]
});