Ext.define('B4.model.GisDictionary', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    fields: [
        { name: 'RegistryNumber', type: 'int' },
        { name: 'Name' },
        { name: 'Group' },
        { name: 'Modified' }
    ],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});
