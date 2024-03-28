Ext.define('B4.view.claimwork.restructdebt.Edit', {
    extend: 'Ext.form.Panel',

    alias: 'widget.restructdebteditpanel',

    bodyStyle: Gkh.bodyStyle,
    border: false,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.ux.button.Delete',
        'B4.enums.RestructDebtStatus',
        'B4.enums.RestructDebtDocumentState',
        'B4.store.claimwork.RestructDebt',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.RestructDebt');

        Ext.applyIf(me, {
            store: store,
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    defaults: {
                        xtype: 'panel',
                        bodyStyle: Gkh.bodyStyle,
                        border: false,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                    },
                    items: [
                        {
                            title: 'Договор реструктуризации',
                            name: 'Contract',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Основание реструктуризации',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        padding: '5 8 0 0'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Документ',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1,
                                                allowBlank: false
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    mouseWheelEnabled: false,
                                                    hideTrigger: true,
                                                    name: 'DocumentNum',
                                                    maxValue: 2147483648, //Int32.MaxValue
                                                    fieldLabel: 'Номер'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentDate',
                                                    fieldLabel: 'Дата',
                                                    readOnly: true,
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            fieldLabel: 'Файл документа',
                                            name: 'DocFile'
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            fieldLabel: 'Скан документа',
                                            name: 'PaymentScheduleFile'
                                        },
                                        {
                                            xtype: 'textarea',
                                            fieldLabel: 'Причина',
                                            name: 'Reason',
                                            minHeight: 60,
                                            maxLength: 255
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Сумма задолженности',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        padding: '5 8 0 0',
                                        defaults: {
                                            xtype: 'numberfield',
                                            mouseWheelEnabled: false,
                                            hideTrigger: true,
                                            labelWidth: 200,
                                            labelAlign: 'right',
                                            readOnly: true,
                                            flex: 1
                                        },
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    name: 'BaseTariffDebtSum',
                                                    fieldLabel: 'Сумма задолженности по базовому тарифу'
                                                },
                                                {
                                                    name: 'DecisionTariffDebtSum',
                                                    fieldLabel: 'Сумма задолженности по тарифу решения'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    name: 'DebtSum',
                                                    fieldLabel: 'Сумма задолженности'
                                                },
                                                {
                                                    name: 'PenaltyDebtSum',
                                                    fieldLabel: 'Сумма задолженности по пени'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    name: 'RestructSum',
                                                    fieldLabel: 'Сумма реструктуризации',

                                                },
                                                {
                                                    name: 'PercentSum',
                                                    fieldLabel: 'в т.ч. проценты (руб.)',
                                                    readOnly: false,
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'Status',
                                                    fieldLabel: 'Статус оплат',
                                                    enumName: 'B4.enums.RestructDebtStatus'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                        },
                        {
                            title: 'Расторжение договора',
                            name: 'RevokeContract',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Расторжение договора реструктуризации',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'container',
                                        layout: {
                                            type: 'hbox',
                                            align: 'stretch'
                                        },
                                        defaults: {
                                            labelWidth: 200,
                                            padding: '5 10 0 10',
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'DocumentState',
                                                    fieldLabel: 'Статус договора',
                                                    enumName: 'B4.enums.RestructDebtDocumentState'
                                                },
                                                {
                                                    xtype: 'hiddenfield'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'PaidDebtSum',
                                                    mouseWheelEnabled: false,
                                                    hideTrigger: true,
                                                    maxLength: 13,
                                                    fieldLabel: 'Оплаченная сумма задолженности'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'NotPaidDebtSum',
                                                    maxLength: 13,
                                                    mouseWheelEnabled: false,
                                                    hideTrigger: true,
                                                    fieldLabel: 'Непогашенная сумма задолженности'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'TerminationDate',
                                                    allowBlank: false,
                                                    fieldLabel: 'Дата расторжения',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'TerminationNumber',
                                                    allowBlank: false,
                                                    maxLength: 10,
                                                    mouseWheelEnabled: false,
                                                    hideTrigger: true,
                                                    fieldLabel: 'Номер документа'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    fieldLabel: 'Документ-основание',
                                                    allowBlank: false,
                                                    name: 'TerminationFile'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'textarea',
                                                    fieldLabel: 'Причина',
                                                    name: 'TerminationReason',
                                                    minHeight: 60,
                                                    maxLength: 255
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ],
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    border: false,
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'b4deletebutton'
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