Ext.define('B4.view.agentpir.DebtorEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'agentPIRDebtorEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.form.FileField',
        'B4.store.personal_account.BasePersonalAccount',
        'B4.view.agentpir.ReferenceCalculationGrid',
        'B4.enums.AgentPIRDebtorStatus',
        'B4.enums.DebtCalc',
        'B4.enums.PenaltyCharge',
        'B4.view.agentpir.PaymentGrid',
        'B4.view.agentpir.DocumentGrid',
        'B4.view.agentpir.CreditGrid',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.EnumCombo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    alias: 'widget.debtoreditwindow',
    layout: 'form',
    width: 1200,
    height: 700,
    bodyPadding: 10,
    title: 'Форма редактирования Должника агент ПИР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 0 5 0',
                        labelWidth: 160,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            fieldLabel: 'Номер ЛС',
                            name: 'BasePersonalAccount',
                            itemId: 'AgentPIRDebtorEW',
                            store: 'B4.store.regop.personal_account.BasePersonalAccount',
                            columns: [
                                { text: 'Владелец ЛС', dataIndex: 'AccountOwner', flex: 2, filter: { xtype: 'textfield' } },
                                { text: 'Номер ЛС', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false,
                            textProperty: 'PersonalAccountNum',
                            disabled: true
                        },
                        {
                            xtype: 'b4enumcombo',
                            labelWidth: 60,
                            fieldLabel: 'Статус',
                            enumName: 'B4.enums.AgentPIRDebtorStatus',
                            name: 'Status'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'UseCustomDate',
                            labelWidth: 150,
                            fieldLabel: 'Ограничить период',
                            itemId: 'cbUseCustomDate',
                            editable: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'Fio',
                            fieldLabel: 'Абонент',
                            disabled: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'CustomDate',
                            labelWidth: 160,
                            itemId: 'dfCustomDate',
                            hidden: true,
                            format: 'd.m.Y',
                            fieldLabel: 'Расчет задолженности с'
                        }
                    ]
                },                
                {
                    xtype: 'fieldset',
                    title: 'Задолженность',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                margin: '5 0 5 0',
                                labelWidth: 150,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'DebtBaseTariff',
                                    fieldLabel: 'Базовый тариф',
                                    itemId: 'nfDebtBaseTariff',
                                    hideTrigger: true,
                                    allowDecimals: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'PenaltyDebt',
                                    fieldLabel: 'Пени',
                                    itemId: 'nfPenaltyDebt',
                                    hideTrigger: true,
                                    allowDecimals: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Credit',
                                    fieldLabel: 'Зачтено',
                                    itemId: 'Credit',
                                    disabled: true,
                                    hideTrigger: true,
                                    allowDecimals: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 0.5
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DebtStartDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата возникновения',
                                    itemId: 'sfDebtStartDate'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DebtEndDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата окончания',
                                    itemId: 'sfDebtEndDate'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 5 0',
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
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Задолженность',
                                    enumName: 'B4.enums.DebtCalc',
                                    name: 'DebtCalc',
                                    itemId: 'cbDebtCalc'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Пени',
                                    enumName: 'B4.enums.PenaltyCharge',
                                    name: 'PenaltyCharge',
                                    itemId: 'cbPenaltyCharge'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Расчитать дату начала задолженности',
                                    name: 'calculateButton',
                                    tooltip: 'Расчитать дату начала задолженности',
                                    action: 'DebtStartCalculate',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculateButton',
                                    flex: 0.5
                                }
                            ]
                        }
                    ]
                },
                
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    layout: {
                        align: 'stretch'
                    },
                    enableTabScroll: true,
                    defaults:
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        flex: 1,
                        margins: -1,
                        border: false
                    },
                    items: [            
                        {
                            xtype: 'panel',
                            name: 'docpanel',
                            title: 'Документы',
                            items: [
                                {
                                    xtype: 'agentpirdebtordocumentGrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            name: 'pamentpanel',
                            title: 'Оплаты',
                            items: [
                                {
                                    xtype: 'paymentGrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            name: 'prefcalcpanel',
                            title: 'Расчет задолженности',
                            items: [
                                {
                                    xtype: 'agentpirreferencecalculationgrid',
                                    flex: 1,
                                    height: 400,
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            name: 'creditpanel',
                            title: 'Детали начислений',
                            items: [
                                {
                                    xtype: 'agentpirdebtorcreditgrid',
                                    flex: 1
                                }
                            ]
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-book-go',
                                    name: 'btnGetExtract',
                                    itemId: 'btnGetExtract',
                                    action: 'getExtract',
                                    text: 'Скачать выписку'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
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