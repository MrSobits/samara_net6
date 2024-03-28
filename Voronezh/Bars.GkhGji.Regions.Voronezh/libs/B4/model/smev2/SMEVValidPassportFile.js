Ext.define('B4.model.smev2.SMEVValidPassportFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVValidPassportFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVValidPassport' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});