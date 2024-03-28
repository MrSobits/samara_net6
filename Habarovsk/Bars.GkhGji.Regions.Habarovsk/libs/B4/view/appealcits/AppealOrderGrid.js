Ext.define('B4.view.appealcits.AppealOrderGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.appealordergrid',
    store: 'appealcits.AppealOrder',
    closable: true,
    title: 'Реестр СОПР',

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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executant',
                    flex: 1,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentINN',
                    flex: 1,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 80,
                    text: 'Номер обращения',
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата регистрации обращения',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OrderDate',
                    flex: 0.5,
                    text: 'Дата размещения в СОПР',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    },
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerformanceDate',
                    flex: 0.5,
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'YesNoNotSet',
                    text: 'Отработано',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Confirmed',
                    text: 'Принято инспектором',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    flex: 1,
                    hidden: true,
                    text: 'Заявитель'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrespondentAddress',
                    flex: 1,
                    hidden: true,
                    text: 'Адрес заявителя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл обращения',
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
                            columns: 6,
                            items: [                            
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
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowCloseAppeals',
                                    boxLabel: 'Показать закрытые обращения',
                                    labelAlign: 'right',
                                    checked: false
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