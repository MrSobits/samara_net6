Ext.define('B4.view.longtermprobject.specialaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.specialaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 700,
    minHeight: 300,
    title: 'Cпециальный счет',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.CreditOrg',
        'B4.view.creditorg.Grid',
        'B4.store.account.ContragentForSpecial',
        'B4.view.contragent.Grid',
        'B4.view.longtermprobject.specialaccount.OperationGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                 {
                     xtype: 'tabpanel',
                     flex: 1,
                     enableTabScroll: true,
                     layout: {
                         type: 'vbox',
                         align: 'stretch'
                     },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    allowBlank: false,
                                    maxLength: 50,
                                    name: 'Number',
                                    fieldLabel: 'Номер'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'OpenDate',
                                            fieldLabel: 'Дата открытия',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'CloseDate',
                                            fieldLabel: 'Дата закрытия'
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
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TotalIncome',
                                            fieldLabel: 'Итого по приходу',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TotalOut',
                                            fieldLabel: 'Итого по расходу',
                                            allowBlank: false
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
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Balance',
                                            fieldLabel: 'Сальдо по счету',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'LastOperationDate',
                                            fieldLabel: 'Последняя операция',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Владелец счета',
                                    name: 'AccountOwner',
                                    

                                    store: 'B4.store.account.ContragentForSpecial',
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
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Кредитная организация',
                                    name: 'CreditOrganization',
                                    

                                    store: 'B4.store.CreditOrg',
                                    editable: false,
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'specaccountopergrid',
                            columnLines: true,
                            flex: 1,
                            itemId: 'SpecAccountOperationsGrid'
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