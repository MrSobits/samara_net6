Ext.define('B4.view.person.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.personEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.TypeIdentityDocument',
        'B4.view.person.QualificationGrid',
        'B4.ux.button.Save',
        'B4.view.Control.GkhTriggerField',
        'B4.view.person.RequestToExamGrid'
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                falign: 'stretch',
                labelAlign: 'right'
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    itemId: 'btnPrint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-laptop',
                                    itemId: 'btnMVD',
                                    text: 'Запрос в МВД'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'textfield',
                    name: 'Surname',
                    fieldLabel: 'Фамилия',
                    allowBlank: false,
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Имя',
                    allowBlank: false,
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    name: 'Patronymic',
                    fieldLabel: 'Отчество',
                    allowBlank: false,
                    maxLength: 100
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Email',
                            fieldLabel: 'E-mail',
                            vtype: 'email',
                            maxLength: 200,
                            itemId: 'tfEmail'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Phone',
                            fieldLabel: 'Телефон',
                            maxLength: 200
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'AddressReg',
                    fieldLabel: 'Адрес регистрации',
                    maxLength: 2000
                },
                {
                    xtype: 'textfield',
                    name: 'AddressLive',
                    fieldLabel: 'Адрес места жительства',
                    maxLength: 2000
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'AddressBirth',
                            fieldLabel: 'Адрес места рождения',
                            allowBlank: false,
                            maxLength: 2000
                        },
                        {
                            xtype: 'datefield',
                            name: 'Birthdate',
                            fieldLabel: 'Дата рождения',
                            allowBlank: false,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Inn',
                    fieldLabel: 'ИНН',
                    maxLength: 20
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Документ, удостоверяющий личность',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    name: 'TypeIdentityDocument',
                                    fieldLabel: 'Тип документа',
                                    displayField: 'Display',
                                    store: B4.enums.TypeIdentityDocument.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'IdIssuedDate',
                                    fieldLabel: 'Дата выдачи',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults:
                            {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                maxLength: 10
                            },
                            items: [
                                {
                                    name: 'IdSerial',
                                    fieldLabel: 'Серия'
                                },
                                {
                                    name: 'IdNumber',
                                    fieldLabel: 'Номер'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'IdIssuedBy',
                            fieldLabel: 'Кем выдан',
                            maxLength: 2000
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    layout: 'vbox',
                    border: false,
                    margins: -1,
                    height: 500,
                    flex: 1,
                    items: [
                        {
                            xtype: 'personqualificationgrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            flex: 1
                        },
                        {
                            xtype: 'personrequesttoexamgrid',
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});