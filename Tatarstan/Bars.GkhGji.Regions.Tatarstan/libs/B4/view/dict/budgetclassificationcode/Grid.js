Ext.define('B4.view.dict.budgetclassificationcode.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'КБК',
    alias: 'widget.budgetclassificationcodegrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.BudgetClassificationCode');
        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kbk',
                    flex: 1,
                    text: 'КБК'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipalities',
                    flex: 1,
                    text: 'Муниципальные образования'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLaw',
                    flex: 1,
                    text: 'Статья закона'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'StartDate',
                    flex: 1,
                    text: 'Дата начала действия'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'EndDate',
                    flex: 1,
                    text: 'Дата окончания действия'
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
                    dock: 'bottom',
                    displayInfo: true,
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    }
});