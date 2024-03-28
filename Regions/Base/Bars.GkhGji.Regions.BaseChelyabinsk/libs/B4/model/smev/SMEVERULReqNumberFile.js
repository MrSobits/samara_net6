Ext.define('B4.model.smev.SMEVERULReqNumberFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVERULReqNumberFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVERULReqNumber' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});