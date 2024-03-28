Ext.define('B4.model.smev2.SMEVStayingPlaceFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVStayingPlaceFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVStayingPlace' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});