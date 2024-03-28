Ext.define('B4.view.businessactivity.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    height: 510,
    width: 700,
    maxWidth: 1200,
    bodyPadding: 5,
    itemId: 'businessActivityEditWindow',
    title: 'Уведомление о начале осуществления предпринимательской деятельности',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhButtonPrint',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.businessactivity.ServiceJuridicalGjiGrid',
        
        'B4.enums.TypeKindActivity'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    itemId: 'businessActivityPanel',
                    border: false,
                    region: 'center',
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Общие сведения',
                            border: false,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Общие сведения',
                                    anchor: '100%',
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            anchor: '100%',
                                            padding: '0 0 5 0',
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'Contragent',
                                                    textProperty: 'ShortName',
                                                    itemId: 'sfContragent',
                                                    fieldLabel: 'Контрагент',
                                                    store: 'B4.store.Contragent',
                                                    allowBlank: false,
                                                    flex: 1,
                                                    labelWidth: 190,
                                                    labelAlign: 'right',
                                                    columns: [
                                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                        {
                                                            text: 'Муниципальное образование',
                                                            dataIndex: 'Municipality',
                                                            flex: 1,
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
                                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'button',
                                                    width: 90,
                                                    name: 'ContragentEditButton',
                                                    itemId: 'btnEditContragent',
                                                    text: 'Редактировать',
                                                    margin: '0 0 0 10'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            name: 'OrganizationFormName',
                                            itemId: 'cbOrganizationFormName',
                                            fieldLabel: 'Организационно-правовая форма',
                                            readOnly: true,
                                            hideTrigger: true,
                                            fields: ['Id', 'Name', 'Code'],
                                            url: '/OrganizationForm/List',
                                            queryMode: 'local'
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                readOnly: true,
                                                labelAlign: 'right',
                                                xtype: 'textfield',
                                                flex: 1,
                                                labelWidth: 190
                                            },
                                            items: [
                                                {
                                                    name: 'OGRN',
                                                    itemId: 'tfOgrn',
                                                    fieldLabel: 'ОГРН'
                                                },
                                                {
                                                    name: 'INN',
                                                    itemId: 'tfInn',
                                                    fieldLabel: 'ИНН'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'MailingAddress',
                                            itemId: 'tfMailingAddress',
                                            fieldLabel: 'Почтовый адрес',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'combobox', editable: false,
                                            name: 'TypeKindActivity',
                                            itemId: 'cbTypeKindActivity',
                                            fieldLabel: 'Вид деятельности',
                                            displayField: 'Display',
                                            store: B4.enums.TypeKindActivity.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            itemId: 'ffFile',
                                            fieldLabel: 'Файл',
                                            anchor: '100%',
                                            editable: false
                                        },
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                labelAlign: 'right',
                                                allowBlank: false,
                                                flex: 1,
                                                labelWidth: 190
                                            },
                                            items: [
                                                {
                                                    name: 'DateBegin',
                                                    itemId: 'dfDateBegin',
                                                    fieldLabel: 'Дата начала деятельности'
                                                },
                                                {
                                                    name: 'DateNotification',
                                                    itemId: 'dfDateNotif',
                                                    fieldLabel: 'Дата уведомления'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'checkboxfield',
                                            name: 'IsNotBuisnes',
                                            itemId: 'chbNotBuisnes',
                                            margin: '0 0 0 195px',
                                            boxLabel: 'Не осуществляет предпринимательскую деятельность'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Сведения о регистрации уведомления',
                                    anchor: '100%',
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'AcceptedOrganization',
                                            itemId: 'tfAcceptedOrganization',
                                            fieldLabel: 'Орган принявший уведомление',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 190,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegNum',
                                                    itemId: 'tfRegNum',
                                                    readOnly: true,
                                                    fieldLabel: 'Регистрационный номер',
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'IncomingNotificationNum',
                                                    itemId: 'tfIncomingNotificationNum',
                                                    fieldLabel: 'Входящий номер уведомления',
                                                    maxLength: 300
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 190,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateRegistration',
                                                    itemId: 'dfDateRegistration',
                                                    format: 'd.m.Y',
                                                    width: 290,
                                                    fieldLabel: 'Дата регистрации'
                                                },
                                                {
                                                    xtype: 'checkboxfield',
                                                    name: 'IsOriginal',
                                                    itemId: 'chbIsOriginal',
                                                    fieldLabel: 'Оригинал'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'serviceJuridicalGjiGrid'
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Отменить',
                                    tooltip: 'Закрыть'
                                },
                                {
                                    xtype: 'button',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить',
                                    textAlign: 'left',
                                    itemId: 'btnUpdate'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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