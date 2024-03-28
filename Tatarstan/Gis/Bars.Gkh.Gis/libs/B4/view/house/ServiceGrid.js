Ext.define('B4.view.house.ServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.house_service_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.HouseService'
    ],

    title: 'Услуги по дому',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.HouseService');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'gridcolumn', dataIndex: 'LsCount', width: 100, text: 'Кол-во ЛС', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'Service', flex: 1, text: 'Услуга', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Supplier', flex: 1, text: 'Поставщик', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Formula', flex: 1, text: 'Формула', filter: { xtype: 'textfield' } }
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