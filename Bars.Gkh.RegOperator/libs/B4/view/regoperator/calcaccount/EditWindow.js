Ext.define('B4.view.regoperator.calcaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.regopcalcacceditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    height: 550,
    width: 900,
    title: 'Расчетный счет',
    maximizable: true,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.creditorg.Grid',
        'B4.store.CreditOrg',
        'B4.ux.grid.column.Enum',
        'B4.form.SelectField',
        'B4.base.Store',
        'B4.store.regoperator.CalcAccOperation',
        'B4.ux.grid.toolbar.Paging'
    ],

    initComponent: function() {
        var me = this,
            debetStore = Ext.create('B4.store.regoperator.CalcAccOperation', {
                isCredit: false
            }),
            creditStore = Ext.create('B4.store.regoperator.CalcAccOperation', {
                isCredit: true
            });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    enableTabScroll: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Основная информация',
                            border: false,
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
                                                    xtype: 'b4savebutton'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'hidden',
                                    name: 'IsSpecial'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ContragentCreditOrg',
                                    margin: '5 7',
                                    fieldLabel: 'Номер',
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    readOnly: false,
                                    editable: false,
                                    allowBlank: false,
                                    textProperty: 'SettlementAccount',
                                    store: Ext.create('B4.base.Store', {
                                        fields: [
                                            { name: 'Id' },
                                            { name: 'Okpo' },
                                            { name: 'Name' },
                                            { name: 'Bik' },
                                            { name: 'CorrAccount' },
                                            { name: 'CrefitOrg' },
                                            { name: 'SettlementAccount' }
                                        ],
                                        proxy: {
                                            type: 'b4proxy',
                                            controllerName: 'ContragentBankCreditOrg',
                                            listAction: 'List'
                                        }
                                    }),
                                    columns: [
                                        { dataIndex: 'SettlementAccount', text: 'Р/с', flex: 1 },
                                        { dataIndex: 'Name', text: 'Наименование', flex: 2 }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '4 7',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right',
                                        readOnly: true,
                                        labelWidth: 150,
                                        xtype: 'textfield'
                                    },
                                    items: [
                                        {
                                            name: 'CorrAcc',
                                            fieldLabel: 'Корр. счет',
                                            margin: '0 15 0 0'
                                        }, {
                                            name: 'Bik',
                                            fieldLabel: 'БИК'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '4 7',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'Кредитная организация',
                                            name: 'Name',
                                            margin: '0 15 0 0',
                                            store: 'B4.store.CreditOrg',
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'OverdraftLimit',
                                            fieldLabel: 'Лимит по овердрафту',
                                            editable: false,
                                            decimalSeparator: ',',
                                            hideTrigger: true,
                                            allowBlank: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '4 7',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        flex: 1,
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            name: 'DateOpen',
                                            fieldLabel: 'Дата открытия',
                                            margin: '0 15 0 0',
                                            allowBlank: false
                                        },
                                        {
                                            name: 'DateClose',
                                            fieldLabel: 'Дата закрытия'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '4 7 6 4',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            flex: 1,
                                            xtype: 'datefield',
                                            labelWidth: 150,
                                            format: 'd.m.Y',
                                            margin: '0 15 0 0',
                                            labelAlign: 'right',
                                            name: 'LastOperationDate',
                                            fieldLabel: 'Дата последней операции',
                                            readOnly: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'gridpanel',
                                            disabled: true,
                                            border: false,
                                            title: 'Дебет',
                                            name: 'DebtGrid',
                                            style: 'border-right: 1px solid #99bce8',
                                            store: debetStore,
                                            flex: 1,
                                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                                            columns: [
                                                {
                                                    xtype: 'datecolumn',
                                                    text: 'Дата',
                                                    dataIndex: 'OperationDate',
                                                    format: 'd.m.Y',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'datefield',
                                                        operand: CondExpr.operands.eq,
                                                        format: 'd.m.Y'
                                                    }
                                                },
                                                {
                                                    text: 'Тип операции',
                                                    dataIndex: 'Reason',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'textfield'
                                                    }
                                                },
                                                {
                                                    text: 'Сумма (руб.)',
                                                    dataIndex: 'Amount',
                                                    xtype: 'numbercolumn',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'numberfield',
                                                        allowDecimals: true,
                                                        hideTrigger: true,
                                                        operand: CondExpr.operands.eq
                                                    }
                                                }
                                            ],
                                            dockedItems: [
                                                {
                                                    xtype: 'pagingtoolbar',
                                                    displayInfo: true,
                                                    store: debetStore,
                                                    dock: 'bottom'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gridpanel',
                                            name: 'CreditGrid',
                                            disabled: true,
                                            border: false,
                                            title: 'Кредит',
                                            flex: 1,
                                            store: creditStore,
                                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                                            columns: [
                                                {
                                                    xtype: 'datecolumn',
                                                    text: 'Дата',
                                                    dataIndex: 'OperationDate',
                                                    format: 'd.m.Y',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'datefield',
                                                        operand: CondExpr.operands.eq,
                                                        format: 'd.m.Y'
                                                    }
                                                },
                                                {
                                                    text: 'Тип операции',
                                                    dataIndex: 'Reason',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'textfield'
                                                    }
                                                },
                                                {
                                                    xtype: 'numbercolumn',
                                                    text: 'Сумма (руб.)',
                                                    dataIndex: 'Amount',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'numberfield',
                                                        allowDecimals: true,
                                                        hideTrigger: true,
                                                        operand: CondExpr.operands.eq
                                                    }
                                                }
                                            ],
                                            dockedItems: [
                                                {
                                                    xtype: 'pagingtoolbar',
                                                    displayInfo: true,
                                                    store: creditStore,
                                                    dock: 'bottom'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'regopservicedhousesgrid',
                            disabled: true
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
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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