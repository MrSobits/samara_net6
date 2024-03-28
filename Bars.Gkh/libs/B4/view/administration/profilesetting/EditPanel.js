Ext.define('B4.view.administration.profilesetting.EditPanel', {
    extend: 'Ext.form.Panel',
    requires: ['B4.ux.button.Save', 'B4.enums.OperatorExportFormat'],
    closable: true,
    layout: 'vbox',
    bodyPadding: 5,
    alias: 'widget.profileSettingEditPanel',
    title: 'Профиль',
    bodyStyle: Gkh.bodyStyle,
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
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
                        }
                    ]
                }
            ],
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype:'fieldset',
                    title: 'Личные данные',
                    defaults: {
                        labelWidth: 200,
                        width: 600
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Имя пользователя',
                            labelAlign: 'right',
                            readOnly: true,
                            maxLength: 1000
                        },
                        {
                            xtype: 'textfield',
                            name: 'Login',
                            fieldLabel: 'Логин',
                            labelAlign: 'right',
                            readOnly: true,
                            maxLength: 100
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Формат выгрузки отчетов',
                            store: B4.enums.OperatorExportFormat.getStore(),
                            displayField: 'Display',
                            labelAlign: 'right',
                            valueField: 'Value',
                            name: 'ExportFormat',

                        }
                    ]
                },
                {
                    xtype:'fieldset',
                    title: 'Настройки пароля',
                    defaults: {
                        labelWidth: 200,
                        width: 600
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            id: 'pass',
                            name: 'Password',
                            labelAlign: 'right',
                            fieldLabel: 'Пароль',
                            inputType: 'password',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            id: 'newPass',
                            name: 'NewPassword',
                            labelAlign: 'right',
                            fieldLabel: 'Новый пароль',
                            inputType: 'password',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            initialPassField: 'newPass',
                            name: 'NewPasswordCommit',
                            labelAlign: 'right',
                            fieldLabel: 'Подтверждение нового пароля',
                            inputType: 'password',
                            vtype: 'password',
                            msgTarget: 'side',
                            autoFitErrors: false,
                            maxLength: 100
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Контактные данные',
                    defaults: {
                        labelWidth: 200,
                        width: 600
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Email',
                            fieldLabel: 'E-mail',
                            labelAlign: 'right',
                            vtype: 'email',
                            maxLength: 250
                        },
                        {
                            xtype: 'textfield',
                            name: 'Phone',
                            fieldLabel: 'Телефон',
                            labelAlign: 'right',
                            maxLength: 50
                        }
                    ]
                }
            ]
        });

        Ext.apply(Ext.form.field.VTypes, {

            password: function (val, field) {
                if (field.initialPassField) {
                    var pwd = Ext.getCmp('newPass');
                    return (val == pwd.getValue());
                }
                return true;
            },

            passwordText: 'Пароли не совпадают'
        });

        me.callParent(arguments);
    }
});