Ext.define('B4.view.gisGkh.DownloadGrid', {
    //extend: 'Ext.tree.Panel',
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisgkhdownloadgrid',

    requires: [
        //'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
    ],

    store: 'gisGkh.DownloadGridStore',
    title: 'Скачивание файлов',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Guid',
                    flex: 1,
                    text: 'Guid',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EntityT',
                    flex: 1,
                    text: 'EntityT',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileField',
                    flex: 1,
                    text: 'FileField',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecordId',
                    flex: 1,
                    text: 'RecordId',
                    filter: {
                        xtype: 'textfield',
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: true,
                //    showClearAllButton: true,
                //    pluginId: 'headerFilter'
                //},
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Загрузить файлы',
                                    tooltip: 'Загрузить все файлы',
                                    iconCls: 'icon-accept',
                                    width: 130,
                                    itemId: 'btnDownload'
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
