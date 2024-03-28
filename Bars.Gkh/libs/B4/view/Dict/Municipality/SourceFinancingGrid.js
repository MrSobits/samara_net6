Ext.define('B4.view.dict.municipality.SourceFinancingGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.sourcefingrid',
    requires: [
        'B4.store.dict.MunicipalitySourceFinancing',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging'
    ],

    order: false,
    title: 'Разрезы финансирования',
    store: 'dict.MunicipalitySourceFinancing',
    itemId: 'municipalitySourceFinancingGrid',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            flex: 1,
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SourceFinancing',
                    flex: 1,
                    text: 'Разрез финансирования'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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