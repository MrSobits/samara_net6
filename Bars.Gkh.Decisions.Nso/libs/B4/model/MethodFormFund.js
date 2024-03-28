Ext.define('B4.model.MethodFormFund', {
    extend: 'Ext.data.Model',
    idProperty: 'Value',
    proxy: {
        type: 'memory'
    },
    fields: [
        { name: 'Value' },
        { name: 'Display' }
    ]
});