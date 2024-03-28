Ext.define('B4.view.priorityparam.multi.SelectedGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparammultiselectedgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.priorityparam.multi.Selected',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    itemId: 'priorityparammultiselectedgrid',
    store: 'priorityparam.multi.Selected',
    title: 'Выбранные коды',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columnLines: true,
            columns: [
                {
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1
                },
                {
                    dataIndex: 'Code',
                    text: 'Код',
                    flex: 0.5
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