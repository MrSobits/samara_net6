Ext.define('B4.view.regop.personal_account.action.DistributeFundsForPerformedWorkWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.distributefundsforperformedworkwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.form.FileField',
        'B4.form.MonthPicker',
        'B4.enums.PerformedWorkFundsDistributionType',
        'B4.view.regop.personal_account.action.DistributeFundsForPerformedWorkGrid'
    ],

    modal: true,
    closable: false,
    width: 880,
    minWidth: 880,
    height: 550,
    minHeight: 300,
    title: 'Зачет средств за ранее выполненные работы',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '10 0 10 0',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4monthpicker',
                                    name: 'OperationDate',
                                    fieldLabel: 'Период операции',
                                    format: 'd.m.Y',
                                    labelWidth: 130,
                                    width: 250,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина',
                                    labelWidth: 80,
                                    width: 270
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'Document',
                                    fieldLabel: 'Документ-основание',
                                    labelWidth: 130
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    fieldLabel: 'Тип распределения средств',
                                    store: B4.enums.PerformedWorkFundsDistributionType.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'DistributionType',
                                    value: B4.enums.PerformedWorkFundsDistributionType.Uniform,
                                    labelWidth: 170,
                                    width: 370,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'DistributionSum',
                                    fieldLabel: 'Распределяемая сумма',
                                    hideTrigger: true,
                                    value: 0,
                                    labelWidth: 150,
                                    width: 230,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'button',
                                    action: 'Distribute',
                                    iconCls: 'icon-accept',
                                    text: 'Распределить',
                                    margin: '0 0 1 7'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Balance',
                                    fieldLabel: 'Остаток',
                                    hideTrigger: true,
                                    value: 0,
                                    readOnly: true,
                                    labelWidth: 60,
                                    width: 140
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                xtype: 'checkbox',
                                fieldStyle: 'vertical-align: middle;',
                                margin: '-2 0 0 10',
                                checked: true,
                                flex: 1
                            },
                            items: [
                                {
                                    boxLabel: 'Распределить зачет средств на базовый тариф',
                                    name: 'CheckDistributeForBaseTariff'
                                },
                                {
                                    boxLabel: 'Распределить зачет средств на тариф решения',
                                    name: 'CheckDistributeForDecisionTariff'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'distributefundsforperformedworkgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            action: 'Accept',
                            text: 'Применить',
                            tooltip: 'Применить',
                            iconCls: 'icon-accept'
                        },
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});