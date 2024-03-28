Ext.define('B4.view.disposal.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.disposaleditpanel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'disposalEditPanel',
    title: '',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.enums.ReasonErpChecking',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.NotificationType',
        'B4.enums.InspectionKind',
        'B4.enums.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.store.dict.Inspector',
        'B4.store.disposal.ProsecutorOfficeForSelect',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.view.disposal.TypeSurveyGrid',
        'B4.view.disposal.AnnexGrid',
        'B4.view.disposal.ExpertGrid',
        'B4.view.disposal.ProvidedDocGrid',
        'B4.view.disposal.SubjectVerificationGrid',
        'B4.view.disposal.SurveyPurposeGrid',
        'B4.view.disposal.SurveyObjectiveGrid',
        'B4.view.disposal.InspFoundationCheckGrid',
        'B4.view.disposal.controlobjectinfo.Grid',
        'B4.view.disposal.inspectionbase.Grid',
        'B4.view.disposal.KnmReasonGrid',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.store.dict.ControlType',
        'B4.view.controllist.Grid',
        'B4.form.FiasSelectAddress'
    ],

    store: 'dict.InspectionBaseType',

    initComponent: function () {
        var me = this;

        me.title = B4.DisposalTextValues.getSubjectiveCase();

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        xtype: 'container',
                        padding: 5,
                        border: false
                    },
                    items: [
                        {
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    width: 140,
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'timefield',
                                    labelWidth: 50,
                                    width: 112,
                                    name: 'DocumentTime',
                                    fieldLabel: 'Время',
                                    format: 'H:i',
                                    minValue: '08:00',
                                    maxValue: '22:00',
                                    value: '08:00',
                                    increment: 15
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'SendToErp',
                                    fieldLabel: 'Отправлено в ЕРП',
                                    labelWidth: 140,
                                    enumName: 'B4.enums.YesNo',
                                    readOnly: true,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'SendToErknm',
                                    fieldLabel: 'Отправлено в ЕРКНМ',
                                    labelWidth: 140,
                                    enumName: 'B4.enums.YesNo',
                                    readOnly: true,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ReasonErpChecking',
                                    fieldLabel: 'Основание для включения проверки в ЕРП',
                                    editable: false,
                                    enumName: 'B4.enums.ReasonErpChecking',
                                    labelWidth: 250,
                                    hidden: true
                                }
                            ]
                        },
                        {
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    width: 253,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 42,
                                    hideTrigger: true
                                },
                                {
                                    width: 295,
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'disposalTabPanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            autoScroll: true,
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: 8,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: .7,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 210,
                                                labelAlign: 'right',
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalBaseName',
                                                    itemId: 'tfBaseName',
                                                    fieldLabel: 'Основание обследования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalPlanName',
                                                    itemId: 'tfPlanName',
                                                    fieldLabel: 'Документ основания'
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'KindCheck',
                                                    fieldLabel: 'Вид проверки',
                                                    displayField: 'Name',
                                                    url: '/KindCheckGji/List',
                                                    valueField: 'Id',
                                                    itemId: 'cbTypeCheck',
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'ControlType',
                                                    itemId: 'sfControlType',
                                                    fieldLabel: 'Вид контроля',
                                                    store: 'B4.store.dict.ControlType',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    labelAlign: 'right',
                                                    readOnly: false,
                                                    allowBlank: false,
                                                    flex: 1,
                                                    columns: [
                                                        {
                                                            xtype: 'gridcolumn',
                                                            header: 'Наименование вида контроля (надзора)',
                                                            dataIndex: 'Name',
                                                            flex: 1,
                                                            filter: { xtype: 'textfield' }
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ErpRegistrationNumber',
                                                    fieldLabel: 'Учетный номер проверки в ЕРП',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ErpId',
                                                    fieldLabel: 'Идентификатор в ЕРП',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ErpRegistrationDate',
                                                    fieldLabel: 'Дата присвоения учетного номера / идентификатора ЕРП',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ErknmRegistrationNumber',
                                                    fieldLabel: 'Учетный номер решения в ЕРКНМ',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ErknmId',
                                                    fieldLabel: 'Идентификатор в ЕРКНМ',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ErknmRegistrationDate',
                                                    fieldLabel: 'Дата присвоения учетного номера / идентификатора ЕРКНМ',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'TorId',
                                                    fieldLabel: 'Идентификатор ТОР',
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'b4fiasselectaddress',
                                                    name: 'DecisionPlace',
                                                    fieldLabel: 'Место вынесения решения',
                                                    readOnly: false,
                                                    hidden: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'DateStart',
                                                            itemId: 'dfDateStart',
                                                            fieldLabel: 'Период обследования с',
                                                            labelWidth: 210,
                                                            flex: 0.65
                                                        },
                                                        {
                                                            name: 'DateEnd',
                                                            itemId: 'dfDateEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 150,
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'gkhintfield',
                                                        labelAlign: 'right',
                                                        regex: /^[0-9]+$/,
                                                        regexText: 'Значение не может быть меньше 0'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'CountDays',
                                                            fieldLabel: 'Срок проверки (количество дней)',
                                                            labelWidth: 210,
                                                            flex: 0.65
                                                        },
                                                        {
                                                            name: 'CountHours',
                                                            fieldLabel: 'Срок проверки (количество часов)',
                                                            labelWidth: 150,
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'ObjectVisitStart',
                                                            itemId: 'dfObjectVisitStart',
                                                            fieldLabel: 'Выезд на объект с',
                                                            labelWidth: 210,
                                                            flex: 0.65
                                                        },
                                                        {
                                                            name: 'ObjectVisitEnd',
                                                            itemId: 'dfObjectVisitEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 150,
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'timefield',
                                                        labelAlign: 'right',
                                                        submitFormat: 'H:i:s',
                                                        minValue: '8:00',
                                                        maxValue: '22:00'
                                                    },
                                                    items: [
                                                        {
                                                            fieldLabel: 'Время с',
                                                            name: 'TimeVisitStart',
                                                            labelWidth: 210,
                                                            format: 'H:i',
                                                            flex: 0.65
                                                        },
                                                        {
                                                            fieldLabel: 'по',
                                                            name: 'TimeVisitEnd',
                                                            labelWidth: 150,
                                                            format: 'H:i',
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    items: [
                                                        {
                                                            xtype: 'checkbox',
                                                            boxLabel: 'Выезд инспектора в командировку',
                                                            name: 'OutInspector',
                                                            itemId: 'cbOutInspector',
                                                            padding: '0 0 0 215'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            name: 'InspectionBase',
                                                            fieldLabel: 'Основание проверки',
                                                            store: me.store,
                                                            textProperty: 'Name',
                                                            editable: false,
                                                            labelWidth: 210,
                                                            labelAlign: 'right',
                                                            flex: 1,
                                                            columns: [
                                                                {
                                                                    xtype: 'gridcolumn',
                                                                    header: 'Код',
                                                                    dataIndex: 'Code',
                                                                    flex: 0.3,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                                {
                                                                    xtype: 'gridcolumn',
                                                                    header: 'Наименование',
                                                                    dataIndex: 'Name',
                                                                    flex: 1.3,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                                {
                                                                    xtype: 'b4enumcolumn',
                                                                    header: 'Вид проверки',
                                                                    dataIndex: 'InspectionKindId',
                                                                    enumName: 'B4.enums.InspectionKind',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                                {
                                                                    xtype: 'gridcolumn',
                                                                    header: 'Значение передается в ЕРП',
                                                                    dataIndex: 'SendErp',
                                                                    renderer: function (val) {
                                                                        return val ? 'Да' : 'Нет';
                                                                    },
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'label',
                                                            width: 210,
                                                            align: 'right',
                                                            text: 'Срок взаимодействия с контролируемым лицом не более:',
                                                            padding: '0 0 0 20'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            padding: '10 5 0 5',
                                                            width: 60,
                                                            editable: false,
                                                            floating: false,
                                                            labelAlign: 'right',
                                                            name: 'InteractionPersonHour',
                                                            displayField: 'Display',
                                                            store: Ext.create('Ext.data.Store', {
                                                                fields: ['Value', 'Display'],
                                                                data: me.numbers(24)
                                                            }),
                                                            value: null,
                                                            valueField: 'Value'
                                                        },
                                                        {
                                                            xtype: 'label',
                                                            text: 'час.',
                                                            padding: '15 0'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            padding: '10 5 0 10',
                                                            width: 50,
                                                            editable: false,
                                                            floating: false,
                                                            labelAlign: 'right',
                                                            name: 'InteractionPersonMinutes',
                                                            displayField: 'Display',
                                                            store: Ext.create('Ext.data.Store', {
                                                                fields: ['Value', 'Display'],
                                                                data: me.numbers(59)
                                                            }),
                                                            value: null,
                                                            valueField: 'Value'
                                                        },
                                                        {
                                                            xtype: 'label',
                                                            text: 'мин.',
                                                            padding: '15 0'
                                                        }
                                                    ],
                                                },
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'UsingMeansRemoteInteraction',
                                                    fieldLabel: 'Использование средств дистанционного взаимодействия с контролируемым лицом',
                                                    labelWidth: 190,
                                                    padding: '0 0 0 20',
                                                    enumName: 'B4.enums.YesNoNotSet',
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 190,
                                                    padding: '0 0 0 20',
                                                    name: 'InfoUsingMeansRemoteInteraction',
                                                    fieldLabel: 'Сведения об использовании средств дистанционного взаимодействия',
                                                    hidden: true
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'PlannedActions',
                                    itemId: 'tfPlannedActions',
                                    fieldLabel: 'Запланированные действия',
                                    labelAlign: 'right',
                                    flex: 1,
                                    editable: false,
                                    labelWidth: 190,
                                    padding: '0 0 20 20',
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Приостановление проведения проверки',
                                    name: 'SuspensionInspection',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'SuspensionInspectionBase',
                                            anchor: '100%',
                                            labelWidth: 190,
                                            labelAlign: 'left',
                                            fieldLabel: 'Основание для приостановления проведения проверки'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 290,
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'SuspensionDateFrom',
                                                            fieldLabel: 'Период приостановки с:',
                                                            format: 'd.m.Y'
                                                        },
                                                        {
                                                            xtype: 'timefield',
                                                            name: 'SuspensionTimeFrom',
                                                            fieldLabel: 'Время приостановки с:',
                                                            format: 'H:i',
                                                            minValue: '08:00',
                                                            maxValue: '22:00',
                                                            value: '08:00',
                                                            increment: 15
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        width: 130,
                                                        labelWidth: 30,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'SuspensionDateTo',
                                                            fieldLabel: 'по:',
                                                            format: 'd.m.Y'
                                                        },
                                                        {
                                                            xtype: 'timefield',
                                                            name: 'SuspensionTimeTo',
                                                            fieldLabel: 'по:',
                                                            format: 'H:i',
                                                            minValue: '08:00',
                                                            maxValue: '22:00',
                                                            value: '08:00',
                                                            increment: 15
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 180,
                                        labelAlign: 'right'
                                    },
                                    title: 'Информация о согласовании с прокуратурой',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 180,
                                                        width: 415,
                                                        labelAlign: 'right',
                                                        readOnly: true,
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            readOnly: false,
                                                            labelAlign: 'right',
                                                            name: 'TypeAgreementProsecutor',
                                                            itemId: 'cbTypeAgreementProsecutor',
                                                            fieldLabel: 'Согласование с прокуратурой',
                                                            store: B4.enums.TypeAgreementProsecutor.getStore(),
                                                            flex: 1
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            readOnly: false,
                                                            labelAlign: 'right',
                                                            name: 'TypeAgreementResult',
                                                            itemId: 'cbTypeAgreementResult',
                                                            fieldLabel: 'Результат согласования',
                                                            store: B4.enums.TypeAgreementResult.getStore(),
                                                            flex: 1
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        flex: 1,
                                                        labelWidth: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'DocumentNumberWithResultAgreement',
                                                            itemId: 'cbDocumentNumberWithResultAgreement',
                                                            fieldLabel: 'Номер документа с результатом согласования'
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            padding: '0 0 5 0',
                                                            layout: 'hbox',
                                                            defaults: {
                                                                xtype: 'datefield',
                                                                format: 'd.m.Y',
                                                                labelAlign: 'right',
                                                                allowBlank: false,
                                                                flex: 1,
                                                                labelWidth: 300
                                                            },
                                                            items: [
                                                                {
                                                                    name: 'DocumentDateWithResultAgreement',
                                                                    itemId: 'cbDocumentDateWithResultAgreement',
                                                                    fieldLabel: 'Дата документа с результатом согласования'
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Prosecutor',
                                            fieldLabel: 'Наименование прокуратуры',
                                            store: 'B4.store.disposal.ProsecutorOfficeForSelect',
                                            textProperty: 'Name',
                                            editable: false,
                                            columns: [
                                                {
                                                    xtype: 'gridcolumn',
                                                    header: 'Код',
                                                    dataIndex: 'Code',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    header: 'Наименование',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'InformationAboutHarm',
                                            fieldLabel: 'Сведения о причинении вреда (ущерба) (ст. 66 ФЗ)',
                                            flex: 1,
                                            maxLength: 1000,
                                            hidden: true
                                        },
                                        {
                                            xtype: 'disposalknmreasongrid',
                                            flex: 1,
                                            minHeight: 50,
                                            hidden: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Должностные лица',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            name: 'IssuedDisposal',
                                            fieldLabel: 'ДЛ, вынесшее распоряжение',
                                            columns: [
                                                {
                                                    header: 'ФИО',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Fio',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    header: 'Должность',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Position',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            dockedItems: [
                                                {
                                                    xtype: 'b4pagingtoolbar',
                                                    displayInfo: true,
                                                    store: 'B4.store.dict.Inspector',
                                                    dock: 'bottom'
                                                }
                                            ],
                                            itemId: 'sfIssuredDisposal',
                                            allowBlank: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'ResponsibleExecution',
                                            itemId: 'sflResponsibleExecution',
                                            fieldLabel: 'Ответственный за исполнение',
                                            textProperty: 'Fio',
                                            columns: [
                                                {
                                                    header: 'ФИО',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Fio',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    header: 'Должность',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Position',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            dockedItems: [
                                                {
                                                    xtype: 'b4pagingtoolbar',
                                                    displayInfo: true,
                                                    store: 'B4.store.dict.Inspector',
                                                    dock: 'bottom'
                                                }
                                            ],
                                            editable: false
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'disposalInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Инспекторы',
                                            allowBlank: false,
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Уведомление о проверке',
                                    name: 'noticeFieldset',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'NcDate',
                                                            fieldLabel: 'Дата'
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'NcDateLatter',
                                                            fieldLabel: 'Дата исходящего письма'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            name: 'NcSent',
                                                            fieldLabel: 'Уведомление передано',
                                                            store: B4.enums.YesNo.getStore()
                                                        },
                                                        {
                                                            xtype: 'b4enumcombo',
                                                            name: 'NotificationType',
                                                            fieldLabel: 'Способ уведомления',
                                                            enumName: 'B4.enums.NotificationType'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 0 10',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NcNum',
                                                            fieldLabel: 'Номер документа'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NcNumLatter',
                                                            fieldLabel: 'Номер исходящего письма'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            name: 'NcObtained',
                                                            fieldLabel: 'Уведомление получено',
                                                            store: B4.enums.YesNo.getStore()
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            padding: '0 0 0 17',
                                                            layout: {
                                                                type: 'hbox',
                                                                align: 'stretch'
                                                            },
                                                            defaults: {
                                                                xtype: 'datefield',
                                                                labelWidth: 173,
                                                                labelAlign: 'right',
                                                                format: 'd.m.Y'
                                                            },
                                                            items: [
                                                                {
                                                                    flex: 1,
                                                                    name: 'SubmissionDate',
                                                                    fieldLabel: 'Дата направления требования о предоставлении документов',
                                                                    hidden: true
                                                                },
                                                                {
                                                                    flex: 1,
                                                                    name: 'ReceiptDate',
                                                                    fieldLabel: 'Дата получения документов во исполнение требования',
                                                                    hidden: true
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'disposalsubjectverificationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalsurveypurposegrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalsurveyobjectivegrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalinspfoundationcheckgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposaltypesurveygrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalprovideddocgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalexpertgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalannexgrid',
                            flex: 1
                        },
                        {
                            xtype: 'controllistgrid',
                            name: 'controlListGrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalinspectionbasegrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalcontrolobjectinfogrid',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
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
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton',
                                    itemId: 'btnCreateDisposal'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отправить в ЕРП',
                                    textAlign: 'left',
                                    singleRegistryTitle: 'ЕРП',
                                    controllerName: 'ErpIntegration',
                                    sendMethodName: 'SendToErp',
                                    additionalParams: ['erpId'],
                                    name: 'btnSendTo',
                                    itemId: 'btnSendToErp',
                                    hidden: true
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отправить в ЕРКНМ',
                                    textAlign: 'left',
                                    singleRegistryTitle: 'ЕРКНМ',
                                    controllerName: 'ErknmIntegration',
                                    sendMethodName: 'SendToErknm',
                                    additionalParams: ['erknmId'],
                                    name: 'btnSendTo',
                                    itemId: 'btnSendToErknm',
                                    hidden: true
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отправить в ТОР КНД',
                                    textAlign: 'left',
                                    singleRegistryTitle: 'ТОР КНД',
                                    controllerName: 'TorIntegration',
                                    sendMethodName: 'SendKnmToTor',
                                    name: 'btnSendTo',
                                    itemId: 'btnSendToTor',
                                    hidden: true
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    // Формирует последовательность чисел для выпадающего списка
    numbers: function (limit) {
        var k, mass = [{ "Value": null, "Display": '' }];

        for (i = 0; i <= limit; i++) {

            if (i < 10) { k = '0' + i; } else { k = i; }

            mass.push({ "Value": i, "Display": k });
        }
        return mass;
    }
});