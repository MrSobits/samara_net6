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
        'B4.view.Control.GkhButtonPrint',
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
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'KindKNDGJI',
                            labelWidth: 170,
                            fieldLabel: 'Вид контроля',
                            enumName: 'B4.enums.KindKNDGJI',
                            flex: 1
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            fieldLabel: 'Документ',
                            maxLength: 300,
                            flex: 1
                        }
                    ]
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