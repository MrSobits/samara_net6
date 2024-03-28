Ext.define('B4.view.regoperator.calcaccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.regopcalcaccountgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.calcaccount.Regop'
    ],

    title: 'Счета регионального оператора',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.calcaccount.Regop');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'ContragentAccountNumber',
                    width: 160,
                    text: 'Номер счета',
                    filter: {
                        xtype: 'textfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    dataIndex: 'ContragentCreditOrg',
                    flex: 1,
                    text: 'Кредитная организация',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOpen',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата открытия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateClose',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата закрытия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,000.00',
                    dataIndex: 'Debt',
                    width: 100,
                    text: 'Итого по дебету',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,000.00',
                    dataIndex: 'Credit',
                    width: 100,
                    text: 'Итого по кредиту',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,000.00',
                    dataIndex: 'Saldo',
                    width: 140,
                    text: 'Сальдо',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});