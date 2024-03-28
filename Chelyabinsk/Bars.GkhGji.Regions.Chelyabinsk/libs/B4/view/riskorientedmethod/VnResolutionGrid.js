Ext.define('B4.view.riskorientedmethod.VnResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.vnresolutiongrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Постановления Vн',
    store: 'riskorientedmethod.VnResolution',
    itemId: 'vnResolutionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Resolution',
                    flex: 1,
                    text: 'номер постановления'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ResolutionDate',
                    flex: 0.5,
                    text: 'Дата постановления',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArtLaws',
                    flex: 1,
                    text: 'Статьи закона'
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