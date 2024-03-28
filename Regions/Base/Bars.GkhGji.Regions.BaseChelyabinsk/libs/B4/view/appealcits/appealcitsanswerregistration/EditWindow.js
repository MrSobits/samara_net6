Ext.define('B4.view.appealcits.appealcitsanswerregistration.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 650,
    minWidth: 500,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'ansRegEditWindow',
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
        'B4.store.Contragent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                    disabled: true,
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    disabled: true,
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
                            disabled: true,
                            maxLength: 300,
                            labelWidth: 130
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            disabled: true,
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
                    disabled: true,
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
                    disabled: true,
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
                    disabled: true,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            header: 'Начальник', xtype: 'gridcolumn', dataIndex: 'IsHead', flex: 1, filter: { xtype: 'textfield' },
                            renderer: function (val) {
                                return val ? "Да" : "Нет";
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'Signer',
                    fieldLabel: 'Подписант',
                    disabled: true,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            header: 'Начальник', xtype: 'gridcolumn', dataIndex: 'IsHead', flex: 1, filter: { xtype: 'textfield' },
                            renderer: function (val) {
                                return val ? "Да" : "Нет";
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.AnswerContentGji',
                    textProperty: 'Name',
                    name: 'AnswerContent',
                    fieldLabel: 'Содержание ответа',
                    disabled: true,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    disabled: true,
                },
                {
                    xtype: 'b4filefield',
                    name: 'SignedFile',
                    fieldLabel: 'Подписанный файл',
                    disabled: true,
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
                            fieldLabel: 'Текст ответа 1',
                            flex: 1,
                            height: 50,
                            maxHeight: 50,
                            disabled: true,
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
                            disabled: true,
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
                        disabled: true,
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
                    disabled: true,
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
                    disabled: true,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'b4closebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});