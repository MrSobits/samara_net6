Ext.define('B4.view.GisGmpParamsPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.gisgmpparamspanel',
    closable: true,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    layout: { type: 'vbox', align: 'stretch' },
    title: 'Настройки интеграции ГИС ГМП',
    trackResetOnLoad: true,
    autoScroll: true,
    closeAction: 'hide',

    requires: [
        'B4.ux.button.Save',
        'B4.view.GisGmpPatternGrid',
        'B4.view.GisGmpPatternDictGrid',
        'B4.store.GisGmpPatternDict'
    ],

    initComponent: function () {
        var me = this,
            vBox = { type: 'vbox', align: 'stretch' },
            hBox = { type: 'hbox', align: 'stretch' };

        Ext.applyIf(me, {
            defaults: {
                layout: hBox,
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 150
                }
            },
            items: [
                {
                    xtype: 'container',
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'GisGmpEnable',
                            fieldLabel: 'Интеграция включена',
                            labelAlign: 'right',
                            labelWidth: 160
                        },
                        {
                            xtype: 'checkbox',
                            name: 'GisGmpLogEnable',
                            fieldLabel: 'Логирование включено',
                            labelAlign: 'right',
                            labelWidth: 160
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: hBox,
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: vBox,
                            margin: '0 5 0 0',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Настройки прокси-сервера',
                                    layout: vBox,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpProxy',
                                            fieldLabel: 'Адрес'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpProxyUser',
                                            fieldLabel: 'Логин'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpProxyPassword',
                                            fieldLabel: 'Пароль'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Отправка начислений',
                                    layout: vBox,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpUriUpload',
                                            fieldLabel: 'Адрес'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Загрузка оплат',
                                    layout: vBox,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpUriLoad',
                                            fieldLabel: 'Адрес'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpLoadTime',
                                            fieldLabel: 'Время загрузки'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpPayeeInn',
                                            fieldLabel: 'ИНН получателя'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'GisGmpPayeeKpp',
                                            fieldLabel: 'КПП получателя'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            flex: 1,
                            layout: vBox,
                            margin: '0 0 10 5',
                            title: 'Общие параметры',
                            items: [
                                {
                                    xtype: 'gisgmppatterngrid',
                                    flex: 1,
                                    title: 'Коды шаблонов',
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GisGmpPatternCode',
                                    fieldLabel: 'Стандартный код шаблона',
                                    minLength: 0,
                                    maxLength: 26,
                                    allowBlank: false,
                                    margins: '5 0 0 0'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GisGmpSystemCode',
                                    fieldLabel: 'Код системы'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Справочник шаблонов',
                    flex: 1,
                    layout: vBox,
                    items: [
                        {
                            xtype: 'gisgmppatterndictgrid',
                            flex: 1,
                            title: 'Шаблоны',
                            itemId: 'gisGmpPatternDictGrid'
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
                                {
                                     xtype: 'b4savebutton',
                                     name: 'saveBtn'
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