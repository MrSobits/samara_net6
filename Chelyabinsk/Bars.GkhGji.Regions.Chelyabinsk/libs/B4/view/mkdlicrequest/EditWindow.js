Ext.define('B4.view.mkdlicrequest.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1100,
    minWidth: 800,
    height: 800,
    resizable: true,
    bodyPadding: 3,
    itemId: 'mkdLicRequestEditWindow',
    title: 'Заявление',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.mkdlicrequest.FileGrid',
        'B4.view.mkdlicrequest.RealityObjectGrid',
        'B4.view.mkdlicrequest.SourceGrid',
        'B4.view.mkdlicrequest.HeadInspectorGrid',
        'B4.view.mkdLicRequest.AnswerGrid',
        'B4.view.mkdlicrequest.EdoPanel',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ZonalInspection',
        'B4.store.dict.Inspector',
        'B4.store.dict.ResolveGji',
        'B4.store.mkdlicrequest.MKDLicRequest',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.enums.AppealStatus',
        'B4.GjiTextValuesOverride',
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
                //{
                //    xtype: 'b4selectfield',
                //    name: 'SSTUTransferOrg',
                //    itemId: 'sfSSTUTransferOrg',
                //    textProperty: 'Name',
                //    labelWidth: 150,
                //    labelAlign: 'right',
                //    fieldLabel: 'Кому перенаправлено',
                //    store: 'B4.store.dict.SSTUTransferOrg',
                //    columns: [
                //        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                //    ],
                //    editable: false
                //},
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
                                    xtype: 'datefield',
                                    anchor: '100%',
                                    width: 250,
                                    name: 'StatementDate',
                                    fieldLabel: 'От',
                                    format: 'd.m.Y',
                                    allowBlank: false,
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
                                    xtype: 'textfield',
                                    name: 'StatementNumber',
                                    fieldLabel: 'Входящий номер',
                                    flex: 1,
                                    allowBlank: false,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'RequestFile',
                                    fieldLabel: 'Файл заявки',
                                    flex: 1
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
                                    xtype: 'b4selectfield',
                                    name: 'MKDLicTypeRequest',
                                    itemId: 'sfMKDLicTypeRequest',
                                    textProperty: 'Name',
                                    allowBlank: false,
                                    flex: 1,
                                    fieldLabel: 'Содержание заявления',
                                    store: 'B4.store.dict.MKDLicTypeRequest',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false
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
                                    xtype: 'b4selectfield',
                                    name: 'ExecutantDocGji',
                                    itemId: 'sfMExecutantDocGji',
                                    textProperty: 'Name',
                                    allowBlank: false,
                                    flex: 1,
                                    fieldLabel: 'Статус заявителя',
                                    store: 'B4.store.dict.ExecutantDocGji',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false
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
                                    //allowBlank: false,
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
                            //allowBlank: false,
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
                    itemId: 'mkdLicRequestTabPanel',
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
                                    title: 'Контрагент',
                                    defaults: {
                                        labelWidth: 140,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'Name',
                                            name: 'Contragent',
                                            fieldLabel: 'Контрагент',
                                            editable: false,
                                            columns: [
                                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            itemId: 'sfContragent',
                                            allowBlank: false
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
                                                    name: 'ContragentAddress',
                                                    fieldLabel: 'Адрес контрагента',
                                                    labelAlign: 'right',
                                                    flex: 2,
                                                    readonly: true,
                                                    maxLength: 2000
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
                                                readonly: true,
                                                maxLength: 255,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Email',
                                                    fieldLabel: 'Электронная почта',
                                                    vtype: 'email'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Phone',
                                                    fieldLabel: 'Контактный телефон'
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
                                                    name: 'RequestStatus',
                                                    inputValue: B4.enums.AppealStatus.Control,
                                                    boxLabel: 'Да'
                                                },
                                                {
                                                    name: 'RequestStatus',
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
                                                    itemId: 'previousRequestSelectField',
                                                    name: 'PreviousRequest',
                                                    fieldLabel: 'Предыдущее обращение',
                                                    textProperty: 'StatementNumber',
                                                    store: 'B4.store.mkdlicrequest.MKDLicRequest',
                                                    editable: false,
                                                    columns: [
                                                        { text: 'Номер', dataIndex: 'StatementNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                        { xtype: 'datecolumn', text: 'Дата обращения', dataIndex: 'StatementDate', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
                                                    ],
                                                    labelWidth: 150
                                                }
                                            ]
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
                                            itemId: 'mkdLicRequestExecutantSelectField',
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
                                }
                            ]
                        },
                        {
                            itemId: 'tabAttachment',
                            title: 'Вложения',
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
                                    xtype: 'mkdlicrequestfilegrid',
                                    bodyStyle: 'backrgound-color:transparent;'
                                }
                            ]
                        },
                        {
                            title: 'Многоквартирные дома',
                            itemId: 'tabRealityObject',
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
                                    xtype: 'mkdlicrequestrogrid',
                                    flex: 1,
                                    bodyStyle: 'backrgound-color:transparent;',
                                    region: 'center'
                                }
                            ]
                        },
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Рассмотрение',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
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
                                            xtype: 'mkdLicRequestHeadInspectorGrid',
                                            title: 'Руководители',
                                            flex: 1
                                        },                                        
                                        {
                                            xtype: 'mkdLicRequestExecutantGrid',
                                            title: 'Исполнители',
                                            flex: 1
                                        },
                                    ]
                                }
                            ]
                        },
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Решение',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            itemId: 'tabDecision',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 140
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ConclusionNumber',
                                    fieldLabel: 'Номер решения',
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ConclusionDate',
                                    fieldLabel: 'Дата решения',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'LicStatementResult',
                                    fieldLabel: 'Результат рассмотрения',
                                    enumName: 'B4.enums.LicStatementResult',
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ChangeDate',
                                    fieldLabel: 'Дата внесения изменений в реестр лицензий',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'ConclusionFile',
                                    fieldLabel: 'Файл решения'
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