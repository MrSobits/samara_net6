Ext.define('B4.view.tablelock.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Реестр блокировок таблиц',
    store: 'TableLock',
    alias: 'widget.tableLockGrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TableName',
                    flex: 1,
                    text: 'Таблица',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    text: 'Действие',
                    filter: { xtype: 'textfield' },
                    width: 150
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LockStart',
                    text: 'Время блокировки',
                    format: 'd.m.Y H:m:s',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    width: 150
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-cross',
                                    text: 'Снять все блокировки',
                                    textAlign: 'left',
                                    action: 'UnlockAll'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});