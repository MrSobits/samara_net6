Ext.define('B4.view.house.ClaimsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.house_claims_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Претензии граждан',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.HouseClaims');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    text: 'Услуга',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Total',
                    text: 'Всего',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Completed',
                    text: 'Выполнено',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InProgress',
                    text: 'В работе',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'New',
                    text: 'Новые',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Overdue',
                    text: 'Просроченные',
                    flex: 1
                }
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