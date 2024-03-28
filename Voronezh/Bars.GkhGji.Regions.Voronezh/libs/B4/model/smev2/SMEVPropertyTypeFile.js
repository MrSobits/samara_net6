Ext.define('B4.model.smev2.SMEVPropertyTypeFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVPropertyTypeFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVPropertyType' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});