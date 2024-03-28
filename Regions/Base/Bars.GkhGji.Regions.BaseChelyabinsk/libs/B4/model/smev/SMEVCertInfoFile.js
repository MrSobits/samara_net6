Ext.define('B4.model.smev.SMEVCertInfoFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVCertInfoFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVCertInfo' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});