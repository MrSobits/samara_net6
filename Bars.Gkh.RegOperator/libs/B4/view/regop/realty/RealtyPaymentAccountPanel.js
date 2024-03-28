Ext.define('B4.view.regop.realty.RealtyPaymentAccountPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.regop.realty.RealtyPaymentAccountTransferGrid'
    ],

    alias: 'widget.realtypaymentaccpanel',
    closable: true,
    title: 'Счет оплат',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    docked: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        me.fireEvent('updateme', me);
                                    }
                                },
                            //Временно убираем кнопку, так как создаются начисления ToDo: выяснить почему создаются начисления
                                {
                                    xtype: 'b4updatebutton',
                                    text: 'Обновить баланс',
                                    handler: function () {
                                        me.fireEvent('updatebalance', me);
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'form',
                    border: false,
                    padding: '0 0 5 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '10 0 0 0',
                            defaults: {
                                labelWidth: 150,
                                width: 250,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Номер счета',
                                    name: 'AccountNum',
                                    itemId: 'tfAccountNum',
                                    labelWidth: 140,
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата открытия счета',
                                    name: 'DateOpen',
                                    itemId: 'dfDateOpen',
                                    width: 230,
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата закрытия счета',
                                    name: 'DateClose',
                                    itemId: 'dfDateClose',
                                    editable: false,
                                    labelWidth: 160
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Заблокировано средств',
                                    width: 230,
                                    name: 'MoneyLocked'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '7 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                width: 250,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Итого по дебету (руб.)',
                                    labelWidth: 140,
                                    name: 'DebtTotal'
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Займы',
                                    width: 230,
                                    name: 'Loan'
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Итого по кредиту (руб.)',
                                    name: 'CreditTotal',
                                    labelWidth: 160
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Сальдо',
                                    name: 'CurrentBalance',
                                    labelWidth: 100,
                                    width: 180
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                width: 250,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Лимит по овердрафту',
                                    itemId: 'tfLimit',
                                    labelWidth: 140,
                                    name: 'OverdraftLimit',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '№ р/с',
                                    width: 350,
                                    name: 'BankAccountNum',
                                    itemId: 'tfBankAccountNum'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата последней операции',
                                    name: 'LastOperationDate',
                                    labelWidth: 160
                                }
                            ]
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
                    padding: '5 5 5 5',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Дебет',
                            layout: 'fit',
                            margin: '0 2 0 0',
                            flex: 1,
                            items: {
                                header: false,
                                xtype: 'realtypayatranfsergrid',
                                type: 'debet'
                            }
                        },
                        {
                            xtype: 'splitter'
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Кредит',
                            layout: 'fit',
                            flex: 1,
                            margin: '0 0 0 2',
                            items: {
                                header: false,
                                xtype: 'realtypayatranfsergrid',
                                type: 'credit'
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});