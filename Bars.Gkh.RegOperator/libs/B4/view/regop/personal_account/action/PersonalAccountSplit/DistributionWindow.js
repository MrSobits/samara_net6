Ext.define('B4.view.regop.personal_account.action.PersonalAccountSplit.DistributionWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.splitdistributionwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Add',
        'B4.view.Control.GkhDecimalField',
        'B4.view.regop.personal_account.action.PersonalAccountSplit.DistributionAccountGrid'
    ],

    modal: true,
    closable: true,
    maximizable: true,
    width: 1000,
    minWidth: 950,
    height: 550,
    minHeight: 350,
    title: 'Распределение задолженности',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    prevWindow: null,
    store: null,

    accountOperationCode: 'PersonalAccountSplitOperation',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    bodyPadding: 10,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        padding: '0 0 10 0',
                        defaults: {
                            labelAlign: 'right'
                        }
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'SplitDate',
                                    fieldLabel: 'Дата разделения',
                                    labelWidth: 100,
                                    readOnly: true,
                                    width: 180
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'DebtSum',
                                    labelWidth: 230,
                                    fieldLabel: 'Распределяемая сумма задолженности',
                                    readOnly: true,
                                    maxWidth: 350,
                                    flex: 0.9
                                },
                                {
                                    xtype: 'button',
                                    action: 'GetDebtSums',
                                    text: 'Рассчитать долг',
                                    margin: '0 0 0 5px'
                                },
                                {
                                    xtype: 'combobox',
                                    fieldLabel: 'Тип распределения средств',
                                    store: B4.enums.regop.SplitAccountDistributionType.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'DistributionType',
                                    editable: false,
                                    value: B4.enums.regop.SplitAccountDistributionType.Manual,
                                    margin: '0 2px 0 0',
                                    labelWidth: 170,
                                    maxWidth: 400,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Оставить оплаты на старом ЛС',
                                    name: 'SavePayments'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'splitdistributionaccountgrid',
                    store: me.store,
                    flex: 1
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
                                    xtype: 'b4savebutton',
                                    text: 'Продолжить'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-arrow-left',
                                    text: 'Назад',
                                    listeners: {
                                        click: function(btn) {
                                            var prevWin = me.prevWindow,
                                                win = btn.up('window');

                                            win.clearListeners();
                                            win.close();

                                            prevWin.show();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],

            getForm: function() {
                return me.down('form');
            },
            getGrid: function() {
                return me.down('splitdistributionaccountgrid');
            },
            getStore: function() {
                return me.getGrid().getStore();
            }
        });

        me.callParent(arguments);
    },
    listeners: {
        beforeclose: function(win) {
            Ext.Msg.confirm('Внимание',
                'При закрытие окна все внесенные изменение в операции "Распределение задолженности" не сохранятся. Действительно ли хотите закрыть окно?',
                function(result) {
                    if (result === 'yes') {
                        win.clearListeners();  // чистим все слушателей, чтобы опять сюда не попасть
                        win.prevWindow.destroy(); // и уничтожаем предыдущее окно
                        win.close();
                    }
                });

            return false;
        }
    }
});