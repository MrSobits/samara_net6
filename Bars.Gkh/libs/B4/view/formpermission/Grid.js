Ext.define('B4.view.formpermission.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'Ext.ux.CheckColumn'
    ],
    alias: 'widget.formpermissiongrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.FormPermission');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Path',
                    text: 'Путь',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 2
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Grant',
                    text: 'Доступ',
                    width: 50
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