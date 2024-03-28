Ext.define('B4.view.personalAccount.CounterGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personalAccount_counter_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.PersonalAccountCounter'
    ],

    title: 'Показания ИПУ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccountCounter');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'gridcolumn', dataIndex: 'Service', flex: 1, text: 'Услуга', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Supplier', flex: 1, text: 'Поставщик', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'CounterNumber', width: 100, text: 'Номер ПУ', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'CounterType', width: 100, text: 'Тип ПУ', filter: { xtype: 'textfield' } },
                { xtype: 'datecolumn', dataIndex: 'PrevStatementDate', width: 100, text: 'Дата предыдущего показания', filter: { xtype: 'textfield' }, format: 'd.m.Y' },
                { xtype: 'gridcolumn', dataIndex: 'PrevCounterValue', width: 100, text: 'Предыдущее показание', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'datecolumn', dataIndex: 'StatementDate', width: 100, text: 'Дата текущего показания', filter: { xtype: 'textfield' }, format: 'd.m.Y' },
                { xtype: 'gridcolumn', dataIndex: 'CounterValue', width: 100, text: 'Текущее показание', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
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