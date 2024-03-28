Ext.define('B4.view.claimwork.lawsuit.CourtClaimInfoPanel', {
    extend: 'Ext.form.Panel',

    closable: false,
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,

    alias: 'widget.clwlawsuitcourtclaiminfopanel',
    title: 'Заявление о выдаче судеб. приказа',
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.AcceptMenuButton',
        'B4.enums.LawsuitConsiderationType',
        'B4.enums.LawsuitResultConsideration',
        'B4.enums.DebtCalcMethod',
        'B4.view.claimwork.DocumentClwAccountDetailGrid',
        'B4.store.dict.Municipality',
        'B4.enums.LawsuitConsiderationType',
        'B4.enums.LawsuitResultConsideration',
        'B4.enums.LawsuitDocumentType',
        'B4.view.Control.GkhButtonPrint',
        'B4.store.dict.JurInstitution',
        'B4.store.dict.PetitionToCourt'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'DocumentDate',
                            fieldLabel: 'Дата формирования'
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания работы'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PetitionType',
                    store: 'B4.store.dict.PetitionToCourt',
                    fieldLabel: 'Тип заявления',
                    labelWidth: 170,
                    labelAlign: 'right',
                    textProperty: 'FullName',
                    columns: [
                        { dataIndex: 'FullName', text: 'Наименование', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'BidNumber',
                            fieldLabel: 'Номер заявления',
                            maxLength: 100,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'datefield',
                            name: 'BidDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата заявления'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelWidth: 170,
                    labelAlign: 'right'
                },
                {
                    xtype: 'fieldset',
                    title: 'Виды взысканий',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
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
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 0.5
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DebtStartDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Расчетная дата начала задолженности',
                                    itemId: 'sfDebtStartDate'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DebtEndDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Расчетная дата конца задолженности',
                                    itemId: 'sfDebtEndDate'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Расчитать дату начала задолженности',
                                    name: 'calculateButton',
                                    tooltip: 'Расчитать дату начала задолженности',
                                    action: 'DebtStartCalculate',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculateButton'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Способ расчета',
                                    enumName: 'B4.enums.DebtCalcMethod',
                                    name: 'DebtCalcMethod',
                                    itemId: 'sfDebtCalcMethod'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'datefield',
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsLimitationOfActions',
                                    fieldLabel: 'Применить срок исковой давности',
                                    labelWidth: 300,
                                    editable: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateLimitationOfActions',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата применения срока исковой давности'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'DebtBaseTariffSum',
                                    itemId: 'fDebtBaseTariffSum',
                                    fieldLabel: 'Сумма задолженности по базовому тарифу (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'DebtDecisionTariffSum',
                                    itemId: 'fDebtDecisionTariffSum',
                                    fieldLabel: 'Сумма задолженности по тарифу решения (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'DebtSum',
                                    itemId: 'fDebtSum',
                                    fieldLabel: 'Сумма долга (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'PenaltyDebt',
                                    itemId: 'fPenaltyDebt',
                                    fieldLabel: 'Сумма пени (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'Duty',
                                    fieldLabel: 'Гос. пошлина (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Costs',
                                    fieldLabel: 'Судебные издержки (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Рассмотрение',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
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
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Кем рассмотрено',
                                    enumName: 'B4.enums.LawsuitConsiderationType',
                                    name: 'WhoConsidered'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.JurInstitution',
                                    name: 'JurInstitution',
                                    fieldLabel: 'Судебный участок',
                                    labelAlign: 'right',
                                    textProperty: 'ShortName',
                                    columns: [
                                        { dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1, filter: { xtype: 'textfield' } },
                                        { dataIndex: 'ShortName', text: 'Краткое наименование', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    listeners: {
                                        'beforeload': function(store, operation) {
                                            operation.params['type'] = 10;
                                        }
                                    },
                                    editable: false
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'JuridicalSectorMu',
                            fieldLabel: 'Место нахождения',
                            store: 'B4.store.dict.Municipality',
                            editable: false
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'datefield',
                                labelWidth: 170,
                                labelAlign: 'right',
                                format: 'd.m.Y',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'DateOfAdoption',
                                    fieldLabel: 'Дата принятия заявления'
                                },
                                {
                                    name: 'DateOfRewiew',
                                    fieldLabel: 'Дата рассмотрения заявления'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'Suspended',
                                    fieldLabel: 'Приостановлено'
                                },
                                {
                                    xtype: 'component'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'datefield',
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DeterminationNumber',
                                    fieldLabel: 'Номер определения',
                                    maxLength: 100,
                                    readOnly: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DeterminationDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения',
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Результат рассмотрения',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
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
                                xtype: 'datefield',
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Результат рассмотрения',
                                    enumName: 'B4.enums.LawsuitResultConsideration',
                                    name: 'ResultConsideration',
                                    value: 10
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Документ',
                                    enumName: 'B4.enums.LawsuitDocumentType',
                                    name: 'LawsuitDocType',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'datefield',
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ConsiderationNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ConsiderationDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата документа'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Сумма признанной задолженности',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
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
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'DebtSumApproved',
                                            fieldLabel: 'Сумма долга (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'PenaltyDebtApproved',
                                            fieldLabel: 'Сумма пени (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Возражение должника',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
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
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Поступило возражение',
                                    enumName: 'B4.enums.YesNo',
                                    name: 'ObjectionArrived',
                                    value: 10 /*YesNo.Yes*/
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ClaimDate',
                                    fieldLabel: 'Дата заявления',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'Document',
                            fieldLabel: 'Документ',
                            labelWidth: 170,
                            labelAlign: 'right'
                        }
                    ]
                },
                {
                    xtype: 'documentclwaccountdetailgrid',
                    name: 'CourtClaimInfo',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    columns: 3,
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'acceptmenubutton'
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