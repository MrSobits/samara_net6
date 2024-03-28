Ext.define('B4.model.dict.ServiceType', {
    extend: 'Ext.data.Model',
    idProperty: 'Value',
    proxy: {
        type: 'memory'
    },
    fields: [
        { name: 'Value'},
        { name: 'Display' }
    ]
});