Ext.define('B4.view.appealcits.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1200,
    minWidth: 800,
    height: 750,
    resizable: true,
    bodyPadding: 3,
    itemId: 'appealCitsEditWindow',
    title: 'Обращение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.MultiRowTabPanel',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.appealcits.RealityObjectGrid',
        'B4.view.appealcits.StatSubjectGrid',
        'B4.view.appealcits.SourceGrid',
        'B4.view.appealcits.RequestGrid',
        'B4.view.appealcits.AnswerGrid',
        'B4.view.appealcits.AppealCitsExecutionTypeGrid',
        'B4.view.appealcits.EdoPanel',
        'B4.view.appealcits.BaseStatementGrid',
        'B4.view.Control.GkhIntField',
        'B4.view.appealcits.PreliminaryCheckGrid',
        'B4.store.dict.ZonalInspection',
        'B4.store.dict.Inspector',
        'B4.view.appealcits.AppealCitsAdmonitionGrid',
        'B4.store.Contragent',
        'B4.store.dict.ResolveGji',
        'B4.form.FileField',
        'B4.enums.QuestionStatus',
        'B4.enums.TypeCorrespondent',
        'B4.GjiTextValuesOverride',
        'B4.store.dict.SSTUTransferOrg',
        'B4.view.appealcits.RelatedAppealCitsGrid',
        'B4.view.appealcits.AppealCitsAttachmentGrid',
        'B4.view.appealcits.AppealCitsResolutionGrid',
        'B4.view.appealcits.TypeOfFeedbackGrid',
        'B4.view.appealcits.AppealCitsHistoryGrid'
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
                                    readOnly: true,
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
                                    hidden:true,
                                    name: 'CaseDate',
                                    itemId: 'dfSoprDate',
                                    fieldLabel: 'Cрок СОПР',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NumberGji',
                                    flex: 1,
                                    fieldLabel: 'Номер ГЖИ',
                                    allowBlank: true,
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
                            title: 'Основная',
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
                                    xtype: 'tabpanel',
                                    itemId: 'appealCitizensTabPanel2',
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
                                                    title: 'Заявитель',
                                                    defaults: {
                                                        labelWidth: 140,
                                                        labelAlign: 'right',
                                                        anchor: '100%'
                                                    },
                                                    items: [
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
                                                                    xtype: 'combobox',
                                                                    name: 'TypeCorrespondent',
                                                                    fieldLabel: 'Тип корреспондента',
                                                                    displayField: 'Display',
                                                                    store: B4.enums.TypeCorrespondent.getStore(),
                                                                    valueField: 'Value',
                                                                    flex:1,
                                                                    editable: false,
                                                                    itemId: 'cbTypeCorrespondent'
                                                                },
                                                                {
                                                                    xtype: 'b4selectfield',
                                                                    name: 'SocialStatus',
                                                                    itemId: 'sfSocialStatus',
                                                                    labelWidth: 160,
                                                                    flex: 1,
                                                                    fieldLabel: 'Социальное положение',
                                                                    store: 'B4.store.dict.SocialStatus',
                                                                    editable: false,
                                                                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                                                                },
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
                                                                    name: 'Correspondent',
                                                                    flex:2,
                                                                    fieldLabel: 'Заявитель',
                                                                    maxLength: 255
                                                                },
                                                                {
                                                                    xtype: 'checkbox',
                                                                    itemId: 'dfIsRF',
                                                                    labelWidth: 200,
                                                                    name: 'IdentityConfirmed',
                                                                    fieldLabel: 'Личность подтверждена',
                                                                    flex: 1,
                                                                    editable: true
                                                                },
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
                                                                    fieldLabel: 'Адрес заявителя',
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
                                                                },
                                                                {
                                                                    xtype: 'numberfield',
                                                                    name: 'CorrespondentAge',                                                             
                                                                    fieldLabel: 'Возраст',
                                                                    labelWidth: 60,
                                                                    width: 120,
                                                                    maxValue: 100,
                                                                    minValue: 0
                                                                },
                                                            ]
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
                                                                    //vtype: 'email'
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Phone',
                                                                    fieldLabel: 'Контактный телефон',
                                                                    maxLength: 255
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    title: 'Обращение',
                                                    defaults: {
                                                        labelWidth: 140,
                                                        anchor: '100%'
                                                    },
                                                    items: [
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
                                                            maxLength: 30000
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
                                                                    fieldLabel: 'Повторность обращения',
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
                                            layout: { type: 'vbox', align: 'stretch' },
                                            title: 'Рассмотрение',
                                            border: false,
                                            frame: true,
                                            itemId: 'tabApproval',
                                            items: [
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
                                                    xtype: 'appealcitsexecutantgrid',
                                                    flex: 1
                                                }
                                            ]
                                        },                                       
                                        {
                                            xtype: 'appealcitsAnswerGrid',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'appealcitsexecutiontypegrid',
                                            hidden:true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'appealCitsAdmonitionGrid',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'baseStatementAppCitsGrid',
                                            flex: 1
                                        },
                                        {
                                            itemId: 'tabpreliminaryCheck',
                                            title: 'Предварительные проверки',
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
                                                    xtype: 'preliminaryCheckGrid',
                                                    bodyStyle: 'backrgound-color:transparent;'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'appealCitsRequestGrid',
                                            flex: 1
                                        },

                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'appealcitsattachmentgrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealCitsResolutionGrid',
                            flex: 1
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
                               xtype: 'tabpanel',
                               itemId: 'appealCitizensTabPanel',
                               title: 'Связанные/аналогичные обращения',
                               flex: 1,
                               layout: {
                                   type: 'vbox',
                                   align: 'middle'
                               },
                               defaults: {
                                   style: {
                                       float: 'left'
                                   },
                                   height: 24,  //basic height
                                   margin: '0 4 8' //basic margins - all elements are floated to left without bottom margin
                               },
                               border: false,
                               items: [
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
                                          title: 'Обращения, связанные с текущим',
                                          itemId: 'tabAnotherrelatedAppealCitsGrid',
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
                                                  itemId: 'gridAnotherRelatedAppealCits',
                                                  xtype: 'anotherrelatedAppealCitsGrid',
                                                  flex: 1,
                                                  bodyStyle: 'backrgound-color:transparent;',
                                                  region: 'center'
                                              }
                                          ]
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
                            title: 'Обратная связь',
                            itemId: 'tabTypeOfFeedbackGrid',
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
                                    xtype: 'appealcitsTypeOfFeedbackGrid',
                                    bodyStyle: 'backrgound-color:transparent;'
                                }
                            ]
                        },
                        {
                            title: 'История изменений',
                            itemId: 'tabHistoryGrid',
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
                                    itemId: 'gridAppealCitsHistoryGrid',
                                    xtype: 'appealcitshistorygrid',
                                    bodyStyle: 'backrgound-color:transparent;'
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
                                    text: 'Назад',
                                   // iconCls: 'icon-cross',
                                    iconCls: 'icon-reload',
                                    textAlign: 'left',
                                    itemId: 'btnBack'
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
    }
});