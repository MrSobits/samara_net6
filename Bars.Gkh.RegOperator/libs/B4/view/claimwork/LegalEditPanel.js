Ext.define('B4.view.claimwork.LegalEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.legalclaimworkeditpanel',
    title: 'Общие сведения',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.enums.DebtorState',
        'B4.view.claimwork.AccountDetailGrid',
        'B4.enums.ContragentState',
        'B4.store.Contragent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            flex: 9,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    name: 'AccountOwnerName',
                                    fieldLabel: 'Абонент'
                                },
                                {
                                    name: 'FactAddress',
                                    fieldLabel: 'Фактический адрес'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'OperManagement',
                                    fieldLabel: 'Контрагент оперативного управления',
                                    store: 'B4.store.Contragent',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 2, filter: { xtype: 'textfield' } },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    labelAlign: 'right',
                                    readOnly: false
                                },
                                {
                                    name: 'OperManReason',
                                    fieldLabel: 'Основание оперативного управления',
                                    readOnly: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'OperManDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата начала оперативного управления',
                                    readOnly: false
                                },
                                {
                                    name: 'Inn',
                                    fieldLabel: 'ИНН'
                                },
                                {
                                    name: 'OrganizationForm',
                                    fieldLabel: 'Организацонно-правовая форма'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ContragentState',
                                    fieldLabel: 'Статус контрагента',
                                    enumName: 'B4.enums.ContragentState'
                                },
                                {
                                    name: 'CurrChargeBaseTariffDebt',
                                    fieldLabel: 'Сумма текущей задолженности по базовому тарифу'
                                },
                                {
                                    name: 'CurrChargeDecisionTariffDebt',
                                    fieldLabel: 'Сумма текущей задолженности по тарифу решения'
                                },
                                {
                                    name: 'CurrChargeDebt',
                                    fieldLabel: 'Общая сумма текущей задолженности'
                                },
                                {
                                    name: 'CurrPenaltyDebt',
                                    fieldLabel: 'Общая сумма текущей задолженности по пени'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 10,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 220,
                                labelAlign: 'right',
                                readOnly: true,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    name: 'JuridicalAddress',
                                    fieldLabel: 'Юридический адрес'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.JurInstitutionRo',
                                    textProperty: 'Address',
                                    editable: false,
                                    columns: [
                                        {
                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        {
                                            text: 'Адрес',
                                            dataIndex: 'Address',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    name: 'JurisdictionAddress',
                                    fieldLabel: 'Зона подсудности',
                                    readOnly: false
                                },
                                {
                                    name: 'Kpp',
                                    fieldLabel: 'КПП'
                                },
                                {
                                    name: 'ParentContragentName',
                                    fieldLabel: 'Головная организация'
                                },
                                {
                                    name: 'DateTermination',
                                    fieldLabel: 'Дата прекращения деятельности'
                                },
                                {
                                    name: 'OrigChargeBaseTariffDebt',
                                    fieldLabel: 'Сумма просроченной задолженности по базовому тарифу'
                                },
                                {
                                    name: 'OrigChargeDecisionTariffDebt',
                                    fieldLabel: 'Сумма просроченной задолженности по тарифу решения'
                                },
                                {
                                    name: 'OrigChargeDebt',
                                    fieldLabel: 'Общая сумма просроченной задолженности'
                                },
                                {
                                    name: 'OrigPenaltyDebt',
                                    fieldLabel: 'Общая сумма просроченной задолженности по пени'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'DebtorState',
                    enumName: 'B4.enums.DebtorState',
                    fieldLabel: 'Статус задолженности',
                    readOnly: true,
                    labelAlign: 'right',
                    labelWidth: 200
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
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
                },
                {
                    xtype: 'claimworkaccountdetailgrid',
                    margin: '10 0 0 0',
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить данные',
                                    textAlign: 'left',
                                    actionName: 'updState',
                                    tooltip: 'Обновление сумм и статусов',
                                    iconCls: 'icon-page-refresh'
                                },
                                {
                                    xtype: 'acceptmenubutton'
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