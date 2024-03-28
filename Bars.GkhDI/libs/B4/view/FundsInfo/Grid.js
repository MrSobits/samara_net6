Ext.define('B4.view.fundsinfo.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.view.Control.GkhDecimalField'
    ],
    store: 'FundsInfo',
    itemId: 'fundsInfoGrid',
    title: 'Сведения о фондах',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Size',
                    flex: 1,
                    text: 'Размер',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'По состоянию на',
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
                plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});