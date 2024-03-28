Ext.define('B4.view.resolution.DecisionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 500,
    minHeight: 450,
    height: 500,
    bodyPadding: 5,
    itemId: 'resolutionDecisionEditWindow',
    title: 'Ответ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.TypeAppelantPresence',
        'B4.store.dict.Inspector',
        'B4.enums.TypeDecisionAnswer'
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
                    name: 'TypeDecisionAnswer',
                    fieldLabel: 'Тип решения',
                    enumName: 'B4.enums.TypeDecisionAnswer',
                   // flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'Apellant',
                    fieldLabel: 'Заявитель',
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    name: 'ApellantPlaceWork',
                    fieldLabel: 'Место работы',
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    name: 'ApellantPosition',
                    fieldLabel: 'Должность',
                    maxLength: 500
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
                            name: 'AppealNumber',
                            fieldLabel: 'Номер жалобы',
                            maxLength: 300,
                            labelWidth: 130
                        },
                        {
                            xtype: 'datefield',
                            name: 'AppealDate',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
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
                            xtype: 'b4enumcombo',
                            editable: false,
                            name: 'TypeAppelantPresence',
                            fieldLabel: 'Рассмотрено',
                            labelAlign: 'right',
                            enumName: 'B4.enums.TypeAppelantPresence',
                            allowBlank: false,
                            labelWidth: 130,
                        },
                        {
                            xtype: 'textfield',
                            name: 'RepresentativeFio',
                            fieldLabel: 'Представитель',
                            labelWidth: 100,
                            maxLength: 1500
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'ConsideringBy',
                    allowblank:false,
                    fieldLabel: 'Кто рассмотрел',
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
                    name: 'SignedBy',
                    fieldLabel: 'Подготовил',
                    allowblank: false,
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
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 300
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Установил',
                    flex: 1,
                    width: 350,
                    name: 'Established'
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Решил',
                    flex: 1,
                    width: 350,
                    name: 'Decided'
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
                                },
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