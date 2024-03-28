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
                                            xtype: 'b4selectfield',
                                            name: 'RealityObject',
                                            fieldLabel: 'Адрес',
                                            store: 'B4.store.RealityObject',
                                            editable: false,
                                            columns: [
                                                {
                                                    text: 'Муниципальное образование',
                                                    dataIndex: 'Municipality',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'b4combobox',
                                                        operand: CondExpr.operands.eq,
                                                        storeAutoLoad: false,
                                                        hideLabel: true,
                                                        editable: false,
                                                        valueField: 'Name',
                                                        emptyItem: { Name: '-' },
                                                        url: '/Municipality/ListWithoutPaging'
                                                    }
                                                },
                                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            textProperty: 'Address',
                                            allowBlank: false,
                                            labelAlign: 'right',
                                            labelWidth: 170,
                                            anchor: '100%',
                                            updateDisplayedText: function(data) {
                                                var me = this,
                                                    text;

                                                if (Ext.isString(data)) {
                                                    text = data;
                                                } else {
                                                    var municipality = data && data['Municipality'] ? data['Municipality'] : '';
                                                    var address = '';
                                                    if (data && data['Address']) {
                                                        address = Ext.isEmpty(municipality) ? '' : ', ';
                                                        address = address + data['Address'];
                                                    }
                                                    text = municipality + address;
                                                    if (Ext.isEmpty(text) && Ext.isArray(data)) {
                                                        text = Ext.Array.map(data, function(record) { return record[me.textProperty]; }).join();
                                                    }
                                                }

                                                me.setRawValue.call(me, text);
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Исполнитель',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right',
                                        labelWidth: 170,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            fieldLabel: 'Тип контрагента',
                                            store: B4.enums.ExecutorType.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'ExecutorType',
                                            disabled: true
                                        },
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
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'Исполнитель',
                                            name: 'Executor',
                                            editable: false,
                                            allowBlank: true,
                                            store: Ext.create('B4.base.Store', {
                                                autoLoad: false,
                                                fields: ['Id', 'Name'],
                                                proxy: {
                                                    type: 'b4proxy',
                                                    url: B4.Url.action('ListExecutor', 'CitizenSuggestion')
                                                }
                                            }),
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        }
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
                                            fieldLabel: 'Заявитель'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantAddress',
                                            fieldLabel: 'Почтовый адрес заявителя'
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
                                                    labelWidth: 170
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 120,
                                                    name: 'ApplicantEmail',
                                                    fieldLabel: 'E-mail'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Описание проблемы',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 170,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ProblemPlace',
                                            fieldLabel: 'Место проблемы',
                                            store: 'B4.store.dict.ProblemPlace',
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            editable: false,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Description',
                                            fieldLabel: 'Описание проблемы',
                                            maxLength: 1000,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'citsugfilegrid',
                                            height: 175,
                                            store: 'suggestion.ProblemFiles',
                                            type: 'Problem'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Ответ',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 170,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'AnswerDate',
                                            fieldLabel: 'Дата ответа',
                                            format: 'd.m.Y',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'AnswerText',
                                            fieldLabel: 'Ответ',
                                            maxLength: 10000
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'AnswerFile',
                                            fieldLabel: 'Файл'
                                        },
                                        {
                                            xtype: 'citsugfilegrid',
                                            height: 175,
                                            store: 'suggestion.AnswerFiles',
                                            type: 'Answer'
                                        }
                                    ]
                                }
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'btnExportToGji',
                                    text: 'Экспорт в реестр обращений',
                                    disabled: true
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