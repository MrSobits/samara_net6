Ext.define('B4.view.longtermprobject.loan.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.longtermprobjectloangrid',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Учет займов',
    store: 'longtermprobject.Loan',
    itemId: 'longtermprobjectloangrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectIssued',
                    flex: 1,
                    text: 'МКД, выдавший займ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LoanAmount',
                    width: 150,
                    text: 'Сумма займа (руб.)',
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        decimalSeparator: ',',
                        hideTrigger: true,
                        minValue: 0
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateIssue',
                    width: 100,
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRepayment',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Срок погашения',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodLoan',
                    width: 150,
                    text: 'Период займа (мес.)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: false,
                        hideTrigger: true,
                        minValue: 0
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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