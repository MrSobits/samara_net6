Ext.define('B4.model.ValidationResult', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id', useNull: true },
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