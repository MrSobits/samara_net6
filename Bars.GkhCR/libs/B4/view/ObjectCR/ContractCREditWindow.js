Ext.define('B4.view.objectcr.ContractCrEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minWidth: 600,
    maxWidth: 700,
    minHeight: 400,
    bodyPadding: 5,
    
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.view.objectcr.BuildContractTypeWorkGrid',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.store.dict.FinanceSource',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNo'
    ],

    title: 'Договор на услуги',
    alias: 'widget.objectcrcontractwin',
    

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 160,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    allowBlank: false,
                    maxLength: 300,
                    itemId: 'tfDocumentName'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 300,
                            itemId: 'tfDocumentNum'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateFrom',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            itemId: 'dfDateFrom'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield', editable: false,
                    name: 'File',
                    fieldLabel: 'Файл',
                    itemId: 'ffFile'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Customer',
                    fieldLabel: 'Заказчик',
                    store: 'B4.store.Contragent',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Contragent',
                    fieldLabel: 'Подрядная организация',
                    store: 'B4.store.Contragent',
                    itemId: 'sflContragent',
                    columns: [
                          { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Тип договора',
                            name: 'TypeContractObject',
                            itemId: 'cbTypeContractObject',
                            store: Ext.create('B4.base.Store', {
                                autoLoad: true,
                                fields: ['Id', 'Key', 'Value'],
                                proxy: {
                                    type: 'b4proxy',
                                    controllerName: 'MultipurposeGlossaryItem',
                                    listAction: 'ListByGlossaryCode?glossaryCode=cr_contract_type'
                                }
                            }),
                            valueField: 'Id',
                            displayField: 'Value',
                            editable: false
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'SumContract',
                            fieldLabel: 'Сумма договора (руб.)',
                            itemId: 'nfSumContract'
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
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'StartSumContract',
                            fieldLabel: 'Начальная цена контракта (руб.)',
                            itemId: 'nfSumContract'
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
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStartWork',
                            itemId: 'sfDateStartWork',
                            fieldLabel: 'Дата начала работ',
                            format: 'd.m.Y',
                            listeners: {
                                change: function() {
                                    var me = this;
                                    me.up().down('datefield[name = DateEndWork]').minValue = me.value;
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEndWork',
                            itemId: 'sfDateEndWork',
                            fieldLabel: 'Дата окончания работ',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'FinanceSource',
                    fieldLabel: 'Разрез финансирования',
                    store: 'B4.store.dict.FinanceSource',
                    itemId: 'sflFinanceSource'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 160,
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
                        labelWidth: 160,
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
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 2000,
                    flex: 1,
                    itemId: 'taDescription'
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Выводить документ на портал',
                    name: 'UsedInExport',
                    store: B4.enums.YesNo.getStore(),
                    displayField: 'Display',
                    valueField: 'Value'
                },
                {
                    xtype: 'contrcrtypewrkgrid',
                    height: 200
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
                            columns: 2,
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