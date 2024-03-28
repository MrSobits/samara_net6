Ext.define('B4.model.smev.PayRegFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PayRegFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'PayReg' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});