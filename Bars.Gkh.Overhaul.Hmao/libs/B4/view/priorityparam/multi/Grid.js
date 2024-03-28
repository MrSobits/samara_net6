Ext.define('B4.view.priorityparam.multi.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparammultigrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.priorityparam.Multi',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    itemId: 'priorityparammultigrid',
    store: 'priorityparam.Multi',
    title: 'Параметры',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Names',
                    text: 'Наименование(-я)',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Codes',
                    text: 'Код(-ы)',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Point',
                    text: 'Балл',
                    flex: 1,
                    sortable: false,
                    renderer: function (val) {
                        return val ? val.toString().replace('.', ',') : val;
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});