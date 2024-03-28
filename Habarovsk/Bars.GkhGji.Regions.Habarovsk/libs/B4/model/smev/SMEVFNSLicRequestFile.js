Ext.define('B4.model.smev.SMEVFNSLicRequestFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVFNSLicRequestFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVFNSLicRequest' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});