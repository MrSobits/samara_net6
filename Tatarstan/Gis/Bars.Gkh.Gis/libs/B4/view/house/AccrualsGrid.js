Ext.define('B4.view.house.AccrualsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.house_accruals_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.HouseAccruals'
    ],

    title: 'Начисления по дому',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.HouseAccruals');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'gridcolumn', dataIndex: 'Service', flex: 1, text: 'Услуга', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'BalanceIn', flex: 1, text: 'Входящее сальдо', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'TariffAmount', flex: 1, text: 'Сумма по тарифу', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'BackorderAmount', flex: 1, text: 'Сумма недопоставки', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'RecalcAmount', flex: 1, text: 'Сумма перерасчета предыдущего периода', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'ErcAmount', flex: 1, text: 'Сумма оплаты через расчетный центр', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'SupplierAmount', flex: 1, text: 'Сумма оплаты напрямую поставщику', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'BalanceOut', flex: 1, text: 'Сумма исходящего сальдо', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});