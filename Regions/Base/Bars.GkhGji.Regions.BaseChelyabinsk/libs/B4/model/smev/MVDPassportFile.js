Ext.define('B4.model.smev.MVDPassportFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MVDPassportFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'MVDPassport' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});