Ext.define('B4.view.syncparams.Panel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.syncparamspanel',

    requires: [
        'B4.ux.button.Save'
    ],

    title: 'Настройка параметров интеграции с Реформой ЖКХ',

    layout: {
        type: 'vbox',
        align: 'left'
    },

    defaults: {
        labelWidth: 150,
        labelAlign: 'right',
        width: 800,
        margin: '5 0'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            items: [
                {
                    xtype: 'checkbox',
                    name: 'Enabled',
                    fieldStyle: 'vertical-align: middle; margin-left: 138px;',
                    boxLabel: 'Включена'
                },
                {
                    xtype: 'textfield',
                    name: 'RemoteAddress',
                    fieldLabel: 'Адрес сервиса'
                },
                {
                    xtype: 'textfield',
                    name: 'User',
                    fieldLabel: 'Имя пользователя'
                },
                {
                    xtype: 'textfield',
                    name: 'Password',
                    fieldLabel: 'Пароль',
                    inputType: 'password'
                },
                {
                    xtype: 'timefield',
                    name: 'IntegrationTime',
                    fieldLabel: 'Время интеграции',
                    value: '23:00',
                    minValue: '00:00',
                    maxValue: '23:00',
                    increment: 30,
                    format: 'H:i'
                },
                {
                    xtype: 'checkbox',
                    name: 'NullIsNotData',
                    fieldStyle: 'vertical-align: middle; margin-left: 138px;',
                    boxLabel: 'Пустота - это не данные'
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
                                    xtype: 'button',
                                    icon: B4.Url.content('content/img/icons/cog_start.png'),
                                    text: 'Запустить интеграцию',
                                    name: 'StartIntegration'
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