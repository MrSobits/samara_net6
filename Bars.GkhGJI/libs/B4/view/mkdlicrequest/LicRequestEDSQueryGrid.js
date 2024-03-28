Ext.define('B4.view.mkdlicrequest.LicRequestEDSQueryGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.licrequestedsquerygrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign'
    ],

    title: 'Запросы',
    store: 'mkdlicrequest.LicRequestEDSQuery',
    //itemId: 'mkdlicRequestQueryGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StatementNumber',
                    flex: 1,
                    text: 'Номер обращения',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StatementDate',
                    flex: 1,
                    text: 'Дата обращения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер запроса',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата запроса',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SendDate',
                    flex: 1,
                    text: 'Дата отправки запроса',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CompetentOrg',
                    flex: 1,
                    text: 'Адресат',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceDate',
                    flex: 1,
                    text: 'Срок исполнения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceFactDate',
                    flex: 1,
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Инспектор',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedFile',
                    width: 100,
                    text: 'Подписанный файл',
                    flex: 1,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFileFromPrivateServer?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                            columns: 5,
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 60,
                                    fieldLabel: 'Дата с',
                                    width: 160,
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    fieldLabel: 'по',
                                    width: 130,
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
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