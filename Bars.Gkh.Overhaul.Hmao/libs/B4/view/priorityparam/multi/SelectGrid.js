Ext.define('B4.view.priorityparam.multi.SelectGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparammultiselectgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.priorityparam.multi.Select',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    itemId: 'priorityparammultiselectgrid',
    store: 'priorityparam.multi.Select',
    title: 'Коды для выбора',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    text: 'Код',
                    flex: 1
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