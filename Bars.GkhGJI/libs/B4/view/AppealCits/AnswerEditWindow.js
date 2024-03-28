Ext.define('B4.view.appealcits.AnswerEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 500,
    minHeight: 400,
    height: 450,
    bodyPadding: 5,
    itemId: 'appealCitsAnswerEditWindow',
    title: 'Ответ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.store.dict.Inspector',
        'B4.enums.TypeAppealAnswer',
        'B4.store.dict.RevenueSourceGji',
      
        'B4.store.dict.AnswerContentGji'
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
                    name: 'TypeAppealAnswer',
                    fieldLabel: 'Тип ответа',
                    enumName: 'B4.enums.TypeAppealAnswer',
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
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'Executor',
                    fieldLabel: 'Исполнитель',
                    editable: false,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Начальник', xtype: 'gridcolumn', dataIndex: 'IsHead', flex: 1, filter: { xtype: 'textfield' }, 
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
                    itemId: 'dfAnswerContent',
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
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
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
                        ,
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