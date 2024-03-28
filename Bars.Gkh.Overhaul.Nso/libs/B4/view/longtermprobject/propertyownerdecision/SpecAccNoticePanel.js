Ext.define('B4.view.longtermprobject.propertyownerdecision.SpecAccNoticePanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.longtermdecisionspecaccnoticepanel',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.TypeOrganization',
        'B4.form.FileField'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Уведомление',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 10 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NoticeNumber',
                            fieldLabel: 'Номер уведомления',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата уведомления',
                            name: 'NoticeDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    labelAlign: 'right',
                    labelWidth: 150,
                    fieldLabel: 'Документ уведомления',
                    possibleFileExtensions: 'pdf'
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    title: 'Информация о владельце лицевого счета',
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'DecTypeOrganization',
                            fieldLabel: 'Тип организации-владельца',
                            displayField: 'Display',
                            store: B4.enums.TypeOrganization.getStore(),
                            valueField: 'Value',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование организации-владельца',
                            name: 'ContragentName'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ContragentMailingAddress',
                            fieldLabel: 'Почтовый адрес'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                flex: 1,
                                xtype: 'textfield'
                            },
                            items: [
                                {
                                    name: 'ContragentInn',
                                    fieldLabel: 'ИНН'
                                },
                                {
                                    name: 'ContragentKpp',
                                    fieldLabel: 'КПП'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                flex: 1,
                                xtype: 'textfield'
                            },
                            items: [
                                {
                                    name: 'ContragentOgrn',
                                    fieldLabel: 'ОГРН'
                                },
                                {
                                    name: 'ContragentOktmo',
                                    fieldLabel: 'ОКTMO'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Регистрация уведомления',
                    items: [
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
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата регистрации уведомления в ГЖИ',
                                    name: 'RegDate',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Входящий номер в ГЖИ',
                                    name: 'GjiNumber'
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
                                    xtype: 'checkbox',
                                    fieldLabel: 'Оригинал уведомления поступил',
                                    name: 'HasOriginal',
                                    labelWidth: 200
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Копия справки поступила',
                                    name: 'HasCopyCertificate',
                                    labelWidth: 190
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Копия протокола поступила',
                                    name: 'HasCopyProtocol',
                                    labelWidth: 190
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
                            columns: 2,
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});