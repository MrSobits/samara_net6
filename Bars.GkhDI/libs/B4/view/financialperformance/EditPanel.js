Ext.define('B4.view.financialperformance.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 2,
    alias: 'widget.financialperformanceEditPanel',
    title: 'Общая информация',
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: ['B4.ux.button.Save'],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 320,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true,
                        minValue: 0,
                    },
                    title: 'Общая информация об оказании услуг (выполнении работ) по содержанию и текущему ремонту общего имущества',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'AdvancePayments',
                            fieldLabel: 'Авансовые платежи потребителей на начало периода (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CarryOverFunds',
                            fieldLabel: 'Переходящие остатки денежных средств на начало периода (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Debt',
                            fieldLabel: 'Задолженность потребителей на начало периода (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 320,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true,
                        maxWidth: 540,
                        minValue: 0
                    },
                    title: 'Начислено за услуги (работы) по содержанию и текущему ремонту',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'ChargeForMaintenanceAndRepairsAll',
                            fieldLabel: 'Всего (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ChargeForMaintenanceAndRepairsMaintanance',
                            fieldLabel: 'в т.ч. за содержание дома (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ChargeForMaintenanceAndRepairsRepairs',
                            fieldLabel: 'в т.ч. за текущий ремонт (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ChargeForMaintenanceAndRepairsManagement',
                            fieldLabel: 'в т.ч. услуги управления (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 320,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true,
                        maxWidth: 540,
                        minValue: 0
                    },
                    title: 'Получено денежных средств',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashAll',
                            fieldLabel: 'Всего (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashFromOwners',
                            fieldLabel: 'в т.ч. денежных средств от собственников/нанимателей помещений (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashFromOwnersTargeted',
                            fieldLabel: 'в т.ч. целевых взносов от собственников/нанимателей помещений (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashAsGrant',
                            fieldLabel: 'в т.ч. субсидий (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashFromUsingCommonProperty',
                            fieldLabel: 'в т.ч. денежных средств от использования общего имущества (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ReceivedCashFromOtherTypeOfPayments',
                            fieldLabel: 'в т.ч. прочие поступления (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 320,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true,
                        maxWidth: 540,
                        minValue: 0
                    },
                    title: 'Итоги',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'CashBalanceAll',
                            fieldLabel: 'Всего денежных средств с учетом остатков (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CashBalanceAdvancePayments',
                            fieldLabel: 'Авансовые платежи потребителей на конец периода (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CashBalanceCarryOverFunds',
                            fieldLabel: 'Переходящие остатки денежных средств на конец периода (руб.)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CashBalanceDebt',
                            fieldLabel: 'Задолженность потребителей на конец периода (руб.)'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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

