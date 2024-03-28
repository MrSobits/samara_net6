Ext.define('B4.view.person.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.personaddwindow',
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 800,
    minWidth: 700,
    minHeight: 455,
    maxHeight: 455,
    bodyPadding: 5,
    itemId: 'personAddWindow',
    title: 'Должностное лицо',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.TypeIdentityDocument',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.store.dict.Position'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
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
                    xtype: 'textfield',
                    name: 'Inn',
                    fieldLabel: 'ИНН',
                    maxLength: 20
                },
                {
                    xtype: 'hiddenfield',
                    name: 'Email'
                },
                {
                    xtype: 'hiddenfield',
                    name: 'Phone'
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
                            padding: '0 5 5 0',
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
                            padding: '0 5 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'IdSerial',
                                    fieldLabel: 'Серия',
                                    maxLength: 10
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'IdNumber',
                                    fieldLabel: 'Номер',
                                    maxLength: 10
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
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: true,
                        anchor: '100%'
                    },
                    title: 'Место работы',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.Contragent',
                            allowBlank: true,
                            editable: false,
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
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'StartDate',
                                    fieldLabel: 'Дата начала',
                                    allowBlank: true,
                                    format: 'd.m.Y',
                                    maxValue: new Date()
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'EndDate',
                                    fieldLabel: 'Дата окончания',
                                    format: 'd.m.Y',
                                    minValue: new Date()
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Position',
                            fieldLabel: 'Должность',
                            store: 'B4.store.dict.Position',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Выбрать из контактов контрагентов',
                                    iconCls: 'icon-add',
                                    actionName: 'addContact'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton'
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