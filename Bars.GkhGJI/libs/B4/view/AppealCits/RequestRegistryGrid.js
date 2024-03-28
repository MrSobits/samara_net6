Ext.define('B4.view.appealcits.RequestRegistryGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealcitsrequestregistrygrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
    
    ],
    closable: true,
    title: 'Реестр запросов ГЖИ',
    store: 'appealcits.RequestRegistry',

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
                    dataIndex: 'AppealNumber',
                    flex: 1,
                    text: 'Номер обращения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AppealDate',
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
                    filter: { xtype: 'textfield' }
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
                    filter: { xtype: 'textfield' }

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
                    dataIndex: 'SenderInspector',
                    flex: 1,
                    text: 'Инспектор',
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'File',
                //    width: 100,
                //    text: 'Файл',
                //    renderer: function (v) {
                //        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedFile',
                    width: 100,
                    text: 'Подписанный файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFileFromPrivateServer?id=' + v.Id) + '" target="_blank" style="color: black">Скачать PDF</a>') : '';
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
                            columns: 4,
                            items: [ 
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 60,
                                    fieldLabel: 'Период с',
                                    labelAlign: 'right',
                                    width: 160,
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    labelAlign: 'right',
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