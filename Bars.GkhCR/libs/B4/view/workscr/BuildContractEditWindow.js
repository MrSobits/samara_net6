Ext.define('B4.view.workscr.BuildContractEditWindow', {
    extend: 'B4.form.Window',
    
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.store.Builder',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeContractBuild'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 800,
    height: 650,
    autoScroll: true,

    title: 'Договор подряда',
    alias: 'widget.workscrbuildcontractwin',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Договор подряда',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentName',
                                    fieldLabel: 'Документ',
                                    allowBlank: false,
                                    itemId: 'tfDocumentName',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelWidth: 190,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNum',
                                            fieldLabel: 'Номер',
                                            itemId: 'tfDocumentNum',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDateFrom',
                                            fieldLabel: 'от',
                                            format: 'd.m.Y',
                                            itemId: 'tfDocumentDateFrom'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'DocumentFile',
                                    fieldLabel: 'Файл',
                                    itemId: 'ffDocumentFile'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Builder',
                                    fieldLabel: 'Подрядчик',

                                    store: 'B4.store.Builder',
                                    textProperty: 'ContragentName',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false,
                                    allowBlank: false,
                                    itemId: 'sfBuilder'
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 190,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            fieldLabel: 'Тип договора КР',
                                            store: B4.enums.TypeContractBuild.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'TypeContractBuild',
                                            itemId: 'cbbxTypeContractBuild'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Sum',
                                            fieldLabel: 'Сумма',
                                            itemId: 'dcfSum'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'BudgetMo',
                                            fieldLabel: 'Бюджет МО',
                                            maxLength: 300,
                                            itemId: 'tfBudgetMo'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'BudgetSubject',
                                            fieldLabel: 'Бюджет субъекта',
                                            maxLength: 300,
                                            itemId: 'tfBudgetSubject'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 5 0',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'OwnerMeans',
                                            fieldLabel: 'Средства собственников',
                                            maxLength: 300,
                                            itemId: 'tfOwnerMeans'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FundMeans',
                                            fieldLabel: 'Средства фонда',
                                            maxLength: 300,
                                            itemId: 'tfFundMeans'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Inspector',
                                    fieldLabel: 'Инспектор',
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    editable: false,
                                    columns: [
                                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    itemId: 'sfInspector'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'anchor'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelWidth: 190,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateStartWork',
                                                    fieldLabel: 'Дата начала работ',
                                                    format: 'd.m.Y',
                                                    itemId: 'dfDateStartWork'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateEndWork',
                                                    fieldLabel: 'Дата окончания работ',
                                                    format: 'd.m.Y',
                                                    itemId: 'dfDateEndWork'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelWidth: 190,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateInGjiRegister',
                                                    fieldLabel: 'Договор внесен в реестр ГЖИ',
                                                    format: 'd.m.Y',
                                                    itemId: 'dfDateInGjiRegister'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateCancelReg',
                                                    fieldLabel: 'Отклонено от регистрации',
                                                    format: 'd.m.Y',
                                                    itemId: 'dfDateCancelReg'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateAcceptOnReg',
                                            fieldLabel: 'Принято на регистрацию, но еще не зарегистрировано',
                                            format: 'd.m.Y',
                                            itemId: 'dfDateAcceptOnReg',
                                            labelWidth: 190,
                                            labelAlign: 'right',
                                            anchor: '50%'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    itemId: 'taDescription',
                                    maxLength: 500,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            layout: 'anchor',
                            title: 'Результат квалификационного отбора',
                            itemId: 'tabResultQual',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            defaults: {
                                labelWidth: 230,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ProtocolName',
                                    fieldLabel: 'Протокол квалификационного отбора',
                                    itemId: 'tfProtocolName',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolNum',
                                            fieldLabel: 'Номер протокола',
                                            itemId: 'tfProtocolNum',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ProtocolDateFrom',
                                            fieldLabel: 'от',
                                            format: 'd.m.Y',
                                            labelWidth: 50,
                                            maxWidth: 150,
                                            itemId: 'tfProtocolDateFrom'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'ProtocolFile',
                                    fieldLabel: 'Файл',
                                    itemId: 'tfProtocolFile'
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
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
                        },
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