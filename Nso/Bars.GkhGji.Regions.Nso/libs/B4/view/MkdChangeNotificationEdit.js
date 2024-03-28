Ext.define('B4.view.MkdChangeNotificationEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.mkdchangenotificationedit',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.view.MkdChangeNotificationFileGrid',
        'B4.form.SelectField',
        'B4.form.FiasSelectCustomAddress',

        'B4.store.dict.NotificationCause',
        'B4.store.dict.MkdManagementMethod',
        'B4.store.ManagingOrganization'
    ],

    modal: true,
    title: 'Уведомление о смене способа управления МКД',
    border: false,
    width: 800,
    height: 600,
    closeAction: 'destroy',
    autoScroll: true,
    bodyPadding: 5,
    defaults: {
        labelWidth: 200,
        labelAlign: 'right'
    },
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Уведомление',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Регистрационный номер дела',
                                    name: 'RegistrationNumber',
                                    readOnly: true
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата',
                                    name: 'Date',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'b4fiasselectcustomaddress',
                            name: 'FiasAddress',
                            allowBlank: false,
                            fieldLabel: 'Адрес',
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.NotificationCause',
                            textProperty: 'Name',
                            name: 'NotificationCause',
                            anchor: '100%',
                            fieldLabel: 'Причина уведомления',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Отметка о принятии ГЖИ',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Входящий номер',
                                    name: 'InboundNumber',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата регистрации уведомления',
                                    name: 'RegistrationDate',
                                    allowBlank: false
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Предыдущий способ управления',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.MkdManagementMethod',
                            textProperty: 'Name',
                            name: 'OldMkdManagementMethod',
                            anchor: '100%',
                            fieldLabel: 'Наименование',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'OldManagingOrganization',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.ManagingOrganization',
                            textProperty: 'ContragentName',
                            editable: false,
                            columns: [
                                { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ИНН',
                                    name: 'OldInn'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОГРН',
                                    name: 'OldOgrn'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Новый способ управления',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.MkdManagementMethod',
                            textProperty: 'Name',
                            name: 'NewMkdManagementMethod',
                            anchor: '100%',
                            fieldLabel: 'Наименование',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'NewManagingOrganization',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.ManagingOrganization',
                            textProperty: 'ContragentName',
                            editable: false,
                            columns: [
                                { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ИНН',
                                    name: 'NewInn'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОГРН',
                                    name: 'NewOgrn'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Юридический адрес',
                            name: 'NewJuridicalAddress'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Руководитель',
                            name: 'NewManager'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Телефон',
                            name: 'NewPhone'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'E-mail',
                            name: 'NewEmail',
                            vtype: 'email'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Официальный сайт',
                            name: 'NewOfficialSite'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата предоставления копии акта',
                            name: 'NewActCopyDate'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    height: 300,
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Приложения',
                    items: [
                        {
                            xtype: 'mkdchangenotificationfilegrid',
                            flex: 1
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'StateButton',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
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