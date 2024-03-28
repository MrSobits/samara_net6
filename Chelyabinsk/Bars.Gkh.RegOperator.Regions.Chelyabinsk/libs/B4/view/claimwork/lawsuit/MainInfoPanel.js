Ext.define('B4.view.claimwork.lawsuit.MainInfoPanel', {
    extend: 'Ext.form.Panel',

    closable: false,
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,
    alias: 'widget.clwlawsuitmaininfopanel',
    title: 'Исковое заявление',
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
        'B4.view.Control.GkhButtonPrint',
        'B4.store.dict.JurInstitution',
        'B4.store.dict.PetitionToCourt',
        'B4.view.claimwork.lawsuit.LawsuitOwnerInfoGrid'
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

                            action: 'GenNumPetition',
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
                             //hidden: true,
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
                                {//Госпошлина
                                    xtype: 'checkbox',
                                    name: 'DutyPostponement',
                                    fieldLabel: 'Отсрочка госпошлины',
                                    labelWidth: 170,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'MoneyLess',
                                    fieldLabel: 'Недостаточно денег для оплаты пошлины',
                                    labelWidth: 170,
                                    labelAlign: 'right'
                                },
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
                            hidden: true,
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
                                    name: 'TODO:JudgeName',
                                    fieldLabel: 'ФИО Судьи',
                                    maxLength: 400,
                                    alowblank: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TODO:JDocNumber',
                                    fieldLabel: '№ Дела',
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
                                    name: 'TODO:LawReturnDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения о возвращении искового заявления'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TODO:LawRejectDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения об отказе в принятии искового заявления'
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
                                    name: 'TODO:LawNoMovementDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения об оставлении искового заявления без движения'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TODO:LawAcceptDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата принятия искового заявления'
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
                                    itemId: 'LawsuitDocType',
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
                                    name: 'IsDenied',
                                    fieldLabel: 'Отказано',
                                    labelWidth: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DeniedDate',
                                    format: 'd.m.Y',

                                    fieldLabel: 'Дата отказа'
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
                                    name: 'IsDeniedAdmission',
                                    fieldLabel: 'Отказано в приеме',
                                    labelWidth: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DeniedAdmissionDate',
                                    format: 'd.m.Y',

                                    fieldLabel: 'Дата отказа в приеме'
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
                                    name: 'IsDirectedByJuridiction',
                                    fieldLabel: 'Направлено по подсудности',
                                    labelWidth: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DirectedByJuridictionDate',
                                    format: 'd.m.Y',

                                    fieldLabel: 'Дата направления по подсудности'
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
                                    name: 'IsDeterminationReturn',
                                    fieldLabel: 'Определение о возврате',
                                    labelWidth: 300,
                                    editable: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateDeterminationReturn',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата определения о возврате'
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
                                    name: 'IsMotionless',
                                    fieldLabel: 'Оставлено без движения',
                                    labelWidth: 300,
                                    editable: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateMotionless',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата оставления без движения'
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
                                    name: 'IsErrorFix',
                                    fieldLabel: 'Устранение недостатков',
                                    labelWidth: 300,
                                    editable: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateErrorFix',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата устранения недостатков'
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
                            itemId: 'LawsuitDistanceDecisionCancelComment',
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
                                    name: 'IsLawsuitDistanceDecisionCancel',
                                    fieldLabel: 'Отмена заочного решения',
                                    labelWidth: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateLawsuitDistanceDecisionCancel',
                                    format: 'd.m.Y',

                                    fieldLabel: 'Дата отмены заочного решения'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            itemId: 'LawsuitDistanceDecisionCancel',
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
                                    name: 'LawsuitDistanceDecisionCancelComment',
                                    fieldLabel: 'Комментарий',
                                    maxLength: 100
                                },
                                { xtype: 'tbfill' }
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
                                        labelAlign: 'right'
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
                    hidden: true,
                    title: 'Обжалование',
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
                            title: 'Жалоба',
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
                                            xtype: 'textfield',
                                            fieldLabel: 'Вид жалобы',
                                            name: 'TODO:PetitionType',
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TODO:PetitionDate',
                                            fieldLabel: 'Дата направления жалобы',
                                            format: 'd.m.Y'
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
                                            fieldLabel: 'Адрес направления жалобы',
                                            name: 'TODO:PetitionAddress'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TODO:PetitionReviewDate',
                                            fieldLabel: 'Дата рассмотрения',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
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
                                            fieldLabel: 'Результат рассмотрения',
                                            name: 'TODO:PetitionResult'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TODO:PetitionAddInfo',
                                            fieldLabel: 'Примечание'
                                        }
                                    ]
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
                                    xtype: 'button',
                                    //padding: '5 25 5 25',
                                    name: 'TODO:AddPetitionButton',
                                    iconCls: 'icon-add',
                                    
                                    text: 'Добавить жалобу'
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
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    labelWidth: 250,
                                    flex: 3,
                                    xtype: 'textfield',
                                    fieldLabel: 'Направление исполнительного листа',
                                    name: 'TODO:RoutingLawtoROSP',
                                },
                                {
                                    labelWidth: 50,
                                    xtype: 'textfield',
                                    fieldLabel: 'в РОСП',
                                    name: 'TODO:RoutingLawtoROSPText2',
                                }
                            ]
                        },   
                        
                        {
                            xtype: 'datefield',
                            name: 'TODO:EnforcementCCDate',
                            fieldLabel: 'Дата направления судебного приказа',
                            format: 'd.m.Y'
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
                        }
                    ]
                },
{
    xtype: 'documentclwaccountdetailgrid',
    name: 'MainInfo',
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
                                    xtype: 'acceptmenubutton',
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    disabled: false,
                                    text: 'Удалить',
                                    textAlign: 'left'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Перенести расчет из ЗВСП',
                                    name: 'getCOcalculateButton',
                                    tooltip: 'Перенести расчет из ЗВСП',
                                    action: 'GetDebtStartCalculate',
                                    iconCls: 'icon-accept',
                                    itemId: 'getCOcalculateButton'
                                },
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});