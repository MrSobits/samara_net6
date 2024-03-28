Ext.define('B4.view.gasu.GASUDataGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.gasudatagrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
    ],

    title: 'Показатели',
    store: 'smev.GASUData',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexUid',
                    text: 'Код показателя',
                    flex: 0.5
                },             
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Indexname',
                    text: 'Наименование показателя',
                    flex: 1
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    text: 'Ед.изм.',
                    flex: 0.5
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    text: 'Значение',
                    flex: 0.5
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