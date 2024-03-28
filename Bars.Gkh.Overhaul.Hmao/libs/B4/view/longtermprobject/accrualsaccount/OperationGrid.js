Ext.define('B4.view.longtermprobject.accrualsaccount.OperationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.accrualaccountopergrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Операции по счету начислений',
    store: 'account.operation.Accruals',

    initComponent: function() {
        var me = this,
            numberfield = {
                xtype: 'numberfield',
                hideTrigger: true,
                decimalSeparator: ',',
                allowDecimals: true
            },
            numberRenderer = function(val) {
                return val ? Ext.util.Format.currency(val) : '';
            };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AccrualDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalDebit',
                    flex: 1,
                    text: 'Итого по приходу',
                    editor: numberfield,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalCredit',
                    flex: 1,
                    text: 'Итого по расходу',
                    editor: numberfield,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OpeningBalance',
                    flex: 1,
                    text: 'Входящий баланс',
                    editor: numberfield,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ClosingBalance',
                    flex: 1,
                    text: 'Исходящий баланс',
                    editor: numberfield,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnSave',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});