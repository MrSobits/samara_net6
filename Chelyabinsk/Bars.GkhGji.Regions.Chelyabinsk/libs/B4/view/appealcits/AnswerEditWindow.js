Ext.define('B4.view.appealcits.AnswerEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 650,
    minWidth: 500,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'appealCitsAnswerEditWindow',
    title: 'Ответ',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.store.dict.Inspector',
        'B4.enums.TypeAppealAnswer',
        'B4.enums.TypeAppealFinalAnswer',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.AnswerContentGji',
        'B4.view.appealcits.AppealCitsAnswerAttachmentGrid',
        'B4.view.appealcits.AnswerStatSubjectGrid',
        'B4.store.Contragent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: null,
                    items: [
                        {
                            xtype: 'panel',
                            title: 'Общие сведения',
                            bodyPadding: 10,      
                            bodyStyle: Gkh.bodyStyle,
                            layout: { type: 'vbox', align: 'stretch' },
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeAppealFinalAnswer',
                                    fieldLabel: 'Тип ответа',
                                    enumName: 'B4.enums.TypeAppealFinalAnswer',
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentName',
                                    fieldLabel: 'Документ',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер документа',
                                            maxLength: 300,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            fieldLabel: 'от',
                                            format: 'd.m.Y',
                                            labelWidth: 130
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.RevenueSourceGji',
                                    textProperty: 'Name',
                                    name: 'Addressee',
                                    fieldLabel: 'Адресат',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    name: 'RedirectContragent',
                                    itemId: 'dfRedirectContragent',
                                    fieldLabel: 'Кому перенаправлено',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'ОГРН', xtype: 'gridcolumn', dataIndex: 'Ogrn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    allowBlank: false,
                                    name: 'Executor',
                                    fieldLabel: 'Исполнитель',
                                    editable: false,
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            header: 'Начальник', xtype: 'gridcolumn', dataIndex: 'IsHead', flex: 1, filter: { xtype: 'textfield' },
                                            renderer: function (val) {
                                                return val ? "Да" : "Нет";
                                            }
                                        }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: 'B4.store.dict.Inspector',
                                            dock: 'bottom'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    name: 'Signer',
                                    fieldLabel: 'Подписант',
                                    editable: false,
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            header: 'Начальник', xtype: 'gridcolumn', dataIndex: 'IsHead', flex: 1, filter: { xtype: 'textfield' },
                                            renderer: function (val) {
                                                return val ? "Да" : "Нет";
                                            }
                                        }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: 'B4.store.dict.Inspector',
                                            dock: 'bottom'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.AnswerContentGji',
                                    textProperty: 'Name',
                                    name: 'AnswerContent',
                                    fieldLabel: 'Содержание ответа',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Файл',
                                    editable: false
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'SignedFile',
                                    fieldLabel: 'Подписанный файл',
                                    editable: false
                                },
                                //{
                                //    xtype: 'textarea',
                                //    name: 'Description',
                                //    fieldLabel: 'Описание',
                                //    maxLength: 500,
                                //    flex: 1
                                //},
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textarea',
                                            fieldLabel: 'Текст ответа 1',
                                            flex: 1,
                                            height: 50,
                                            maxHeight: 50,
                                            //  width: 350,
                                            name: 'Description'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textarea',
                                            fieldLabel: 'Текст ответа 2',
                                            flex: 1,
                                            height: 50,
                                            maxHeight: 50,
                                            //  width: 350,
                                            name: 'Description2'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: { type: 'hbox', align: 'stretch' },
                                    margin: '0 0 5px 0',
                                    defaults: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                        labelStyle: 'padding: 0;'
                                    },
                                    items: [
                                        {
                                            name: 'ExecDate',
                                            fieldLabel: 'Дата исполнения (направления ответа)',
                                            allowBlank: false
                                        },
                                        {
                                            name: 'ExtendDate',
                                            fieldLabel: 'Дата продления срока исполнения'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.FactCheckingType',
                                    textProperty: 'Name',
                                    name: 'FactCheckingType',
                                    fieldLabel: 'Вид проверки фактов',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ConcederationResult',
                                    textProperty: 'Name',
                                    name: 'ConcederationResult',
                                    fieldLabel: 'Результат рассмотрения',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'appealcitsanswerattachmentgrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealcitsanswerstatsubjectgrid',
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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
                        },
                         {
                             xtype: 'buttongroup',
                             itemId: 'emailButtonGroup',
                             items: [
                                 {
                                     xtype: 'button',
                                     text: 'Отправить Email',
                                     tooltip: 'Отправить Email',
                                     iconCls: 'icon-accept',
                                     // width: 250,
                                     //    action: 'romExecute',
                                     itemId: 'sendEmailButton'
                                 },
                             ]
                         },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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