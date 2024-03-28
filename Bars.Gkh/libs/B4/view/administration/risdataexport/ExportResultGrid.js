Ext.define('B4.view.administration.risdataexport.ExportResultGrid',
{
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.ux.RowExpander',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Progress',
        'B4.enums.FormatDataExportStatus',
        'B4.enums.FormadDataExportRemoteStatus'
    ],

    alias: 'widget.risdataexportresultgrid',
    title: 'Результаты экспорта',

    plugins: [
        {
            ptype: 'rowexpander',
            pluginId: 'rowExpander',
            expandOnDblClick: false,
            rowBodyTpl: [
                '<p>',
                '<tpl if="EntityCodeList">',
                '<tpl if="Status &lt; ' + B4.enums.FormatDataExportStatus.Successfull + '">',
                '<b>Экспортируемые секции: </b>',
                '<tpl else>',
                '<b>Экспортированные секции: </b>',
                '</tpl>',
                '<tpl for="EntityCodeList">',
                '<p>{.}</p>',
                '</tpl>',
                '</tpl>',
                '</p>'
            ]
        }
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.risdataexport.FormatDataExportResult');

        Ext.applyIf(me,
        {
            xtype: 'b4grid',
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'Login',
                    text: 'Пользователь'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FormatDataExportStatus',
                    filter: true,
                    flex: 1,
                    dataIndex: 'Status',
                    text: 'Статус'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'StartDate',
                    text: 'Время запуска'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'EndDate',
                    text: 'Время завершения'
                },
                {
                    xtype: 'progresscolumn',
                    dataIndex: 'Progress',
                    width: 100,
                    showValue: true,
                    text: 'Прогресс',
                },
                {
                    xtype: 'actioncolumn',
                    hideable: false,
                    width: 40,
                    align: 'center',
                    tooltip: 'Скачать лог',
                    icon: B4.Url.content('content/img/icons/disk.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.up('grid');
                        scope.fireEvent('rowaction', scope, 'getlog', rec);
                    },
                    text: 'Лог'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FormadDataExportRemoteStatus',
                    filter: true,
                    width: 150,
                    dataIndex: 'RemoteStatus',
                    text: 'Статус задачи в РИС',
                },
                {
                    xtype: 'actioncolumn',
                    hideable: false,
                    width: 40,
                    align: 'center',
                    tooltip: "Обновить статус",
                    icon: B4.Url.content('content/img/icons/arrow_refresh_small.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.up('grid');
                        scope.fireEvent('rowaction', scope, 'updatestatus', rec);
                    }
                },
                {
                    xtype: 'actioncolumn',
                    hideable: false,
                    width: 80,
                    align: 'center',
                    text: 'Лог РИС',
                    tooltip: "Получить результат загрузки",
                    icon: B4.Url.content('content/img/icons/package_down.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.up('grid');
                        scope.fireEvent('rowaction', scope, 'getuploadresult', rec);
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true,
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    name: 'buttons',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        store.load();
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});