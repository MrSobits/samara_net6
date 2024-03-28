Ext.define('B4.view.regop.realty.RealtyChargeAccountOperationGrid', {
    extend: 'B4.ux.grid.Panel',
    title: 'Операции',

    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    alias: 'widget.realtychargeaccopgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.realty.RealtyChargeAccountOperation');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    text: 'Период',
                    dataIndex: 'Period',
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Входящее сальдо (руб.)',
                    dataIndex: 'SaldoIn',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Итого начислено (руб.)',
                    dataIndex: 'ChargedTotal',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Итого оплачено (руб.)',
                    dataIndex: 'PaidTotal',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Начислено пени (руб.)',
                    dataIndex: 'ChargedPenalty',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Оплачено пени (руб.)',
                    dataIndex: 'PaidPenalty',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Исходящее сальдо (руб.)',
                    dataIndex: 'SaldoOut',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});