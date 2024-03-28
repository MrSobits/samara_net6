Ext.define('B4.view.financialperformance.CommunalServiceEditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 2,
    alias: 'widget.finperfcomserviceeditpanel',
    title: 'Коммунальные услуги',
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save'],

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
                        xtype: 'numberfield',
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 320,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true,
                        minValue: 0
                    },
                    title: 'Общая информация по предоставленным коммунальным услугам',
                    items: [
                        {                        
                            name: 'ComServStartAdvancePay',
                            fieldLabel: 'Авансовые платежи потребителей на начало периода (руб.)'
                        },
                        {
                            name: 'ComServStartCarryOverFunds',
                            fieldLabel: 'Переходящие остатки денежных средств на начало периода (руб.)'
                        },
                        {
                            name: 'ComServStartDebt',
                            fieldLabel: 'Задолженность потребителей на начало периода (руб.)'
                        },
                        {
                            name: 'ComServEndAdvancePay',
                            fieldLabel: 'Авансовые платежи потребителей на конец периода (руб.)'
                        },
                        {
                            name: 'ComServEndCarryOverFunds',
                            fieldLabel: 'Переходящие остатки денежных средств на конец периода (руб.)'
                        },
                        {
                            name: 'ComServEndDebt',
                            fieldLabel: 'Задолженность потребителей на конец периода (руб.)'
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
                        minValue: 0
                    },
                    title: 'Информация о наличии претензий по качеству предоставленных коммунальных услуг',
                    items: [
                        {
                            xtype: 'numberfield',
                            allowDecimals: false,
                            name: 'ComServReceivedPretensionCount',
                            fieldLabel: 'Количество поступивших претензий (ед.)'
                        },
                        {
                            xtype: 'numberfield',
                            allowDecimals: false,
                            name: 'ComServApprovedPretensionCount',
                            fieldLabel: 'Количество удовлетворенных претензий (ед.)'
                        },
                        {
                            xtype: 'numberfield',
                            allowDecimals: false,
                            name: 'ComServNoApprovedPretensionCount',
                            fieldLabel: 'Количество претензий, в удовлетворении которых отказано (ед.)'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'ComServPretensionRecalcSum',
                            fieldLabel: 'Сумма произведенного перерасчета (руб.)'
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

