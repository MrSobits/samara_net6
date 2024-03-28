Ext.define('B4.view.preventiveaction.task.ObjectiveTabPanel', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.objectivetabpanel',
    title: 'Цели мероприятия',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'preventiveaction.task.Objective',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
            {
                columnLines: true,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'Name',
                        text: 'Наименование'
                    },
                    {
                        xtype: 'b4deletecolumn',
                        scope: me
                    }
                ],
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
                        store: me.store,
                        dock: 'bottom'
                    }
                ],
                plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                viewConfig: { loadMask: true },
            });

        me.callParent(arguments);
    }
});