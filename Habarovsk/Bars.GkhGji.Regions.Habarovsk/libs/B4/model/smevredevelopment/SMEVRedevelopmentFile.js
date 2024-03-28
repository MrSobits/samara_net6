Ext.define('B4.model.smevredevelopment.SMEVRedevelopmentFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVRedevelopmentFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVRedevelopment' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});