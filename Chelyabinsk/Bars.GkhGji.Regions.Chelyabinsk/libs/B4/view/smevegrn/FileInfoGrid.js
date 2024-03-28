Ext.define('B4.view.smevegrn.FileInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.smevegrnfileinfogrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.SMEVFileType',
    ],

    title: 'Файлы запросов',
    store: 'smev.SMEVEGRNFile',
  //  itemId: 'risExportTaskDocumentGJIGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.SMEVFileType',
                    dataIndex: 'SMEVFileType',
                    text: 'Тип',
                    flex: 0.7,
                    filter: true,
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {              
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFile?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'checkbox',
                                    labelWidth: 150,
                                    fieldLabel: 'Показать системные файлы',
                                    itemId: 'showSysFiles'
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