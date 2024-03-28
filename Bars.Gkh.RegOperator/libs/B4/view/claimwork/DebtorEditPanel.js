Ext.define('B4.view.claimwork.DebtorEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.debtorclaimworkeditpanel',
    title: 'Общие сведения',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'RoomAddress',
                    fieldLabel: 'Aдрес',
                    readOnly: true,
                    labelAlign: 'right',
                    labelWidth: 150
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    name: 'PersAccState',
                                    fieldLabel: 'Статус ЛС'
                                },
                                {
                                    name: 'AccountOwner',
                                    fieldLabel: 'Aбонент'
                                },
                                {
                                    name: 'CountDaysDelay',
                                    fieldLabel: 'Количество дней просрочки'
                                },
                                {
                                    name: 'CurrChargeDebt',
                                    fieldLabel: 'Сумма текущей задолженности'
                                },
                                {
                                    name: 'OrigChargeDebt',
                                    fieldLabel: 'Сумма исходной задолженности'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    name: 'PersonalAccountNum',
                                    fieldLabel: 'Номер ЛС'
                                },
                                {
                                    name: 'OwnerType',
                                    fieldLabel: 'Тип абонента'
                                },
                                {
                                    name: 'CountMonthDelay',
                                    fieldLabel: 'Количество месяцев просрочки'
                                },
                                {
                                    name: 'CurrPenaltyDebt',
                                    fieldLabel: 'Сумма текущей задолженности по пени'
                                },
                                {
                                    name: 'OrigPenaltyDebt',
                                    fieldLabel: 'Сумма исходной задолженности по пени'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'StateName',
                    fieldLabel: 'Статус задолженности',
                    readOnly: true,
                    labelAlign: 'right',
                    labelWidth: 150
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'IsDebtPaid',
                            fieldLabel: 'Задолженность погашена'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DebtPaidDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата погашения'
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    textAlign: 'left',
                                    actionName: 'updState',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                ,
                                {
                                    xtype: 'gkhbuttonprint'
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