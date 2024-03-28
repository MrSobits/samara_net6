Ext.define('B4.view.house.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.house_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.House'
    ],

    title: 'Информация о ЖКУ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.House');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn' },
                { xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, text: 'Адрес', filter: { xtype: 'textfield' } }
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