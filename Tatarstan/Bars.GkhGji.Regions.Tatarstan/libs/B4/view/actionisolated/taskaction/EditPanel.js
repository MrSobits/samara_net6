Ext.define('B4.view.actionisolated.taskaction.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'taskactionEditPanel',
    title: 'Задание',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhTriggerField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.view.actionisolated.taskaction.HouseGrid',
        'B4.view.actionisolated.taskaction.ItemGrid',
        'B4.view.actionisolated.taskaction.AnnexGrid',
        'B4.view.actionisolated.taskaction.ArticleLawGrid',
        'B4.view.actionisolated.taskaction.SurveyPurposeGrid',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.form.field.GkhTimeField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.GjiDocumentCreateButton',
        'B4.store.dict.PlanActionGji',
        'B4.store.dict.Inspector',
        'B4.store.AppealCits',
        'B4.store.Contragent',
        'B4.store.dict.ControlType',
        'B4.store.dict.ZonalInspection',
        'B4.enums.TypeDocumentGji',
        'B4.enums.TypeObjectAction',
        'B4.enums.TypeBaseAction',
        'B4.enums.TypeJurPersonAction'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'container',
                    autoScroll: true,
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            width: 500,
                            padding: '10 0 5 15',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    border: false,
                                    width: 550,
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            readOnly: true,
                                            labelAlign: 'right',
                                            labelWidth: 100,
                                            width: 300,
                                            itemId: 'documentNum'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y',
                                            allowBlank: false,
                                            readOnly: true,
                                            labelWidth: 50,
                                            labelAlign: 'right',
                                            width: 150,
                                            itemId: 'documentDate'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    labelAlign: 'right',
                                    labelWidth: 100,
                                    width: 450,
                                    readOnly: true,
                                    includeNull: false,
                                    enumName: 'B4.enums.TypeObjectAction',
                                    name: 'TypeObject',
                                    fieldLabel: 'Объект мероприятия',
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    labelAlign: 'right',
                                    labelWidth: 100,
                                    width: 450,
                                    readOnly: true,
                                    includeNull: false,
                                    enumName: 'B4.enums.TypeJurPersonAction',
                                    name: 'TypeJurPerson',
                                    fieldLabel: 'Тип контрагента',
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            width: 1000,
                            padding: '10 15 5 0',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    border: false,
                                    layout: 'hbox',
                                    items: [
                                        {

                                            padding: '0 0 5 0',
                                            xtype: 'b4selectfield',
                                            labelAlign: 'right',
                                            editable: false,
                                            readOnly: true,
                                            store: 'B4.store.dict.Municipality',
                                            name: 'Municipality',
                                            labelWidth: 180,
                                            fieldLabel: 'Муниципальное образование',
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                            ],
                                            itemId: 'municipality',
                                            width: 530
                                        },
                                        {
                                            padding: '0 0 5 0',
                                            xtype: 'b4selectfield',
                                            labelAlign: 'right',
                                            editable: false,
                                            readOnly: true,
                                            store: 'B4.store.dict.ZonalInspection',
                                            name: 'ZonalInspection',
                                            textProperty: 'ZoneName',
                                            labelWidth: 100,
                                            fieldLabel: 'Орган ГЖИ',
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'ZoneName', flex: 1 }
                                            ],
                                            width: 450
                                        },
                                    ]
                                },
                                {
                                    padding: '0 0 5 0',
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    name: 'Contragent',
                                    labelWidth: 180,
                                    fieldLabel: 'Организация',
                                    labelAlign: 'right',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ],
                                    width: 980,
                                    readOnly: true
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    border: false,
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PersonName',
                                            fieldLabel: 'ФИО',
                                            labelAlign: 'right',
                                            labelWidth: 180,
                                            width: 530,
                                            maxLength: 255
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Inn',
                                            fieldLabel: 'ИНН',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            width: 450,
                                            maxLength: 12
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'taskactionTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    border: false,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 120
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4enumcombo',
                                                            padding: '0 0 5 0',
                                                            enumName: 'B4.enums.KindAction',
                                                            name: 'KindAction',
                                                            fieldLabel: 'Вид мероприятия',
                                                            includeNull: false,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'b4selectfield',
                                                            name: 'ControlType',
                                                            itemId: 'tfControlType',
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
                                                            xtype: 'b4enumcombo',
                                                            name: 'TypeBase',
                                                            fieldLabel: 'Основание мероприятия',
                                                            enumName: 'B4.enums.TypeBaseAction',
                                                            includeNull: false,
                                                            readOnly: true
                                                        },
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    border: false,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 130
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            name: 'PlanAction',
                                                            store: 'B4.store.dict.PlanActionGji',
                                                            textProperty: 'Name',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'Name',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                }
                                                            ],
                                                            fieldLabel: 'План'
                                                        },
                                                        {
                                                            xtype: 'b4selectfield',
                                                            name: 'AppealCits',
                                                            itemId: 'sfAppealCits',
                                                            fieldLabel: 'Обращение гражданина',
                                                            textProperty: 'NumberGji',
                                                            store: 'B4.store.AppealCits',
                                                            editable: false,
                                                            columns: [
                                                                { text: 'Номер', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                                                                { xtype: 'datecolumn', text: 'Дата обращения', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                                { text: 'Номер ГЖИ', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                                                                { text: 'Управляющая организация', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                                                                { text: 'Количество вопросов', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'gkhintfield' } }
                                                            ],
                                                            updateDisplayedText: function (data) {
                                                                var me = this,
                                                                    date = data && data['DateFrom']
                                                                        ? new Date(data['DateFrom'])
                                                                        .toLocaleDateString()
                                                                        : '',
                                                                    text = data && data['NumberGji']
                                                                        ? data['NumberGji'] +
                                                                        (date ? ' от ' + date : '')
                                                                        : '';

                                                                me.setRawValue.call(me, text);
                                                            }
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            border: false,
                                                            layout: 'hbox',
                                                            defaults: {
                                                                xtype: 'datefield',
                                                                labelAlign: 'right',
                                                                flex: 1,
                                                                allowBlank: false
                                                            },
                                                            items: [
                                                                {
                                                                    labelWidth: 130,
                                                                    fieldLabel: 'Дата начала мероприятия',
                                                                    name: 'DateStart',
                                                                    format: 'd.m.Y'
                                                                },
                                                                {
                                                                    xtype: 'gkhtimefield',
                                                                    labelWidth: 100,
                                                                    fieldLabel: 'Время начала мероприятия',
                                                                    name: 'TimeStart',
                                                                    increment: 30
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 120,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'PlannedActions',
                                            itemId: 'tfPlannedActions',
                                            fieldLabel: 'Запланированные действия',
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                        allowBlank: false
                                    },
                                    title: 'Должностные лица',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            border: false,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.Inspector',
                                                    name: 'IssuedTask',
                                                    fieldLabel: 'ДЛ, вынесшее задание',
                                                    textProperty: 'Fio',
                                                    labelWidth: 120,
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
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.Inspector',
                                                    name: 'ResponsibleExecution',
                                                    fieldLabel: 'Ответственный за исполнение',
                                                    textProperty: 'Fio',
                                                    labelWidth: 130,
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
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'Inspectors',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                        allowBlank: true
                                    },
                                    title: 'Документ основание',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'BaseDocumentName',
                                            fieldLabel: 'Наименование',
                                            maxLength: 255

                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            border: false,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'BaseDocumentNumber',
                                                    fieldLabel: 'Номер',
                                                    labelWidth: 120,
                                                    maxLength: 255

                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'BaseDocumentDate',
                                                    fieldLabel: 'Дата',
                                                    labelWidth: 130,
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'BaseDocumentFile',
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'taskactionisolatedhousegrid'
                        },
                        {
                            xtype: 'taskactionisolatedtaskitemgrid'
                        },
                        {
                            xtype: 'tasksurveypurposegrid'
                        },
                        {
                            xtype: 'taskactionisolatedarticlelawgrid'
                        },
                        {
                            xtype: 'taskactionisolatedannexgrid'
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
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
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