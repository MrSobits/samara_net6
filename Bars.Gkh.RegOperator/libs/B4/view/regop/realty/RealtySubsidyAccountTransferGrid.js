Ext.define('B4.view.regop.realty.RealtySubsidyAccountTransferGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.regop.realty.RealtyFactSubsidyAccountOperation'
    ],

    alias: 'widget.realtysubsidytransfergrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.TransferForSubsidyAccount');

        Ext.apply(me, {
            store: store,
            cls: 'x-large-head',
            columns: [
                {
                    xtype: 'datecolumn',
                    text: 'Дата',
                    dataIndex: 'OperationDate',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Тип операции',
                    dataIndex: 'Reason',
                    flex: 1
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Сумма (руб.)',
                    dataIndex: 'Amount',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
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