Ext.define('B4.view.import.chesimport.SumsPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.enums.regop.WalletType',
        'B4.form.SelectField',
        'B4.view.import.chesimport.payments.SummaryGrid',
        'B4.store.import.chesimport.payments.Summary',
        'B4.store.regop.ChargePeriod'
    ],

    title: 'Сводные суммы',
    alias: 'widget.chesimportsumspanel',

    bodyStyle: Gkh.bodyStyle,

    closable: true,

    chargeStore: null,
    paymentStore: null,
    saldoStore : null,
    recalcStore: null,

    _decimalStyle: '<div style="font-size: 11px; line-height: 13px;">{0}</div>',
    _textStyle: '<div style="font-size: 11px; line-height: 13px; text-align: right">{0}</div>',

    initComponent: function() {
        var me = this,
            decimalRenderer = function(val) { return Ext.util.Format.currency(val || 0); },            
            summaryRenderer = function (val, summaryData, dataIndex) {
                return Ext.isNumber(val) ? Ext.String.format(me._decimalStyle, Ext.util.Format.currency(val)) : '';
            };

        me.chargeStore = Ext.create('B4.store.import.chesimport.ChargeSummary');
        me.paymentStore = Ext.create('B4.store.import.chesimport.payments.Summary');
        me.saldoStore = Ext.create('B4.store.import.chesimport.SaldoChangeSummary');
        me.recalcStore = Ext.create('B4.store.import.chesimport.RecalcSummary');

        me.relayEvents(me.chargeStore, ['beforeload'], 'chargeStore.');
        me.relayEvents(me.paymentStore, ['beforeload'], 'paymentStore.');
        me.relayEvents(me.saldoStore, ['beforeload'], 'saldoStore.');
        me.relayEvents(me.recalcStore, ['beforeload'], 'recalcStore.');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowX: 'auto',
                    layout: { type: 'anchor' },
                    defaults: {
                        xtype: 'panel',
                        collapsible: true,
                        collapsed: false,
                        flex: 1
                    },
                    items: [
                        {
                            title: 'Начисления',
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    store: me.chargeStore,
                                    columnLines: true,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'WalletType',
                                            text: '',
                                            width: 150,
                                            renderer: function(val) {
                                                return Ext.String.format(me._textStyle, B4.enums.regop.WalletType.displayRenderer(val) + ':');
                                            },
                                            summaryRenderer: function (val, summaryData) {
                                                return Ext.String.format(me._textStyle, 'Итого:');
                                            }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'SaldoIn',
                                            text: 'Входящее сальдо',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Charged',
                                            text: 'Начислено',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Paid',
                                            text: 'Оплачено',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'SaldoChange',
                                            text: 'Изменение сальдо',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Recalc',
                                            text: 'Перерасчет',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'SaldoOut',
                                            text: 'Исходящее сальдо',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        }
                                    ],
                                    features: [{ ftype: 'summary' }]
                                }
                            ]
                        },
                        {
                            xtype: 'chesimportpaymentssummarygrid',
                            title: 'Оплаты (детализация)',
                            store: me.paymentStore
                        },
                        {
                            title: 'Изменение сальдо (детализация)',
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    store: me.saldoStore,
                                    columnLines: true,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'WalletType',
                                            text: '',
                                            width: 150,
                                            renderer: function (val) {
                                                return Ext.String.format(me._textStyle, B4.enums.regop.WalletType.displayRenderer(val) + ':');
                                            },
                                            summaryRenderer: function (val, summaryData) {
                                                return Ext.String.format(me._textStyle, 'Итого:');
                                            }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Change',
                                            text: 'Изменение сальдо',
                                            width: 120,
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        }
                                    ],
                                    features: [{ ftype: 'summary' }]
                                }
                            ]
                        },
                        {
                            title: 'Перерасчет (детализация)',
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    store: me.recalcStore,
                                    columnLines: true,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'WalletType',
                                            text: '',
                                            width: 150,
                                            renderer: function (val) {
                                                return Ext.String.format(me._textStyle, B4.enums.regop.WalletType.displayRenderer(val) + ':');
                                            },
                                            summaryRenderer: function (val, summaryData) {
                                                return Ext.String.format(me._textStyle, 'Итого:');
                                            }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Recalc',
                                            text: 'Перерасчет',
                                            renderer: decimalRenderer,
                                            summaryType: 'sum',
                                            summaryRenderer: summaryRenderer
                                        }
                                    ],
                                    features: [{ ftype: 'summary' }]
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Экспорт',
                                    action: 'Export',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }//,
                                //{
                                //    xtype: 'button',
                                //    text: 'Валидация',
                                //    iconCls: 'icon-accept',
                                //    disabled: true
                                //}
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});