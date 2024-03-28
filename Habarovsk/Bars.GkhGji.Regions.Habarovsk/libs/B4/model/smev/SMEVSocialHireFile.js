Ext.define('B4.model.smev.SMEVSocialHireFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVSocialHireFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVSocialHire' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});