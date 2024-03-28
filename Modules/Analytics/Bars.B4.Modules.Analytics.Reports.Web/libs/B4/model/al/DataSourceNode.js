Ext.define('B4.model.al.DataSourceNode', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'text' },
        { name: 'children' },
        { name: 'isLeaf' },
        { name: 'type' }
    ]
});