Ext.define('B4.view.contragent.RiskGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.contragentriskgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Категории риска',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.contragent.Risk');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер проверки',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RiskCategory',
                    flex: 1,
                    text: 'Категория риска',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Дата начала',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Дата окончания',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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