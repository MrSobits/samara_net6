Ext.define('B4.model.RecordComparison', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    fields: [
        { name: 'ExternalEntity' },
        { name: 'GisRecord' }
    ],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});
