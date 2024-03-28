Ext.define('B4.view.appealcits.AppealCitsAdmonitionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'appealCitsAdmonitionEditWindow',
    title: 'Форма редактирования предостережения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.appealcits.AppCitAdmonVoilation',
        'B4.view.appealcits.AppCitAdmonVoilationGrid',
        'B4.store.appealcits.AppCitAdmonAnnex',
        'B4.view.appealcits.AppCitAdmonAnnexGrid',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.PayerType',
        'B4.store.dict.Inspector'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
               
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            labelWidth: 150 
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'PerfomanceDate',
                            fieldLabel: 'Срок исполнения',
                            labelWidth: 170 
                        },
                        {
                            name: 'PerfomanceFactDate',
                            fieldLabel: 'Дата факт. исполнения',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false
                },

                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0 0 5 0',
                        labelWidth: 150,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'PayerType',
                            fieldLabel: 'Вид исполнителя',
                            displayField: 'Display',
                            itemId: 'dfPayerType',
                            flex: 1,
                            store: B4.enums.PayerType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsUrParams',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты исполнителя - Юр.лицо',
                    hidden: true,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                /*margin: '10 0 5 0',*/
                                labelWidth: 80,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    name: 'Contragent',
                                    itemId: 'contragent',
                                    flex: 1,
                                    fieldLabel: 'Контрагент',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    allowBlank: false
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsFizParams',
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты исполнителя - физ.лицо',
                    hidden: true,
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'PhysicalPersonDocType',
                            fieldLabel: 'Вид документа',
                            store: 'B4.store.dict.PhysicalPersonDocType',
                            allowBlank: false,
                            editable: false,
                            flex: 1,
                            itemId: 'dfPhysicalPersonDocType',
                            columns: [
                                { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'FIO',
                            itemId: 'dfFIO',
                            fieldLabel: 'ФИО',
                            allowBlank: false,
                            disabled: false,
                            flex: 1,
                            editable: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentSerial',
                            itemId: 'dfDocumentSerial',
                            fieldLabel: 'Серия документа',
                            allowBlank: false,
                            disabled: false,
                            flex: 1,
                            editable: true,
                            maxLength: 20
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumberFiz',
                            itemId: 'dfDocumentNumberFiz',
                            fieldLabel: 'Номер документа',
                            allowBlank: false,
                            disabled: false,
                            flex: 1,
                            editable: true,
                            maxLength: 20
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsIpParams',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты исполнителя - ИП',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                //     margin: '10 0 5 0',
                                labelWidth: 80,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'INN',
                                    itemId: 'dfINN2',
                                    fieldLabel: 'ИНН',
                                    allowBlank: false,
                                    flex: 1,
                                    editable: true,
                                    maxLength: 12,
                                    regex: /^(\d{12})$/,
                                    regexText: '12 цифр'
                                },
                            ]
                        }
                    ]
                },

                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    name: 'Inspector',
                    editable: true,
                    fieldLabel: 'Должностное лицо, издавшее предостережение',
                    textProperty: 'Fio',
                    isGetOnlyIdProperty: true,
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                 {
                     xtype: 'b4selectfield',
                     store: 'B4.store.dict.Inspector',
                     name: 'Executor',
                     editable: true,
                     fieldLabel: 'Инспектор',
                     textProperty: 'Fio',
                     isGetOnlyIdProperty: true,
                     columns: [
                         { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                         { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                     ]
                 },
                 {
                     xtype: 'b4filefield',
                     name: 'AnswerFile',
                     fieldLabel: 'Файл ответа',
                     editable: true
                 },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'appCitAdmonVoilationGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appCitAdmonAnnexGrid',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                         {
                             xtype: 'buttongroup',
                             items: [
                                 {
                                     xtype: 'gkhbuttonprint'
                                 }
                             ]
                         },
                        {
                            xtype: 'tbfill'
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