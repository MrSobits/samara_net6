Ext.define('B4.view.appealcits.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1100,
    minWidth: 800,
    height: 600,
    resizable: true,
    bodyPadding: 3,
    itemId: 'appealCitsEditWindow',
    title: 'Обращение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.appealcits.RealityObjectGrid',
        'B4.view.appealcits.StatSubjectGrid',
        'B4.view.appealcits.SourceGrid',
        'B4.view.appealcits.RequestGrid',
        'B4.view.appealcits.AnswerGrid',
        'B4.view.appealcits.EdoPanel',
        'B4.view.appealcits.DecisionGrid',
        'B4.view.appealcits.BaseStatementGrid',
        'B4.view.appealcits.DefinitionGrid',
        'B4.view.appealcits.DefinitionEditWindow',
        'B4.view.Control.GkhIntField',
        'B4.view.appealcits.AppealCitsAdmonitionGrid',
        'B4.view.appealcits.AppealCitsEmergencyHouseGrid',
        'B4.store.dict.ZonalInspection',
        'B4.store.dict.Inspector',
        'B4.store.dict.ResolveGji',
        'B4.store.dict.Citizenship',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.enums.TypeCorrespondent',
        'B4.enums.Gender',
        'B4.enums.AppealStatus',
        'B4.GjiTextValuesOverride',
        'B4.view.appealcits.RelatedAppealCitsGrid',
        'B4.view.appealcits.AppealCitsCategoryGrid',
        'B4.view.appealcits.AppealCitsAttachmentGrid',
        'B4.view.appealcits.AppealCitsHeadInspectorGrid',
        'B4.enums.QuestionStatus',
        'B4.store.dict.SSTUTransferOrg'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'combobox',
                    name: 'QuestionStatus',
                    itemId: 'sfQuestionStatus',
                    labelWidth: 150,
                    labelAlign: 'right',
                    fieldLabel: 'Статус вопроса в ССТУ',
                    displayField: 'Display',
                    store: B4.enums.QuestionStatus.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'SSTUTransferOrg',
                    itemId: 'sfSSTUTransferOrg',
                    textProperty: 'Name',
                    labelWidth: 150,
                    labelAlign: 'right',
                    fieldLabel: 'Кому перенаправлено',
                    store: 'B4.store.dict.SSTUTransferOrg',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'panel',
                    layout: 'form',
                    split: false,
                    collapsible: false,
                    bodyStyle: Gkh.bodyStyle,
                    margins: '0 0 5px 0',
                    border: false,
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            anchor: '100%',
                            bodyStyle: Gkh.bodyStyle,
                            border: false,
                            padding: '0 0 3 0',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер обращения',
                                    flex: 1,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    anchor: '100%',
                                    width: 250,
                                    name: 'DateFrom',
                                    fieldLabel: 'От',
                                    format: 'd.m.Y',
                                    labelWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            anchor: '100%',
                            bodyStyle: Gkh.bodyStyle,
                            border: false,
                            padding: '0 0 3 0',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    bodyStyle: Gkh.bodyStyle,
                                    border: false,
                                    padding: '0 0 5 0',
                                    layout: { type: 'hbox', pack: 'start', },
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhintfield',
                                            width: 250,
                                            name: 'Year',
                                            fieldLabel: 'Год'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: '1',
                                    bodyStyle: Gkh.bodyStyle,
                                    border: false,
                                    padding: '0 0 5 0',
                                    layout: { type: 'hbox', pack: 'end' },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            anchor: '100%',
                                            width: 284,
                                            name: 'ExtensTime',
                                            fieldLabel: 'Продленный контрольный срок',
                                            format: 'd.m.Y',
                                            labelWidth: 184,

                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            anchor: '100%',
                            bodyStyle: Gkh.bodyStyle,
                            border: false,
                            padding: '0 0 3 0',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    width: 250,
                                    name: 'CheckTime',
                                    fieldLabel: 'Контрольный срок',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    width: 250,
                                    name: 'ControlDateGisGkh',
                                    fieldLabel: 'Дата ГИС ЖКХ',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NumberGji',
                                    flex: 1,
                                    fieldLabel: 'Номер ГЖИ',
                                    allowBlank: false,
                                    maxLength: 255
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ZonalInspection',
                            itemId: 'sflZonalInspection',
                            fieldLabel: B4.GjiTextValuesOverride.getText('ГЖИ, рассмотревшая обращение'),
                            store: 'B4.store.dict.ZonalInspection',
                            allowBlank: false,
                            editable: false,
                            textProperty: 'ZoneName',
                            flex: 1
                        },
                        {
                            xtype: 'edoPanel'
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'appealCitizensTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            autoScroll: true,
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'OrderContragent',
                                    fieldLabel: 'Контрагент',
                                    store: 'B4.store.Contragent',
                                    itemId: 'sflOrderContragent',
                                    editable: false,
                                    allowBlank: true,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListMoAreaWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Корреспондент',
                                    defaults: {
                                        labelWidth: 140,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            name: 'TypeCorrespondent',
                                            fieldLabel: 'Тип корреспондента',
                                            displayField: 'Display',
                                            store: B4.enums.TypeCorrespondent.getStore(),
                                            valueField: 'Value',
                                            editable: false,
                                            itemId: 'cbTypeCorrespondent'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'Name',
                                            name: 'ContragentCorrespondent',
                                            fieldLabel: 'Контрагент',
                                            editable: false,
                                            columns: [
                                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            itemId: 'cbContragentCorrespondent',
                                            allowBlank: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Correspondent',
                                            fieldLabel: 'Корреспондент',
                                            maxLength: 255
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            editable: false,
                                            items: B4.enums.Gender.getItems(),
                                            name: 'DeclarantSex',
                                            itemId: 'cbDeclarantSex',
                                            fieldLabel: 'Пол'

                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Citizenship',
                                            name: 'Citizenship',
                                            fieldLabel: 'Гражданство',
                                            editable: false,
                                            columns: [
                                                { text: 'Код ОКСМ', dataIndex: 'OksmCode', flex: 1, filter: { xtype: 'textfield', hideTrigger: true, operand: 'eq' } },
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'panel',
                                            padding: '0 0 3 0',
                                            bodyStyle: Gkh.bodyStyle,
                                            border: false,
                                            defaults: {
                                                labelWidth: 140,
                                                labelAlign: 'right'
                                            },
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CorrespondentAddress',
                                                    fieldLabel: 'Адрес корреспондента',
                                                    labelAlign: 'right',
                                                    flex: 2,
                                                    maxLength: 2000
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FlatNum',
                                                    fieldLabel: 'Номер квартиры',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 255
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DeclarantMailingAddress',
                                            fieldLabel: 'Почтовый адрес'
                                        },
                                        {
                                            xtype: 'panel',
                                            padding: '0 0 3 0',
                                            bodyStyle: Gkh.bodyStyle,
                                            border: false,
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelWidth: 140,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Email',
                                                    fieldLabel: 'Электронная почта',
                                                    maxLength: 255,
                                                    vtype: 'email'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Phone',
                                                    fieldLabel: 'Контактный телефон',
                                                    maxLength: 255
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DeclarantWorkPlace',
                                            fieldLabel: 'Место работы заявителя'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Обращение',
                                    defaults: {
                                        labelWidth: 140,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'radiogroup',
                                            fieldLabel: 'Контрольное',
                                            columns: 1,
                                            vertical: true,
                                            items: [
                                                {
                                                    name: 'AppealStatus',
                                                    inputValue: B4.enums.AppealStatus.Control,
                                                    boxLabel: 'Да'
                                                },
                                                {
                                                    name: 'AppealStatus',
                                                    inputValue: B4.enums.AppealStatus.Uncontrol,
                                                    boxLabel: 'Нет'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'AmountPages',
                                            itemId: 'nfAmountPages',
                                            fieldLabel: 'Количество листов в обращении',
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'panel',
                                            padding: '0 0 3 0',
                                            bodyStyle: Gkh.bodyStyle,
                                            border: false,
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelWidth: 140,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    itemId: 'kindStatementSelectField',
                                                    name: 'KindStatement',
                                                    fieldLabel: 'Вид обращения',
                                                    textProperty: 'Name',


                                                    store: 'B4.store.dict.KindStatementGji',
                                                    editable: false,
                                                    columns: [
                                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                                                        { text: 'Код', dataIndex: 'Code', flex: 1 },
                                                        { text: 'Описание', dataIndex: 'Description', flex: 1 }
                                                    ],
                                                    flex: 0.7
                                                },
                                                {
                                                    xtype: 'gkhintfield',
                                                    name: 'QuestionsCount',
                                                    itemId: 'nfQuestionsCount',
                                                    fieldLabel: 'Количество вопросов',
                                                    flex: 0.3
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textarea',
                                            anchor: '100%',
                                            name: 'Description',
                                            fieldLabel: 'Описание',
                                            height: 45,
                                            labelAlign: 'right',
                                            maxLength: 10000
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'AppealRegistrator',
                                            fieldLabel: 'Регистратор обращения',
                                            editable: false,
                                            columns: [
                                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            textProperty: 'Fio'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'PlannedExecDate',
                                            fieldLabel: 'Планируемая дата исполнения',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'panel',
                                            padding: '0 0 3 0',
                                            bodyStyle: Gkh.bodyStyle,
                                            border: false,
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelWidth: 140,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4combobox',
                                                    editable: false,
                                                    name: 'RedtapeFlag',
                                                    itemId: 'cbRedtapeFlag',
                                                    fieldLabel: 'Признак волокиты',
                                                    displayField: 'Name',
                                                    valueField: 'Id',
                                                    url: '/RedtapeFlagGji/List'
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    itemId: 'previousAppealCitsSelectField',
                                                    name: 'PreviousAppealCits',
                                                    fieldLabel: 'Предыдущее обращение',
                                                    textProperty: 'NumberGji',


                                                    store: 'B4.store.AppealCits',
                                                    editable: false,
                                                    columns: [
                                                        { text: 'Номер ГЖИ', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                                                        { xtype: 'datecolumn', text: 'Дата обращения', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
                                                    ],
                                                    labelWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            fieldLabel: 'Файл',
                                            labelAlign: 'right',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ExecutantTakeDate',
                                            fieldLabel: 'Дата приёма в работу исполнителем',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'Executant',
                                            itemId: 'appealCitsExecutantSelectField',
                                            fieldLabel: 'Исполнитель',
                                            textProperty: 'Fio',
                                            editable: false,
                                            isGetOnlyIdProperty: true,
                                            columns: [
                                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'relatedAppeals',
                                    itemId: 'trigfRelatedAppealCitizens',
                                    fieldLabel: 'Связанные/аналогичные обращения'
                                }
                            ]
                        },
                        {
                            xtype: 'appealcitscategorygrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsattachmentgrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsquestiongrid',
                            flex: 1
                        },
                        {
                            title: 'Связанные/аналогичные обращения',
                            itemId: 'tabRelatedAppealCits',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            defaults: {
                                labelWidth: 80
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'start'
                            },
                            items: [
                                {
                                    itemId: 'gridRelatedAppealCits',
                                    xtype: 'relatedAppealCitsGrid',
                                    flex: 1,
                                    bodyStyle: 'backrgound-color:transparent;',
                                    region: 'center'
                                }
                            ]
                        },
                        {
                            title: 'Место возникновения проблемы',
                            itemId: 'tabLocationProblem',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            defaults: {
                                labelWidth: 80
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'start'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    height: 50,
                                    anchor: '100%',
                                    name: 'DescriptionLocationProblem',
                                    fieldLabel: 'Описание',
                                    labelAlign: 'right',
                                    maxLength: 255
                                },
                                {
                                    xtype: 'appealCitsRealObjGrid',
                                    flex: 1,
                                    bodyStyle: 'backrgound-color:transparent;',
                                    region: 'center'
                                }
                            ]
                        },
                        {
                            itemId: 'tabSources',
                            title: 'Источники поступления',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            layout: 'fit',
                            defaults: {
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'appealCitsSourceGrid',
                                    bodyStyle: 'backrgound-color:transparent;'
                                }
                            ]
                        },
                        {
                            title: 'Тематики',
                            itemId: 'tabStatementSubject',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            layout: 'fit',
                            defaults: {
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'appealCitsStatSubjectGrid',
                                    bodyStyle: 'backrgound-color:transparent;'
                                }
                            ]
                        },
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Рассмотрение',
                            border: false,
                            frame: true,
                            itemId: 'tabApproval',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ResolveGji',
                                    name: 'SuretyResolve',
                                    labelAlign: 'right',
                                    labelWidth: 145,
                                    fieldLabel: 'Виза',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Comment',
                                    labelAlign: 'right',
                                    labelWidth: 145,
                                    fieldLabel: 'Комментарий'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    name: 'ApprovalContragent',
                                    labelAlign: 'right',
                                    labelWidth: 145,
                                    fieldLabel: 'Контрагент',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'tabpanel',
                                    flex: 1,
                                    items: [                                        
                                        {
                                            xtype: 'appealcitsheadinspectorgrid',
                                            title: 'Руководители',
                                            flex: 1
                                        },                                        
                                        {
                                            xtype: 'appealcitsexecutantgrid',
                                            title: 'Исполнители',
                                            flex: 1
                                        },
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'baseStatementAppCitsGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealCitsRequestGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealCitsAdmonitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealCitsEmergencyHouseGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsDecisionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsAnswerGrid',
                            flex: 1
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать проверку',
                                    iconCls: 'icon-application-go',
                                    textAlign: 'left',
                                    itemId: 'btnCreateStatement'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Перейти в СОПР',
                                    iconCls: 'icon-application-go',
                                    textAlign: 'left',
                                    itemId: 'btnSOPR'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-cog-go',
                                    name: 'btnGetAttachmentArchive',
                                    text: 'Скачать вложения архивом',
                                    disabled: false
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Копировать',
                                    iconCls: 'icon-application-go',
                                    itemId: 'btnCopy',
                                    name: 'btnCopy',
                                    textAlign: 'left'
                                }
                            ]
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
    }
});