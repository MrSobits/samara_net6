Ext.define('B4.view.disposal.AdminRegulationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposaladminregulationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Административные регламенты',
    store: 'disposal.AdminRegulation',

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    text: 'Код',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
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