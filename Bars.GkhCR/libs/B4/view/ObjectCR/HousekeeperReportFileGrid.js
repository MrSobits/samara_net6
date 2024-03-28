Ext.define('B4.view.objectcr.HousekeeperReportFileGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.housekeeperreportfilegrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Add',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Подтверждающие материалы',
    store: 'objectcr.HousekeeperReportFile',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    text: 'Имя файла',
                    flex: 1,
                    renderer: function (v) {
                        return v.Name;
                    }
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 2,
                    text: 'Описание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                                { xtype: 'b4addbutton' },
                                {
                                    xtype: 'b4updatebutton'
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