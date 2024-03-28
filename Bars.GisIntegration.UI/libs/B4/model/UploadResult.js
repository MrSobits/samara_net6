Ext.define('B4.model.UploadResult', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'State' },
        { name: 'Message' }
    ],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});