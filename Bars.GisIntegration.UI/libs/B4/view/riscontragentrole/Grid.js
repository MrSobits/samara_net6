Ext.define('B4.view.riscontragentrole.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisoperatorgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.gisrole.GisOperator',
        'B4.store.RisContragent'
    ],

    title: 'Роли РИС контрагентов',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.gisrole.GisOperator');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент РИС',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});