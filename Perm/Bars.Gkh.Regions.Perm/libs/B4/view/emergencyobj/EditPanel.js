Ext.define('B4.view.emergencyobj.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'emergencyObjEditPanel',
    title: 'Сведения по дому',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject',
        'B4.store.dict.ResettlementProgram',
        'B4.store.dict.FurtherUse',
        'B4.store.dict.ReasonInexpedient',
        'B4.ux.button.Save',
        'B4.enums.ConditionHouse'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 230,
                        labelAlign: 'right'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.RealityObject',
                                            textProperty: 'Address',
                                            allowBlank: false,
                                            editable: false,
                                            columns: [
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
                                                {
                                                    text: 'Адрес',
                                                    dataIndex: 'Address',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            name: 'RealityObject',
                                            fieldLabel: 'Жилой дом',
                                            anchor: '100%',
                                            itemId: 'sfRealityObjectEmerObj'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: .5,
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'ConditionHouse',
                                            fieldLabel: 'Состояние дома',
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            store: B4.enums.ConditionHouse.getStore(),
                                            itemId: 'cbConditionHouseEmerObj'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            store: 'B4.store.dict.ResettlementProgram',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ],
                            name: 'ResettlementProgram',
                            fieldLabel: 'Программа переселения',
                            anchor: '100%',
                            itemId: 'sfResettlementProgEmerObj'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            store: 'B4.store.dict.ReasonInexpedient',
                                            name: 'ReasonInexpedient',
                                            fieldLabel: 'Основание нецелесообразности',
                                            itemId: 'sfReasInexpedientEmerObj'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: .5,
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'checkboxfield',
                                            name: 'IsRepairExpedient',
                                            fieldLabel: 'Ремонт целесообразен',
                                            itemId: 'cbIsRepairExpedientEmerObj'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            anchor: '100%',
                            name: 'ExemptionsBasis',
                            fieldLabel: 'Основание изъятия',
                            maxLength: 300
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 230,
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты документа, подтверждающие признание МКД аварийным',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        align: 'stretch',
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            defaults: {
                                                labelWidth: 220,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'EmergencyDocumentName',
                                                    anchor: '100%',
                                                    fieldLabel: 'Наименование документа',
                                                    itemId: 'tfEmergencyDocumentName',
                                                    readOnly: true,
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    anchor: '100%',
                                                    name: 'EmergencyFileInfo',
                                                    fieldLabel: 'Документ',
                                                    itemId: 'ffEmergencyFileInfo',
                                                    onTrigger1Click: function () {},
                                                    onTrigger3Click: function () {}
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: .5,
                                            defaults: {
                                                labelWidth: 230,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    width: 350,
                                                    name: 'EmergencyDocumentDate',
                                                    fieldLabel: 'Дата документа',
                                                    format: 'd.m.Y',
                                                    readOnly: true,
                                                    itemId: 'dfEmergencyDocumentDate'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'EmergencyDocumentNumber',
                                                    width: 350,
                                                    fieldLabel: 'Номер',
                                                    itemId: 'tfEmergencyDocumentNumber',
                                                    maxLength: 300
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            anchor: '100%',
                            editable: false,
                            store: 'B4.store.dict.FurtherUse',
                            name: 'FurtherUse',
                            fieldLabel: 'Дальнейшее использование',
                            itemId: 'sfFurtherUseEmerObj'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 280,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    title: 'Планируемые сроки',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'ResettlementDate',
                                            fieldLabel: 'Дата завершения переселения',
                                            format: 'd.m.Y',
                                            itemId: 'dfResettlementDateEmerObj'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DemolitionDate',
                                            fieldLabel: 'Дата сноса/реконструкции МКД',
                                            format: 'd.m.Y',
                                            itemId: 'dfDemolitionDateEmerObj'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 280,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    title: 'Фактические сроки',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'FactResettlementDate',
                                            fieldLabel: 'Дата завершения переселения',
                                            format: 'd.m.Y',
                                            itemId: 'dfFactResettlementDateEmerObj'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'FactDemolitionDate',
                                            fieldLabel: 'Дата сноса/реконструкции МКД',
                                            format: 'd.m.Y',
                                            itemId: 'dfFactDemolitionDateEmerObj'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 230,
                                labelAlign: 'right'
                            },
                            title: 'Показатели',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        align: 'stretch',
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            defaults: {
                                                labelWidth: 280,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'LandArea',
                                                    fieldLabel: 'Площадь земельного участка',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    allowDecimals: true,
                                                    minValue: 0,
                                                    decimalSeparator: ',',
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    itemId: 'nfLandAreaEmerObj'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'TotalArea',
                                                    fieldLabel: 'Общая площадь жилых помещений',
                                                    readOnly: true,
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    itemId: 'nfTotalAreaEmerObj'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'ResettlementFlatArea',
                                                    fieldLabel: 'Площадь расселяемых жилых помещений',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    allowDecimals: true,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    decimalSeparator: ',',
                                                    itemId: 'nfResettlementFlatAreaEmerObj'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            defaults: {
                                                labelWidth: 290,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CadastralNumber',
                                                    fieldLabel: 'Кадастровый номер земельного участка',
                                                    itemId: 'tfCadastralNumberEmerObj',
                                                    readOnly: true,
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'InhabitantNumber',
                                                    fieldLabel: 'Число жителей планируемых к переселению',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    allowDecimals: false,
                                                    mouseWheelEnabled: false,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    itemId: 'nfInhabitantNumberEmerObj'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'ResettlementFlatAmount',
                                                    fieldLabel: 'Количество расселяемых жилых помещений',
                                                    hideTrigger: true,
                                                    minValue: 0,
                                                    allowDecimals: false,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    itemId: 'nfRessetlementAmountEmerObj'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 220,
                                labelAlign: 'right'
                            },
                            title: 'Документ',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        align: 'stretch',
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            defaults: {
                                                labelWidth: 220,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentName',
                                                    anchor: '100%',
                                                    fieldLabel: 'Наименование документа',
                                                    itemId: 'tfDocumentName',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    anchor: '100%',
                                                    name: 'FileInfo',
                                                    fieldLabel: 'Документ',
                                                    itemId: 'ffFileInfo'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: .5,
                                            defaults: {
                                                labelWidth: 230,
                                                labelAlign: 'right',
                                                anchor: '100%'
                                            },
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    width: 350,
                                                    name: 'DocumentDate',
                                                    fieldLabel: 'Дата документа',
                                                    format: 'd.m.Y',
                                                    itemId: 'dfDocumentDate'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumber',
                                                    width: 350,
                                                    fieldLabel: 'Номер',
                                                    itemId: 'tfDocumentNumber',
                                                    maxLength: 300
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    anchor: '100%',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    itemId: 'taDescription',
                                    maxLength: 300
                                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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