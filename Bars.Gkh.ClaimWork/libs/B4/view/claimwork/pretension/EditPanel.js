Ext.define('B4.view.claimwork.pretension.EditPanel', {
    extend: 'Ext.form.Panel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    alias: 'widget.pretensioneditpanel',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.enums.RequirementSatisfaction',
        'B4.form.FileField',
        'B4.ux.button.AcceptMenuButton',
        'B4.view.claimwork.DocumentClwAccountDetailGrid',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function () {
        var me = this,
            labelLeftWidth = 240;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: labelLeftWidth,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'container',
                        flex: 1,
                        padding: '0 0 5 0',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        }
                    },
                    items: [
                        {
                            defaults: {
                                labelWidth: 240,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата формирования'
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 240,
                                        padding: '0 0 5 0'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NumberPretension',
                                            fieldLabel: 'Номер претензии',
                                            maxLength: 100,
                                            itemId: 'numberPretension',
                                            labelAlign: 'right',
                                            flex: 4
                                        }, 
                                        {
                                            xtype: 'button',
                                            text: 'Сгенерировать номер',
                                            name: 'calculateButton',

                                            action: 'GenNumPretension',
                                            iconCls: 'icon-accept',
                                            itemId: 'calculateNunPretensionButton',
                                            flex: 1,
                                            height: 30
                                        }
                                    ]
                                },
                                {
                                    xtype: 'numberfield',
                                    labelAlign: 'right',
                                    hideTrigger: true,
                                    decimalSeparator: ',',
                                    minValue: 0,
                                    name: 'DebtBaseTariffSum',
                                    fieldLabel: 'Сумма задолженности по базовому тарифу (руб.)'
                                },
                                {
                                    xtype: 'numberfield',
                                    labelAlign: 'right',
                                    hideTrigger: true,
                                    decimalSeparator: ',',
                                    minValue: 0,
                                    name: 'Sum',
                                    fieldLabel: 'Сумма претензии (основной долг) (руб.)'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'PaymentPlannedPeriod',
                                    itemId: 'cwPaymentPlannedPeriod',
                                    fieldLabel: 'Планируемый срок оплаты',
                                    format: 'd.m.Y',
                                    labelAlign: 'right',
                                    readOnly: true
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'SendDate',
                                    labelAlign: 'right',
                                    fieldLabel: 'Дата отправки'
                                }

                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 180,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'DateReview',
                                    fieldLabel: 'Дата рассмотрения'
                                },
                                {
                                    xtype: 'numberfield',
                                    labelAlign: 'right',
                                    hideTrigger: true,
                                    decimalSeparator: ',',
                                    minValue: 0,
                                    name: 'DebtDecisionTariffSum',
                                    fieldLabel: 'Сумма задолженности по тарифу решения (руб.)'
                                },
                                {
                                    xtype: 'numberfield',
                                    labelAlign: 'right',
                                    hideTrigger: true,
                                    decimalSeparator: ',',
                                    minValue: 0,
                                    name: 'Penalty',
                                    fieldLabel: 'Пени (руб.)'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'RequirementSatisfaction',
                                    labelAlign: 'right',
                                    fieldLabel: 'Удовлетворение требований',
                                    enumName: 'B4.enums.RequirementSatisfaction'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Файл',
                                    labelAlign: 'right'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'documentclwaccountdetailgrid',
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    text: 'Удалить',
                                    textAlign: 'left'
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