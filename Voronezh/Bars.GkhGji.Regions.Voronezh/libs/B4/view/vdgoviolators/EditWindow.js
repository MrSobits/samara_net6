Ext.define('B4.view.vdgoviolators.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.view.Control.GkhTriggerField',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'vdgoviolatorsEditWindow',
    title: 'Окно редактирования нарушителей ВДГО',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Данные уведомления ВДГО',
                    items: [
                        {

                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    name: 'Contragent',
                                    disabled: true,
                                    fieldLabel: 'Контрагент ВДГО',
                                    isGetOnlyIdProperty: true,
                                    allowBlank: true,
                                    flex: 1,
                                    width: 150,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },

                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'NotificationDate',
                                    fieldLabel: 'Дата уведомления',
                                    allowBlank: true,
                                    width: 150,
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NotificationNumber',
                                    fieldLabel: 'Номер уведомления',
                                    allowBlank: true,
                                    width: 150,
                                    flex: 1,
                                }

                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.vdgoviolators.VDGOViolatorsMinOrgContr',
                                    name: 'MinOrgContragent',
                                    editable: true,
                                    fieldLabel: 'Контрагент УК',
                                    isGetOnlyIdProperty: true,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    flex: 1,
                                    width: 300,
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.vdgoviolators.VDGOViolatorsRO',
                                    name: 'Address',
                                    itemId: 'VDGOViolatorsROId',
                                    editable: true,
                                    fieldLabel: 'Адрес',
                                    isGetOnlyIdProperty: true,
                                    textProperty: 'Address',
                                    columns: [
                                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    flex: 1,
                                    width: 300,
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'NotificationFile',
                                    fieldLabel: 'Список нарушителей',
                                    labelAlign: 'right',
                                    textProperty: 'Name',
                                    possibleFileExtensions: 'xls,xlsx,doc,docx',
                                    width: 500,
                                }
                            ]
                        },
                    ]
                },

                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Данные об исполнении',
                    items: [
                        {

                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FIO',
                                    fieldLabel: 'ФИО',
                                    allowBlank: true,
                                    width: 150,
                                    flex: 1,
                                }

                            ]
                        },
                        {

                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PhoneNumber',
                                    fieldLabel: 'Номер телефона',
                                    allowBlank: true,
                                    width: 150,
                                    flex: 1,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    fieldLabel: 'Электронная почта',
                                    allowBlank: true,
                                    width: 150,
                                    flex: 1,
                                }

                            ]
                        },
                        {

                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateExecution',
                                    fieldLabel: 'Дата исполнения',
                                    allowBlank: true,
                                    width: 473,
                                    margin: '0 125 0 0'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'MarkOfExecution',
                                    boxLabel: 'Отметка об исполнении',
                                    checked: false
                                }
                            ]
                        },
                        {

                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0',
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Примечание',
                                    maxLength: 500,
                                    flex: 1
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Приложение',
                                    labelAlign: 'right',
                                    textProperty: 'Name',
                                    possibleFileExtensions: 'xls,xlsx,doc,docx',
                                    flex: 1
                                }
                            ]
                        },                        
                    ]
                },
                
                
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
                                xtype: 'b4savebutton'
                            }
                        ]
                    },
                    {
                        xtype: 'tbfill'
                    },
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
                            xtype: 'b4closebutton'
                        }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});