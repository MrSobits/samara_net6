Ext.define('B4.view.riskorientedmethod.ROMCategoryMKDGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.romcategorymkdgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'МКД в управлении',
    store: 'riskorientedmethod.ROMCategoryMKD',
    itemId: 'rOMCategoryMKDGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес МКД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectArea',
                    flex: 1,
                    text: 'Общая площадь'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Дата начала управления',
                    format: 'd.m.Y'
                },
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