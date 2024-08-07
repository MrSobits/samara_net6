﻿Ext.define('B4.view.appealcits.AnswerEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch', pack: 'start' },
    width: 900,
    minWidth: 800,
    height: 500,
    resizable: true,
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
        'B4.store.dict.Inspector',
        'B4.store.dict.AnswerContentGji',
        'B4.view.appealcits.AnswerAddresseeGrid'
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
                                labelWidth: 150,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'от',
                                    format: 'd.m.Y'
                                }
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
                                {
                                    header: 'Начальник',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'IsHead',
                                    flex: 1,
                                    filter: { xtype: 'textfield' },
                                    renderer: function(val) {
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
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500,
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'appealcitsansweraddresseedrid',
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
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
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