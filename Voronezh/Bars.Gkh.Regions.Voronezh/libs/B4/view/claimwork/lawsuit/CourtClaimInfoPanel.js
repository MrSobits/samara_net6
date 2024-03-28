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
        'B4.view.claimwork.DocumentClwAccountDetailGrid',
        'B4.store.dict.Municipality',
        'B4.enums.LawsuitConsiderationType',
        'B4.enums.LawsuitResultConsideration',
		'B4.enums.LawsuitDocumentType',
		'B4.enums.DebtCalcMethod',
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
                            itemId: 'bidNumber',
                            fieldLabel: 'Номер заявления',
                            maxLength: 100,
                            labelAlign: 'right',
                            flex: 1.5
                        },
                        {
                            xtype: 'button',
                            text: 'Сгенерировать номер',
                            name: 'calculateButton',

                            action: 'GenNumLawsuit',
                            iconCls: 'icon-accept',
                            itemId: 'calculateButton',
                            flex: 0.5
                        },
                        {
                            xtype: 'datefield',
                            name: 'BidDate',
                            itemId: 'bidDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата заявления',
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'RedirectDate',
                            itemId: 'redirectDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата перенаправления',
                            flex: 1
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
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '',
                                    name: 'Description',
                                    hidden:true,
                                    flex: 0,
                                    itemId: 'tfDescription'
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
                                    name: 'PayDocNumber',
                                    fieldLabel: 'Номер платежного поручения',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'PayDocDate',
                                    fieldLabel: 'Дата платежного поручения',
                                    format: 'd.m.Y'
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
                        {//Госпошлина
                            xtype: 'checkbox',
                            name: 'DutyPostponement',
                            fieldLabel: 'Отсрочка госпошлины',
                            labelWidth: 170,
                            labelAlign: 'right'
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
                                        'beforeload': function (store, operation) {
                                            operation.params['type'] = 10;
                                        }
                                    },
                                    editable: false
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
                                    xtype: 'textfield',
                                    name: 'JudgeName',
                                    fieldLabel: 'Фио судьи',
                                    maxLength: 100,
                                    alowblank: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NumberCourtBuisness',
                                    fieldLabel: 'Номер дела',
                                    maxLength: 100,
                                    alowblank: true
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
                                    readOnly: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DeterminationDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения',
                                    readOnly: false
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            hidden: true,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'datefield',
                                labelWidth: 220,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'TODO:CCReturnDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения о возвращении заявления о вынесении судебного приказа'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TODO:CCRejectDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения об отказе в принятии заявления о вынесении судебного приказа'
                                }
                            ]
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateJudicalOrder',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата вынесения судебного приказа'
                        },
                        {
                            xtype: 'textareafield',                            
                            maxRows: 4,
                            height: 50,
                            maxLength: 500,
                            grow: true,
                            anchor: '100%',                           
                            flex: 1,
                            name: 'ComentСonsideration',
                            fieldLabel: 'Примечание'
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
                            xtype: 'datefield',
                            name: 'TODO:ConsiderationDate',
                            hidden: true,
                            format: 'd.m.Y',
                            fieldLabel: 'Дата рассмотрения',

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
                                    readOnly: false
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
                                     itemId: 'IsDeterminationReturn',
                                     name: 'IsDeterminationReturn',
                                     fieldLabel: 'Определение о возврате ЗВСП',
                                     labelWidth: 300,
                                     editable: true
                                 },
                                {
                                    xtype: 'datefield',
                                    name: 'DateDeterminationReturn',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения о возврате ЗВСП'
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
                                      itemId: 'IsDeterminationRenouncement',
                                      name: 'IsDeterminationRenouncement',
                                      fieldLabel: 'Определение об отказе принятия ЗВСП',
                                      labelWidth: 300,
                                      editable: true
                                  },
                                 {
                                     xtype: 'datefield',
                                     name: 'DateDeterminationRenouncement',
                                     format: 'd.m.Y',
                                     fieldLabel: 'Дата определения об отказе принятия ЗВСП'
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
                                       itemId: 'IsDeterminationCancel',
                                       name: 'IsDeterminationCancel',
                                       fieldLabel: 'Определение об отмене судебного приказа',
                                       labelWidth: 300,
                                       editable: true
                                   },
                                  {
                                      xtype: 'datefield',
                                      name: 'DateDeterminationCancel',
                                      format: 'd.m.Y',
                                      fieldLabel: 'Дата определения об отмене судебного приказа'
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
                                       itemId: 'IsDeterminationOfTurning',
                                       name: 'IsDeterminationOfTurning',
                                       fieldLabel: 'Определение о повороте СП',
                                       labelWidth: 300,
                                       editable: true
                                   },
                                  {
                                      xtype: 'datefield',
                                      name: 'DateDeterminationOfTurning',
                                      format: 'd.m.Y',
                                      fieldLabel: 'Дата определения о повороте СП'
                                  },
                                  {
                                    xtype: 'numberfield',
                                    name: 'FKRAmountCollected',
                                    fieldLabel: 'Взысканная сумма с ФКР',
                                    hideTrigger: true,
                                    allowDecimals: true
                                  },
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
                                            allowDecimals: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'PenaltyDebtApproved',
                                            fieldLabel: 'Сумма пени (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'DutyDebtApproved',
                                            fieldLabel: 'Сумма пошлины (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            flex: 2
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
                    xtype: 'fieldset',
                    title: 'Отмена судебного приказа',
                    hidden: true,
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
                                    xtype: 'datefield',
                                    name: 'TODO:DecisionDate',
                                    fieldLabel: 'Дата определения',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TODO:DecisionResult',
                                    fieldLabel: 'Результат рассмотрения'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Возврат денежных средств',
                    hidden: true,
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
                            xtype: 'fieldset',
                            title: 'Поворот исполнения судебного приказа',
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
                                            xtype: 'datefield',
                                            name: 'TODO:DecisionDate',
                                            fieldLabel: 'Дата определения',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TODO:DecisionResult',
                                            fieldLabel: 'Результат рассмотрения'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TODO:DecisionResult',
                                            fieldLabel: 'Взысканная сумма с ФКР'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Исполнительное производство',
                    hidden: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Направление судебного приказа в РОСП',
                            name: 'TODO:RoutingCCtoROSP',
                        },
                        {
                            xtype: 'textfield',
                            name: 'TODO:EnforcementNumber',
                            fieldLabel: 'Номер исполнительного производства',
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата',
                            name: 'TODO:EnforcmentDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'TODO:EnforcementCCDate',
                            fieldLabel: 'Дата направления судебного приказа',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Направление судебного приказа судом в РОСП',
                            name: 'TODO:EnforcmentCCRouteROSP'
                        },
                        {
                            xtype: 'textfield',
                            name: 'TODO:EnforcementStatementStartNumber',
                            fieldLabel: 'Постановление о возбуждении исполнительного производства №'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата постановления',
                            name: 'TODO:EnforcmentStatementStartDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            name: 'TODO:EnforcementStatementEndNumber',
                            fieldLabel: 'Постановление об окончании исполнительного производства №'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата постановления',
                            name: 'TODO:EnforcmentStatementEndDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            name: 'TODO:EnforcementCollectedDebtSum',
                            fieldLabel: 'Взысканная сумма долга'
                        },
                        {
                            xtype: 'textfield',
                            name: 'TODO:EnforcementLawsuitToCourtDate',
                            fieldLabel: 'Дата подачи искового заявления в суд'
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
                                    xtype: 'button',
                                    iconCls: 'icon-page',
                                    action: 'PrintExtract',
                                    //href: '/ExtractPrinter/PrintExtractForClaimWork/?id=',
                                    text: 'Выписка'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    disabled: false,
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