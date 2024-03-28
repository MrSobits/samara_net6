Ext.define('B4.view.longtermprobject.accrualsaccount.AccountsAggregationGrid', {
    extend: 'B4.ux.grid.Panel',
    //alias: 'widget.accrualaccountopergrid',
    alias: 'widget.accountsaggregationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Агрегация по счетам абонентов',
    store: 'longtermprobject.AccountCharge',

    initComponent: function() {
        var me = this,
            numberfield = {
                xtype: 'numberfield',
                hideTrigger: true,
                operand: CondExpr.operands.eq,
                decimalSeparator: ','
            },
            numberRenderer = function (val) {
                return Ext.util.Format.currency(val);
            };
        
        Ext.applyIf(me, {
            cls: 'x-large-head',
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Date',
                    text: 'Месяц',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'PaymentChargeMonth',
                    text: 'Начислено взносов за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    dataIndex: 'PaymentPaidMonth',
                    text: 'Оплачено взносов за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    dataIndex: 'PaymentDebtMonth',
                    text: 'Задолженность по взносам за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    dataIndex: 'PenaltiesChargeMonth',
                    text: 'Начислено пени за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    dataIndex: 'PenaltiesPaidMonth',
                    text: 'Оплачено пени за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    dataIndex: 'PenaltiesDebtMonth',
                    text: 'Задолженность по пени за месяц',
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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