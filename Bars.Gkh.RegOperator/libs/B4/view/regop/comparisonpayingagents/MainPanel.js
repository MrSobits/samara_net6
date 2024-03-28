Ext.define('B4.view.regop.comparisonpayingagents.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.comparisonpayingagentspanel',

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.PaymentOrChargePacketState',
        'B4.enums.SuspenseAccountStatus'
    ],

    title: 'Сопоставление платежных агентов',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },


    initComponent: function () {
        var me = this,
            storeConfig = {
                autoLoad: false,
                fields: [
                    { name: 'DocId' },
                    { name: 'DocPaymentRecordId' },
                    { name: 'EntityId' },
                    { name: 'AccountNum' },
                    { name: 'PaymentDate' },
                    { name: 'Sum' },
                    { name: 'State' }
                ]
            };

        var storeAgents =
            Ext.create('B4.base.Store', Ext.apply({
                autoLoad: true,
                proxy: {
                    type: 'b4proxy',
                    url: B4.Url.action('GetDocsForResolve', 'BankDocumentResolver')
                }
            }, storeConfig));
        var storePayments = Ext.create('B4.base.Store', Ext.apply({
            proxy: {
                type: 'b4proxy',
                url: B4.Url.action('GetSuspects', 'BankDocumentResolver'),
                extraParams: {
                    type: 10
                }
            }
        }, storeConfig));
        var storeSusp = Ext.create('B4.base.Store', Ext.apply({
            proxy: {
                type: 'b4proxy',
                url: B4.Url.action('GetSuspects', 'BankDocumentResolver'),
                extraParams: {
                    type: 20
                }
            }
        }, storeConfig));

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4grid',
                            flex: 1,
                            style: 'border-right: 1px solid #99bce8',
                            title: 'Оплаты платежных агентов',
                            name: 'payagent',
                            selModel: Ext.create('Ext.selection.RowModel', {
                                mode: 'SINGLE',
                                checkOnly: true,
                                ignoreRightMouseSelection: true
                            }),
                            border: false,
                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'AccountNum',
                                    text: 'Лицевой счет',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield',
                                        operand: CondExpr.operands.contains
                                    }
                                },
                                {
                                    xtype: 'numbercolumn',
                                    dataIndex: 'Sum',
                                    text: 'Сумма',
                                    flex: 1,
                                    filter: {
                                        xtype: 'numberfield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'PaymentDate',
                                    text: 'Дата оплаты',
                                    flex: 1,
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield',
                                        operand: CondExpr.operands.eq
                                    }
                                }
                            ],
                            store: storeAgents,
                            dockedItems:[
                                {
                                    xtype: 'b4pagingtoolbar',
                                    dock: 'bottom',
                                    store: storeAgents,
                                    displayInfo: true
                                }
                            ]
                        },
                        {
                            xtype: 'b4grid',
                            name: 'payments',
                            title: 'Неподтвержденные оплаты',
                            flex: 1,
                            selModel: Ext.create('Ext.selection.RowModel', {
                                mode: 'SINGLE',
                                checkOnly: true,
                                ignoreRightMouseSelection: true
                            }),
                            border: false,
                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'AccountNum',
                                    text: 'Cчет',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield',
                                        operand: CondExpr.operands.contains
                                    }
                                },
                                {
                                    xtype: 'numbercolumn',
                                    dataIndex: 'Sum',
                                    text: 'Сумма',
                                    flex: 1,
                                    filter: {
                                        xtype: 'numberfield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'PaymentDate',
                                    text: 'Дата оплаты',
                                    flex: 1,
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'b4enumcolumn',
                                    dataIndex: 'State',
                                    enumName: 'B4.enums.PaymentOrChargePacketState',
                                    text: 'Состояние',
                                    flex: 1,
                                    filter: true
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'b4pagingtoolbar',
                                    dock: 'bottom',
                                    store: storePayments,
                                    displayInfo: true
                                }
                            ],
                            store: storePayments
                        },
                        {
                            xtype: 'b4grid',
                            name: 'suspense',
                            title: 'НВС',
                            hidden: true,
                            flex: 1,
                            selModel: Ext.create('Ext.selection.RowModel', {
                                mode: 'SINGLE',
                                checkOnly: true,
                                ignoreRightMouseSelection: true
                            }),
                            border: false,
                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'AccountNum',
                                    text: 'Cчет',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield',
                                        operand: CondExpr.operands.contains
                                    }
                                },
                                {
                                    xtype: 'numbercolumn',
                                    dataIndex: 'Sum',
                                    text: 'Сумма',
                                    flex: 1,
                                    filter: {
                                        xtype: 'numberfield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'PaymentDate',
                                    text: 'Дата оплаты',
                                    flex: 1,
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'b4enumcolumn',
                                    dataIndex: 'State',
                                    enumName: 'B4.enums.SuspenseAccountStatus',
                                    text: 'Состояние',
                                    flex: 1,
                                    filter: true
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'b4pagingtoolbar',
                                    dock: 'bottom',
                                    store: storeSusp,
                                    displayInfo: true
                                }
                            ],
                            store: storeSusp
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    flex: 1,
                    title: 'Сопоставление данных',
                    name: 'merge',
                    border: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AccountNumAgent',
                            text: 'Номер счета (платежный агент)',
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AccountNumOther',
                            text: 'Номер счета',
                            flex: 1
                        },
                        {
                            xtype: 'b4deletecolumn'
                        }
                    ],
                    store: Ext.create('B4.base.Store', {
                        fields: [
                            { name: 'DocPaymentRecordId' },
                            { name: 'EntityId' },
                            { name: 'AccountNumAgent' },
                            { name: 'AccountNumOther' }
                        ]
                    })
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    name: 'accept',
                                    iconCls: 'icon-accept',
                                    text: 'Сопоставить'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    editable: false,
                                    name: 'paytype',
                                    fieldLabel: 'Тип оплаты',
                                    labelAlign: 'right',
                                    labelWidth: 65,
                                    width: 140,
                                    emptyItem: { Name: '-' },
                                    enumName: 'B4.enums.ImportedPaymentState'
                                },
                                {
                                    xtype: 'button',
                                    disabled: true,
                                    iconCls: 'icon-page-white-magnify',
                                    name: 'autocomp',
                                    text: 'Сопоставить автоматически'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});