Ext.define('B4.view.dict.municipalitytree.MunicipalityFiasOktmoGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.municipalityfiasoktmogrid',
    requires: [
        'B4.store.dict.MunicipalityFiasOktmo',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Населенные пункты',
    store: 'dict.MunicipalityFiasOktmo',
    
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
                    dataIndex: 'OffName',
                    flex: 1,
                    text: 'Населенный пункт'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Oktmo',
                    flex: 1,
                    text: 'ОКТМО'
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