Ext.define('B4.view.personalAccount.AccrualsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personalAccount_accruals_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Edit',
        'B4.store.PersonalAccountAccruals'
    ],

    title: 'Начисления по лицевому счету',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccountAccruals');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {xtype: 'b4editcolumn',scope: me},
                { xtype: 'gridcolumn', dataIndex: 'Service', flex: 1, text: 'Услуга', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Supplier', flex: 1, text: 'Поставщик', filter: { xtype: 'textfield' } },
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
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 1,
                    items: [
                        { xtype: 'b4updatebutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});