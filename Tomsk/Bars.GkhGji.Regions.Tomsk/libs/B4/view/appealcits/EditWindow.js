Ext.define('B4.view.appealcits.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.appealCitsEditWindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align : 'stretch',
        pack  : 'start'
    },
    width: 900,
    minWidth: 800,
    height: 500,
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
        'B4.GjiTextValuesOverride'
    ],

    initComponent: function () {
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
                                    name: 'NumberGji',
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
                                    format: 'd.m.Y'
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
                                    xtype: 'gkhintfield',
                                    width: 250,
                                    name: 'Year',
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
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Особый контроль',
                                    name: 'SpecialControl',
                                    width: 250
                                }
                            ]
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
                                    title: 'Место хранения документа',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Документ помещен в дело №',
                                                    name: 'CaseNumber',
                                                    labelWidth: 180,
                                                    maxWidth: 280
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'CaseDate',
                                                    fieldLabel: 'От',
                                                    labelWidth: 30,
                                                    maxWidth: 130
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Корреспондент',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
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
                                            name: 'SocialStatus',
                                            itemId: 'sfSocialStatus',
                                            fieldLabel: 'Социальное положение',
                                            store: 'B4.store.dict.SocialStatus',
                                            editable: false,
                                            columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
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
                                                labelWidth: 150,
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
                                                labelWidth: 150,
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
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
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
                                                labelWidth: 150,
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
                                            name: 'Description',
                                            fieldLabel: 'Описание',
                                            height: 45,
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
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                               /*{
                                                    xtype: 'b4combobox',
                                                    editable: false,
                                                    name: 'RedtapeFlag',
                                                    itemId: 'cbRedtapeFlag',
                                                    fieldLabel: 'Признак волокиты',
                                                    displayField: 'Name',
                                                    valueField: 'Id',
                                                    url: '/RedtapeFlagGji/List',
                                                    hidden: true
                                                },*/
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
                                                        { text: 'Дата обращения', xtype: 'datecolumn', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                        { text: 'Корреспондент', dataIndex: 'Correspondent', flex: 1, filter: { xtype: 'textfield' } },
                                                        { text: 'Адрес корреспондента', dataIndex: 'CorrespondentAddress', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'relatedAppeals',
                                    itemId: 'trigfRelatedAppealCitizens',
                                    fieldLabel: 'Связанные/аналогичные обращения',
                                    labelWidth: 160
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
                                    name: 'DescriptionLocationProblem',
                                    fieldLabel: 'Описание',
                                    labelAlign: 'right',
                                    maxLength: 255
                                },
                                {
                                    xtype: 'appealCitsRealObjGrid',
                                    flex: 1,
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
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ResolveGji',
                                    name: 'SuretyResolve',
                                    itemId: 'sflSuretyResolve',
                                    fieldLabel: 'Резолюция',
                                    labelAlign: 'right',
                                    labelWidth: 145,
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
                                    xtype: 'appealcitsexecutantgrid',
                                    flex: 1
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
                                    text: 'Сформировать процесс по обращению',
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