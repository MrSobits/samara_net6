Ext.define('B4.view.suggestion.citizensuggestion.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.citizensuggestionwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    height: 600,
    bodyPadding: 5,
    title: 'Редактирование',

    requires: [
        'B4.form.SelectField',
        'B4.form.Combobox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.suggestion.rubric.Grid',
        'B4.view.realityobj.Grid',
        'B4.store.suggestion.Rubric',
        'B4.store.RealityObject',
        'B4.store.dict.ProblemPlace',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.dict.problemplace.Grid'
    ],

    initComponent: function() {
        var me = this,
            historyStore = Ext.StoreManager.lookup('suggestion.History');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '5',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Number',
                            fieldLabel: 'Номер обращения',
                            maxLength: 120
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'CreationDate',
                            fieldLabel: 'Дата обращения'
                        },
                        {
                            xtype: 'datefield',
                            disabled: true,
                            visible: false,
                            allowBlank: true,
                            name: 'Deadline',
                            fieldLabel: 'Контрольный срок'
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'actCheckTabPanel',
                    border: false,
                    flex: 1,
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Общие сведения',
                            border: false,
                            frame: true,
                            autoScroll: true,
                            defaults: {
                                allowBlank: false,
                                labelAlign: 'right',
                                labelWidth: 170
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Место возникновения проблемы',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'RoomAddress',
                                            fieldLabel: 'Адрес',                                           
                                            allowBlank: false,
                                            readOnly: true,
                                            labelAlign: 'right',
                                            labelWidth: 170,
                                            anchor: '100%'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Категория сообщения',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right',
                                        labelWidth: 170,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Rubric',
                                            visible: false,
                                            fieldLabel: 'Рубрика',
                                            store: 'B4.store.suggestion.Rubric',
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            editable: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Тип домовладения',
                                            name: 'CategoryPosts',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'MessageSubject',
                                            fieldLabel: 'Категория сообщения',
                                            maxLength: 3000,
                                            readOnly: true,
                                            height: 50,
                                            allowBlank: false
                                        },
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Контактная информация для обратной связи',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right',
                                        labelWidth: 170,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantFio',
                                            fieldLabel: 'Заявитель',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantAddress',
                                            fieldLabel: 'Почтовый адрес заявителя',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'container',
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
                                                    xtype: 'textfield',
                                                    name: 'ApplicantPhone',
                                                    fieldLabel: 'Контактный телефон',
                                                    labelWidth: 170,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 120,
                                                    name: 'ApplicantEmail',
                                                    fieldLabel: 'E-mail',
                                                    readOnly: true
                                                }
                                            ]
                                        }
                                    ]
                                },
                                //{
                                //    xtype: 'fieldset',
                                //    title: 'Описание проблемы',
                                //    defaults: {
                                //        labelAlign: 'right',
                                //        labelWidth: 170,
                                //        anchor: '100%'
                                //    },
                                //    items: [
                                //        {
                                //            xtype: 'b4selectfield',
                                //            name: 'ProblemPlace',
                                //            fieldLabel: 'Место проблемы',
                                //            store: 'B4.store.dict.ProblemPlace',
                                //            columns: [
                                //                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                //            ],
                                //            editable: false,
                                //            allowBlank: false
                                //        },
                                //        {
                                //            xtype: 'textarea',
                                //            name: 'Description',
                                //            fieldLabel: 'Описание проблемы',
                                //            maxLength: 1000,
                                //            allowBlank: false
                                //        },
                                //        {
                                //            xtype: 'citsugfilegrid',
                                //            height: 175,
                                //            store: 'suggestion.ProblemFiles',
                                //            type: 'Problem'
                                //        }
                                //    ]
                                //},
                                //{
                                //    xtype: 'fieldset',
                                //    title: 'Ответ',
                                //    defaults: {
                                //        labelAlign: 'right',
                                //        labelWidth: 170,
                                //        anchor: '100%'
                                //    },
                                //    items: [
                                //        {
                                //            xtype: 'datefield',
                                //            name: 'AnswerDate',
                                //            fieldLabel: 'Дата ответа',
                                //            format: 'd.m.Y',
                                //            allowBlank: false
                                //        },
                                //        {
                                //            xtype: 'textarea',
                                //            name: 'AnswerText',
                                //            fieldLabel: 'Ответ',
                                //            maxLength: 1000
                                //        },
                                //        {
                                //            xtype: 'b4filefield',
                                //            name: 'AnswerFile',
                                //            fieldLabel: 'Файл'
                                //        },
                                //        {
                                //            xtype: 'citsugfilegrid',
                                //            height: 175,
                                //            store: 'suggestion.AnswerFiles',
                                //            type: 'Answer'
                                //        }
                                //    ]
                                //}
                            ]
                        },
                        {
                            xtype: 'citsugcommentgrid',
                            flex: 1
                        },
                        {
                            xtype: 'grid',
                            name: 'history',
                            store: historyStore,
                            title: 'История изменений',
                            columns: [
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'RecordDate',
                                    format: 'd.m.Y',
                                    flex: 1,
                                    text: 'Дата'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ExecutorName',
                                    text: 'Исполнитель',
                                    flex: 1
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'ExecutionDeadline',
                                    text: 'Срок исполнения',
                                    flex: 1
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    store: historyStore,
                                    dock: 'bottom'
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-cog-go',
                                    name: 'btnGetArchive',
                                    text: 'Скачать архивом',
                                    disabled: false
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
                                    name: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
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