Ext.define('B4.view.utilityclaimwork.ExecProcEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.utilityexecproceditpanel',
    title: 'Исполнительное производство',
    frame: true,
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton',
        'B4.view.utilityclaimwork.ExecProcDocumentGrid',
        'B4.enums.TerminationReasonType',
        'B4.store.dict.JurInstitution',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.enums.OwnerType',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'fieldset',
                title: 'Исполнительное производство',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    xtype: 'textfield',
                    labelWidth: 200,
                    labelAlign: 'right',
                    padding: '5 8 0 0'
                },
                items: [
                    {
                        xtype: 'container',
                        layout: 'hbox',
                        items: [
                            {
                                xtype: 'container',
                                flex: 1,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    labelAlign: 'right'
                                },
                                items: [
                                    {
                                        xtype: 'b4selectfield',
                                        store: 'B4.store.dict.JurInstitution',
                                        textProperty: 'Name',
                                        editable: false,
                                        columns: [
                                            {
                                                text: 'Наименование',
                                                dataIndex: 'Name',
                                                flex: 1,
                                                filter: { xtype: 'textfield' }
                                            }
                                        ],
                                        name: 'JurInstitution',
                                        fieldLabel: 'Подразделение ОСП'
                                    },
                                    {
                                        name: 'RegistrationNumber',
                                        fieldLabel: 'Регистрационный номер',
                                        allowBlank: false
                                    },
                                    {
                                        name: 'Document',
                                        fieldLabel: 'Документ'
                                    },
                                    {
                                        name: 'DocumentNumber',
                                        fieldLabel: 'Номер документа'
                                    },
                                    {
                                        xtype: 'b4filefield',
                                        name: 'File',
                                        fieldLabel: 'Файл'
                                    },
                                    {
                                        xtype: 'gkhdecimalfield',
                                        name: 'PaidSum',
                                        fieldLabel: 'Сумма погашения в рамках производствa'
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                flex: 1,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    labelAlign: 'right'
                                },
                                items: [
                                    {
                                        xtype: 'datefield',
                                        name: 'DateStart',
                                        fieldLabel: 'Дата возбуждения',
                                        format: 'd.m.Y'
                                    },
                                    {
                                        xtype: 'gkhdecimalfield',
                                        name: 'DebtSum',
                                        fieldLabel: 'Сумма для погашения'
                                    },
                                    {
                                        xtype: 'datefield',
                                        name: 'DocumentDate',
                                        fieldLabel: 'Дата документа',
                                        format: 'd.m.Y'
                                    },
                                    {
                                        xtype: 'checkbox',
                                        name: 'IsTerminated',
                                        fieldLabel: 'Производство прекращено'
                                    },
                                    {
                                        xtype: 'b4enumcombo',
                                        name: 'TerminationReasonType',
                                        fieldLabel: 'Причина прекращения',
                                        enumName: 'B4.enums.TerminationReasonType'
                                    },
                                    {
                                        xtype: 'datefield',
                                        name: 'DateEnd',
                                        fieldLabel: 'Дата прекращения',
                                        format: 'd.m.Y'
                                    }
                                ]
                                }
                            ]
                        },               
                        {
                            name: 'Notation',
                            fieldLabel: 'Примечание'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Взыскатель',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        padding: '5 8 0 0'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 200,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'textfield',
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            name: 'Creditor',
                                            fieldLabel: 'Взыскатель'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.RealityObject',
                                            textProperty: 'Address',
                                            editable: false,
                                            columns: [
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
                                                {
                                                    text: 'Адрес',
                                                    dataIndex: 'Address',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            name: 'LegalOwnerRealityObject',
                                            fieldLabel: 'Адрес юр.лица',
                                            labelAlign: 'right'
                                        }
                                    ]
                                },                           
                                {
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    maskRe: /\d/
                                }
                            ]
                        }                       
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Должник',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 200,
                        labelAlign: 'right',
                        padding: '5 8 0 0'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 200,
                                labelAlign: 'right',
                                flex: 1
                            },                          
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'textfield',
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            name: 'AccountOwner',
                                            fieldLabel: 'Абонент'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.RealityObject',
                                            textProperty: 'Address',
                                            editable: false,
                                            columns: [
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
                                                {
                                                    text: 'Адрес',
                                                    dataIndex: 'Address',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            name: 'RealityObject',
                                            fieldLabel: 'Адреc',
                                            labelAlign: 'right',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип абонента',
                                    enumName: 'B4.enums.OwnerType',
                                    name: 'OwnerType'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Нормативные документы',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        padding: '5 8 0 0'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 200,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'Clause',
                                    fieldLabel: 'Статья'
                                },
                                {
                                    name: 'Paragraph',
                                    fieldLabel: 'Пункт'
                                },

                                {
                                    name: 'Subparagraph',
                                    fieldLabel: 'Подпункт'
                                }
                            ]
                        }
                    ]
                },      
                {
                    xtype: 'execprocdocumentgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    type: 'mainForm'
                                },                           
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    text: 'Удалить',
                                    textAlign: 'left'
                                },
                                {
                                    xtype: 'acceptmenubutton'
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'State',
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