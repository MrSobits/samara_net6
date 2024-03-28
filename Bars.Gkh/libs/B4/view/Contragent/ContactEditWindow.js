Ext.define('B4.view.contragent.ContactEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    minWidth: 650,
    minHeight: 400,
    maxHeight: 500,
    itemId: 'contragentContactEditWindow',
    title: 'Контакт',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.Gender',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.dict.Position',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.contragent.ContactCasesPanel'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'tabpanel',
                    padding: 0,
                    border: false,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    frame: true,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Основная информация',
                            padding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                allowBlank: false
                            },
                            
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Surname',
                                    fieldLabel: 'Фамилия',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Имя',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Patronymic',
                                    fieldLabel: 'Отчество',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'Position',
                                            fieldLabel: 'Должность',
                                            store: 'B4.store.dict.Position',
                                            labelAlign: 'right',
                                            labelWidth: 130,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Snils',
                                            fieldLabel: 'СНИЛС',
                                            labelAlign: 'right',
                                            labelWidth: 150,
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'Gender',
                                            enumName: 'B4.enums.Gender',
                                            fieldLabel: 'Пол',
                                            labelAlign: 'right',
                                            labelWidth: 130,
                                            value: 0,
                                            includeEmpty: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'BirthDate',
                                            fieldLabel: 'Дата рождения',
                                            format: 'd.m.Y',
                                            labelAlign: 'right',
                                            labelWidth: 150,
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'FLDocSeries',
                                            allowBlank: true,
                                            labelAlign: 'right',
                                            flex: 1,
                                            labelWidth: 130,
                                            fieldLabel: 'Серия паспорта',
                                            maxLength: 10
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FLDocNumber',
                                            allowBlank: true,
                                            labelAlign: 'right',
                                            flex: 1,
                                            labelWidth: 150,
                                            fieldLabel: 'Номер паспорта',
                                            maxLength: 10
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'FLDocIssuedDate',
                                            fieldLabel: 'Дата выдачи',
                                            allowBlank: false,
                                            flex: 1,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FLDocIssuedBy',
                                            fieldLabel: 'Кем выдан',
                                            labelAlign: 'right',
                                            labelWidth: 80,
                                            flex: 1.5
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'DateStartWork',
                                            fieldLabel: 'Дата начала работы',
                                            allowBlank: false,
                                            labelWidth: 130
                                        },
                                        {
                                            name: 'DateEndWork',
                                            fieldLabel: 'Дата окончания работы',
                                            labelWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 0 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'textfield',
                                        maxLength: 50,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'Phone',
                                            fieldLabel: 'Телефон',
                                            labelWidth: 130
                                        },
                                        {
                                            name: 'Email',
                                            fieldLabel: 'E-mail',
                                            regex: /^([\w\-\'\-]+)(\.[\w\'\-]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/,
                                            labelWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 120
                                    },
                                    title: 'Сведения о приказе',
                                    items: [
                                        {
                                            xtype: 'container',
                                            anchor: '100%',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OrderNum',
                                                    fieldLabel: 'Номер приказа',
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'OrderDate',
                                                    fieldLabel: 'Дата приказа',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'OrderName',
                                            fieldLabel: 'Документ приказа',
                                            labelAlign: 'right',
                                            maxLength: 100,
                                            anchor: '50%'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'contragentContactCasesPanel',
                            border: false
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
                                { xtype: 'b4savebutton' }
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