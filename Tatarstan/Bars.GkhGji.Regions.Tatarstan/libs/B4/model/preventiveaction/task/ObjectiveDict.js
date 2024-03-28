Ext.define('B4.model.preventiveaction.task.ObjectiveDict', {
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