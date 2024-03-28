Ext.define('B4.view.contextsettings.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.enums.FileStorageName',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Настройки подсистем',
    store: 'ContextSettings',
    alias: 'widget.contextsettingsGrid',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FileStorageName',
                    dataIndex: 'FileStorageName',
                    flex: 1,
                    text: 'Наимемнование подсистемы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Context',
                    flex: 1,
                    text: 'Контекст'
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
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