Ext.define('B4.view.administration.TariffDataIntegrationLogGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.store.administration.TariffDataIntegrationLog',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Update',
        'B4.enums.TariffDataIntegrationMethod',
        'B4.enums.ExecutionStatus'
    ],

    title: 'Лог интеграции данных по тарифам',
    alias: 'widget.tariffDataIntegrationLogGrid',
    closable: true,
    store: 'administration.TariffDataIntegrationLog',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TariffDataIntegrationMethod',
                    text: 'Метод',
                    enumName: 'B4.enums.TariffDataIntegrationMethod',
                    flex: 0.7,
                    width: 150,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Login',
                    text: 'Логин',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartMethodTime',
                    width: 150,
                    text: 'Время запуска метода',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Parameters',
                    text: 'Параметры',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ExecutionStatus',
                    text: 'Статус выполнения',
                    enumName: 'B4.enums.ExecutionStatus',
                    width: 130,
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'LogFileId',
                    width: 85,
                    text: 'Лог операции',
                    renderer: function (value, metadata) {
                        metadata.style = 'text-align: center;';
                        this.icon = value > 0 ? B4.Url.content('content/img/download.png') : null;
                        return "";
                    },
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var fileId = rec.getData().LogFileId;
                        if (fileId > 0) {
                            var url = B4.Url.action('Download', 'FileUpload', { Id: fileId });
                            window.open(url);
                            return;
                        }
                    }
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
                            xtype: 'datefield',
                            fieldLabel: 'Дата интеграции с',
                            labelWidth: 105,
                            itemId: 'beginDate',
                            value: new Date()
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'по',
                            labelWidth: 20,
                            itemId: 'endDate',
                            value: new Date()
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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