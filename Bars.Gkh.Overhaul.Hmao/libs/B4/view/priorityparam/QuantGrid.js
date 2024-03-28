Ext.define('B4.view.priorityparam.QuantGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparamquantgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.priorityparam.Quant',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    itemId: 'priorityparamquantgrid',
    store: 'priorityparam.Quant',
    title: 'Параметры',

    initComponent: function() {
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
                    dataIndex: 'MinValue',
                    text: 'Начальное значение',
                    flex: 1,
                    sortable: false,
                    renderer: function (val) {
                        return val ? val.toString().replace('.', ',') : val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MaxValue',
                    text: 'Конечное значение',
                    flex: 1,
                    sortable: false,
                    renderer: function (val) {
                        return val ? val.toString().replace('.', ',') : val;
                    }
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
                            columns: 3,
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