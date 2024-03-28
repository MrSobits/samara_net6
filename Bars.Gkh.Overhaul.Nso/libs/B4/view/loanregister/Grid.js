Ext.define('B4.view.loanregister.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.LoanRegister'
    ],

    title: 'Реестр займов',
    alias: 'widget.loanregistergrid',
    closable: true,
    store: 'LoanRegister',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.LoanRegister');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectAddress',
                    flex: 1,
                    text: 'Объкт капитального ремонта',
                    filter: { xtype: 'textfield' }
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
                    flex: 1,
                    text: 'Сумма (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        minValue: 0
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateIssue',
                    width: 150,
                    format: 'd.m.Y',
                    text: 'Дата (дата выдачи)',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRepayment',
                    width: 150,
                    format: 'd.m.Y',
                    text: 'Срок погашения',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
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
                        hideTrigger: true,
                        allowDecimals: false,
                        minValue: 0
                    }
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
                            columns: 1,
                            items: [
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});