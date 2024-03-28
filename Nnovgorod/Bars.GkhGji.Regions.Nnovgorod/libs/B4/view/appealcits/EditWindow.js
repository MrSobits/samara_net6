Ext.define('B4.view.appealcits.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 900,
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
        'B4.view.appealcits.BaseStatementGrid',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ZonalInspection',
        'B4.store.dict.Inspector',
        'B4.store.dict.ResolveGji',
        'B4.form.FileField',
        'B4.enums.TypeCorrespondent',
        'B4.GjiTextValuesOverride',
        'B4.form.ComboBox',
        'B4.enums.Accepting'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
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
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер обращения',
                                    flex: 1,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    anchor: '100%',
                                    width: 250,
                                    name: 'DateFrom',
                                    itemId: 'dfDateFrom',
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
                                    xtype: 'gkhintfield',
                                    width: 250,
                                    name: 'Year',
                                    itemId: 'fYear',
                                    fieldLabel: 'Год'
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
                                    xtype: 'button',
                                    text: 'История изменений',
                                    textAlign: 'left',
                                    actionName: 'checkTimeHistoryBtn',
                                    iconCls: 'icon-book-open',
                                    margin: '0 0 0 10'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ArchiveNumber',
                                    fieldLabel: 'Архивный номер',
                                    itemId: 'archiveNum',
                                    flex: 1,
                                    maxLength: 50,
                                    margin: '0 0 0 200'
                                }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Отдел, принявший обращение',
                            store: B4.enums.Accepting.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'Accepting',
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ZonalInspection',
                            itemId: 'sflZonalInspection',
                            fieldLabel: B4.GjiTextValuesOverride.getText('ГЖИ, рассмотревшая обращение'),
                            store: 'B4.store.dict.ZonalInspection',
                            allowBlank: true,
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
                                            xtype: 'textfield',
                                            name: 'Correspondent',
                                            fieldLabel: 'Корреспондент',
                                            maxLength: 255
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
                                                    maxLength: 255
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
                                            maxLength: 2000
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
                            bodyPadding: 5,
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Поручитель',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'Surety',
                                            itemId: 'appealCitsSuretySelectField',
                                            editable: false,
                                            fieldLabel: 'ФИО',
                                            textProperty: 'Fio',
                                            isGetOnlyIdProperty: true,
                                            columns: [
                                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            itemId: 'appealCitsSuretyPositionTextField',
                                            fieldLabel: 'Должность',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 150,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.ResolveGji',
                                                    name: 'SuretyResolve',
                                                    itemId: 'sflSuretyResolve',
                                                    fieldLabel: 'Резолюция',
                                                    flex: 1,
                                                    editable: false,
                                                    columns: [
                                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                                    ]
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'SuretyDate',
                                                    itemId: 'dfSuretyDate',
                                                    fieldLabel: 'Срок исполнения',
                                                    format: 'd.m.Y',
                                                    width: 250
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Исполнитель',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'Executant',
                                            itemId: 'appealCitsExecutantSelectField',
                                            fieldLabel: 'ФИО',
                                            textProperty: 'Fio',
                                            editable: false,
                                            isGetOnlyIdProperty: true,
                                            columns: [
                                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            itemId: 'appealCitsExecutantPositionTextField',
                                            fieldLabel: 'Должность',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'container',
                                            defaults: {
                                                labelWidth: 170,
                                                labelAlign: 'right'
                                            },
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.Inspector',
                                                    name: 'Tester',
                                                    itemId: 'sflTester',
                                                    fieldLabel: 'Проверяющий (инспектор)',
                                                    textProperty: 'Fio',
                                                    editable: false,
                                                    flex: 1,
                                                    columns: [
                                                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ExecuteDate',
                                                    itemId: 'dfExecuteDate',
                                                    fieldLabel: 'Срок исполнения',
                                                    format: 'd.m.Y',
                                                    labelWidth: 150,
                                                    width: 250
                                                }
                                            ]
                                        }
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