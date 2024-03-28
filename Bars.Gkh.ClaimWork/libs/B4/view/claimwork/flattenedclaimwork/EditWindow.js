Ext.define('B4.view.claimwork.flattenedclaimwork.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel',
        'B4.ux.button.ChangeValue',
        'B4.enums.LawsuitResultConsideration',
        'B4.form.EnumCombo',
        'B4.form.SelectField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'start' },
    width: 800,
    minWidth: 500,
    minHeight: 500,
    height: '100%',
    bodyPadding: 5,
    itemId: 'flattenedclaimworkEditWindow',
    title: 'Просмотр',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    autoScroll: true,
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items:
            [
                {
                    xtype: 'container',
                    layout: 'vbox',
                    align: 'start',
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Id',
                            fieldLabel: 'Идентификатор',
                            hidden: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'Num',
                            fieldLabel: 'Номер ЗВСП'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Общие сведения',
                    layout: {
                        type: 'vbox',
                        align: 'start'
                    },
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DebtorFullname',
                            fieldLabel: 'Должник'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorRoomAddress',
                            fieldLabel: 'Адрес МКД'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorRoomType',
                            fieldLabel: 'Тип помещения'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorRoomNumber',
                            fieldLabel: 'Номер пом.'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Share',
                            fieldLabel: 'Доля собственности'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorDebtPeriod',
                            fieldLabel: 'Период задолженности'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'DebtorDebtAmount',
                            fieldLabel: 'Сумма долга'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'DebtorDutyAmount',
                            fieldLabel: 'Размер пошлины (руб.)'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DebtorDebtPaymentDate',
                            fieldLabel: 'Дата оплаты'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorDutyPaymentAssignment',
                            fieldLabel: '№ платежного поручения'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DebtorClaimDeliveryDate',
                            fieldLabel: 'Дата подачи заявления в суд'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorJurInstType',
                            fieldLabel: 'Тип суда'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DebtorJurInstName',
                            fieldLabel: 'Наименование суда'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'ЗВСП',
                    layout: {
                        type: 'vbox',
                        align: 'start'
                    },
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimNum',
                            fieldLabel: '№ дела'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimDate',
                            fieldLabel: 'Дата вынесения'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimConsiderationResult',
                            fieldLabel: 'Результат рассмотрения.:'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimCancellationDate',
                            fieldLabel: 'Определение об отмене СП (дата)'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimRospName',
                            fieldLabel: 'Наименование РОСП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimRospDate',
                            fieldLabel: 'Дата направления в РОСП'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimEnforcementProcNum',
                            fieldLabel: '№ Постановления о возбуждении  ИП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimEnforcementProcDate',
                            fieldLabel: 'Дата (ИП)'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimPaymentAssignmentNum',
                            fieldLabel: '№ платежного поручения'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimPaymentAssignmentDate',
                            fieldLabel: 'Дата (ПП)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CourtClaimRospDebtExact',
                            fieldLabel: 'Сумма основного долга'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CourtClaimRospDutyExact',
                            fieldLabel: 'Размер пошлины'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CourtClaimEnforcementProcActEndNum',
                            fieldLabel: '№ Постановления об окончании ИП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'CourtClaimDeterminationTurnDate',
                            fieldLabel: 'Определение о повороте исполнения СП (дата)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Иск к ФКР',
                    layout: {
                        type: 'vbox',
                        align: 'start'
                    },
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'FkrRospName',
                            fieldLabel: 'Наименование РОСП'
                        },
                        {
                            xtype: 'textfield',
                            name: 'FkrEnforcementProcDecisionNum',
                            fieldLabel: '№ Постановления о возбуждении  ИП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'FkrEnforcementProcDate',
                            fieldLabel: 'Дата (ИП)'
                        },
                        {
                            xtype: 'textfield',
                            name: 'FkrPaymentAssignementNum',
                            fieldLabel: '№ платежного поручения'
                        },
                        {
                            xtype: 'datefield',
                            name: 'FkrPaymentAssignmentDate',
                            fieldLabel: 'Дата (ПП)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'FkrDebtExact',
                            fieldLabel: 'Сумма основного долга'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'FkrDutyExact',
                            fieldLabel: 'Размер пошлины'
                        },
                        {
                            xtype: 'textfield',
                            name: 'FkrEnforcementProcActEndNum',
                            fieldLabel: '№ Постановления об окончании ИП'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Иск',
                    layout: {
                        type: 'vbox',
                        align: 'start'
                    },
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items:
                    [
                        {
                            xtype: 'datefield',
                            name: 'LawsuitCourtDeliveryDate',
                            fieldLabel: 'Дата подачи иска в суд'
                        },
                        {
                            xtype: 'textfield',
                            name: 'LawsuitDocNum',
                            fieldLabel: '№ дела'
                        },
                        {
                            xtype: 'datefield',
                            name: 'LawsuitConsiderationDate',
                            fieldLabel: 'Дата рассмотрения'
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'LawsuitResultConsideration',
                            enumName: 'B4.enums.LawsuitResultConsideration',
                            fieldLabel: 'Результат рассмотрения:',
                            value: 30 //"Не задано"
                        },
                        {
                            xtype: 'numberfield',
                            name: 'LawsuitDebtExact',
                            fieldLabel: 'Сумма основного долга'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'LawsuitDutyExact',
                            fieldLabel: 'Размер пошлины'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Исполнительный лист',
                    layout: {
                        type: 'vbox',
                        align: 'start'
                    },
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items:
                    [
                        {
                            xtype: 'textfield',
                            name: 'ListListNum',
                            fieldLabel: 'Серия №'
                        },
                        {
                            xtype: 'datefield',
                            name: 'ListListRopsDate',
                            fieldLabel: 'Дата направления в РОСП'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ListRospName',
                            fieldLabel: 'Наименование РОСП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'ListRospDate',
                            fieldLabel: 'Дата направления в РОСП'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ListEnfProcDecisionNum',
                            fieldLabel: '№ Постановления о возбуждении  ИП'
                        },
                        {
                            xtype: 'datefield',
                            name: 'ListEnfProcDate',
                            fieldLabel: 'Дата (ИП)'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ListPaymentAssignmentNum',
                            fieldLabel: '№ платежного поручения'
                        },
                        {
                            xtype: 'datefield',
                            name: 'ListPaymentAssignmentDate',
                            fieldLabel: 'Дата (ПП)'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ListRospDebtExacted',
                            fieldLabel: 'Сумма основного долга'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'ListRospDutyExacted',
                            fieldLabel: 'Размер пошлины'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ListEnfProcActEndNum',
                            fieldLabel: '№ Постановления об окончании ИП'
                        }
                    ]
                    },
                {
                    xtype: 'container',
                    layout: 'vbox',
                    align: 'start',
                    defaults: {
                        width: 700,
                        height: 24,
                        labelWidth: 200,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Note',
                            fieldLabel: 'Примечание'
                        }
                    ]
                },

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
                                    xtype: 'b4savebutton',
                                    disabled: true
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