Ext.define('B4.ux.config.GenericCellEditing', {
    extend: 'Ext.grid.plugin.CellEditing',
    alias: 'plugin.genericcellediting',

    getEditor: function(record, column) {
        return this.callParent([
            record,
            {
                getItemId: function() { return (column.getId() || column.itemId) + '_' + (record.get('id') || record.id); },
                getEditor: column.getEditor.bind(column)
            }
        ]);
    }
});