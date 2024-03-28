Ext.define('B4.view.priorityparam.QualityGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparamqualitygrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.priorityparam.Quality',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit'
    ],

    itemId: 'priorityparamqualitygrid',
    store: 'priorityparam.Quality',
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
                    dataIndex: 'EnumDisplay',
                    text: 'Значение',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Point',
                    text: 'Балл',
                    flex: 1,
                    sortable: false,
                    renderer: function(val) {
                        return val ? val.toString().replace('.', ',') : val;
                    }
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});