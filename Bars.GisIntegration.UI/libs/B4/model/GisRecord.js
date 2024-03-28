Ext.define('B4.model.GisRecord', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    fields: [
        { name: 'Code' },
        { name: 'Guid' },
        { name: 'Name' }
    ],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});
