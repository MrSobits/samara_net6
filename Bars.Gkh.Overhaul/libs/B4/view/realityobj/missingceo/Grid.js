Ext.define('B4.view.realityobj.missingceo.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.missingcommonestobjgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.realityobj.MissingCeo'
    ],

    store: 'realityobj.MissingCeo',
    title: 'Отсутствие конструктивных элементов',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.MissingCeo');

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MissingCommonEstateObject',
                    flex: 3,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        },
                        { xtype: 'tbfill' }
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