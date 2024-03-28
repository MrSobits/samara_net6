Ext.define('B4.view.appealcits.PrFondVoilationGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.prFondVoilationGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Нарушения',
    store: 'appealcits.AppCitPrFondVoilation',
    itemId: 'prFondVoilationGrid',

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
                    dataIndex: 'ViolationGji',
                    flex: 1,
                    text: 'Нарушение'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PlanedDate',
                    flex: 1,
                    text: 'Плановая дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FactDate',
                    flex: 1,
                    text: 'Фактическая дата',
                    format: 'd.m.Y'
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