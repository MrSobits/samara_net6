Ext.define('B4.view.indicator.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.enums.GisTypeIndicator',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.gisrealestate.IndicatorServiceComparison'
    ],

    title: 'Индикатор',

    alias: 'widget.indicatorgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.gisrealestate.IndicatorServiceComparison");

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ServiceName',
                    flex: 1,
                    text: 'Услуга',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'GisTypeIndicator',
                    flex: 1,
                    text: 'Индикатор',
                    enumName: 'B4.enums.GisTypeIndicator',
                    filter: true
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});