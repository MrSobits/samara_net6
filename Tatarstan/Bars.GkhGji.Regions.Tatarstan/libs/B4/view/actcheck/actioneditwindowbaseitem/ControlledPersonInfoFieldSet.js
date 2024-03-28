Ext.define('B4.view.actcheck.actioneditwindowbaseitem.ControlledPersonInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.actcheckactioncontrolledpersoninfofieldset',

    requires: [
        'B4.view.actcheck.actioneditwindowbaseitem.IdentityDocInfoFieldSet'
    ],

    title: 'Контролируемое лицо',
    layout: 'hbox',
    items: [
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'ФИО',
                    name: 'ContrPersFio',
                    maxLength: 255
                },
                {
                    xtype: 'datefield',
                    name: 'ContrPersBirthDate',
                    fieldLabel: 'Дата рождения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Место рождения',
                    name: 'ContrPersBirthPlace',
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Адрес регистрации',
                    name: 'ContrPersRegistrationAddress',
                    maxLength: 500
                },
                {
                    xtype: 'checkbox',
                    name: 'ContrPersLivingAddressMatched',
                    boxLabel: 'Адрес проживания совпадает с адресом регистрации',
                    fieldStyle: 'vertical-align: middle;',
                    margin: '0 0 0 115',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('actcheckactioncontrolledpersoninfofieldset'),
                                livingAddressField = fieldSet.down('textfield[name=ContrPersLivingAddress]');
                            
                            if (!newValue) {
                                livingAddressField.show();
                            }
                            else {
                                livingAddressField.hide();
                                livingAddressField.setValue(null);
                            }
                        }
                    }
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес проживания',
                            name: 'ContrPersLivingAddress',
                            labelWidth: 110,
                            labelAlign: 'right',
                            maxLength: 500,
                            padding: '0 -110 0 0',
                            flex: 1
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ContrPersIsHirer',
                            boxLabel: 'Наниматель',
                            fieldStyle: 'vertical-align: middle;',
                            margin: '0 0 4 115'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Номер телефона',
                    name: 'ContrPersPhoneNumber',
                    maxLength: 50
                }
            ]
        },
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Место работы',
                    name: 'ContrPersWorkPlace',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Должность',
                    name: 'ContrPersPost',
                    maxLength: 255
                },
                {
                    xtype: 'actcheckactionidentitydocinfofieldset',
                    margin: '0 0 0 10'
                }
            ]
        }
    ]
});