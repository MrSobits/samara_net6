Ext.define('B4.model.smev2.SMEVLivingPlaceFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVLivingPlaceFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVLivingPlace' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});