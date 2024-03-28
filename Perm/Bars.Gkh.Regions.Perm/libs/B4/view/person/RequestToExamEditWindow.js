Ext.define('B4.view.person.RequestToExamEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.personrequesttoexameditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 900,
    minWidth: 900,
    minHeight: 300,
    bodyPadding: 5,
    title: 'Заявка на доступ к экзамену',
    closeAction: 'destroy',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.RequestSupplyMethod',
        'B4.form.EnumCombo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Способ подачи заявления',
                    enumName: 'B4.enums.RequestSupplyMethod',
                    name: 'RequestSupplyMethod',
                    includeEmpty: false,
                    enumItems: []
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'RequestNum',
                            labelWidth: 180,
                            fieldLabel: 'Номер',
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        },
                        {
                            xtype: 'datefield',
                            name: 'RequestDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y'
                        },
                        {
                            fieldLabel: 'Время',
                            name: 'RequestTime',
                            xtype: 'textfield',
                            regex: /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                            regexText: 'В это поле необходимо вводить время в формате 00:00'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'RequestFile',
                    fieldLabel: 'Файл заявления'
                },
                {
                    xtype: 'b4filefield',
                    name: 'PersonalDataConsentFile',
                    fieldLabel: 'Файл согласия на обработку перс.данных'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NotificationNum',
                            labelWidth: 180,
                            fieldLabel: 'Номер уведомления'
                        },
                        {
                            xtype: 'datefield',
                            name: 'NotificationDate',
                            fieldLabel: 'Дата уведомления',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Отказ',
                            fieldStyle: 'vertical-align: middle; margin-left: 100px;',
                            name: 'IsDenied'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Экзамен назначен на',
                    padding: '5 5 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ExamDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y'
                        },
                        {
                            fieldLabel: 'Время',
                            name: 'ExamTime',
                            xtype: 'textfield',
                            regex: /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                            regexText: 'В это поле необходимо вводить время в формате 00:00'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Результаты экзамена',
                    padding: '5 5 5 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'CorrectAnswersPercent',
                            fieldLabel: 'Количество набранных баллов',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            flex: 0.5,
                            minValue: 0
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 10 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ProtocolNum',
                                    fieldLabel: 'Номер протокола'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ProtocolDate',
                                    fieldLabel: 'Дата протокола',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'ProtocolFile',
                            flex: 1,
                            fieldLabel: 'Файл протокола'
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ResultNotificationNum',
                                    fieldLabel: 'Номер уведомления',
                                    labelWidth: 180
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ResultNotificationDate',
                                    fieldLabel: 'Дата уведомления',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'MailingDate',
                                    fieldLabel: 'Дата отправки почтой',
                                    format: 'd.m.Y'
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
                            items: [
                                { xtype: 'b4savebutton' },
                                {
                                    xtype: 'b4addbutton',
                                    action: 'AddCert',
                                    text: 'Добавить квалификационный аттестат'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});