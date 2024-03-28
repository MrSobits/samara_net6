Ext.define('B4.view.fundformatcontr.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.fundformatcontrwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.FundFormationContractType',
        'B4.form.SelectField',
        'B4.view.regoperator.Grid',
        'B4.store.RegOperator',
        'B4.view.longtermprobject.Grid',
        'B4.store.LongTermPrObject'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 700,
    maxWidth: 700,
    minHeight: 275,
    maxHeight: 275,
    bodyPadding: 5,
    title: 'Договор на формирование фонда капитального ремонта',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180,
                flex: 1
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Объект капитального ремонта',
                    name: 'LongTermPrObject',                    
                    store: 'B4.store.LongTermPrObject',
                    editable: false,
                    allowBlank: false,
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
                            text: 'Наименование',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    textProperty: 'Address'
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Региональный оператор',
                    name: 'RegOperator',                    

                    store: 'B4.store.RegOperator',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Contragent',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    textProperty: 'Contragent'
                },
                {
                    xtype: 'combobox',
                    floating: false,
                    allowBlank: false,
                    name: 'TypeContract',
                    fieldLabel: 'Тип договора',
                    displayField: 'Display',
                    store: B4.enums.FundFormationContractType.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right',
                        flex: 1
                    },
                    padding: '0 0 5 0',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContractNumber',
                            fieldLabel: 'Номер',
                            flex: 1,
                            maxLength: 100
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            width: 100,
                            name: 'ContractDate',
                            fieldLabel: 'от'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 180,
                        flex: 1,
                        labelAlign: 'right',
                        format: 'd.m.Y'
                    },
                    padding: '0 0 5 0',
                    items: [
                        {
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            allowBlank: false
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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
                            columns: 2,
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