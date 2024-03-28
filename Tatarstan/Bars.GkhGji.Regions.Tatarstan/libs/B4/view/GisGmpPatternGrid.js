Ext.define('B4.view.GisGmpPatternGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.GisGmpPattern'
    ],

    alias: 'widget.gisgmppatterngrid',

    itemId: 'gisGmpPatternGrid',

    initComponent: function () {
        var me = this;

        var store = Ext.create('B4.store.GisGmpPattern');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                { xtype: 'b4editcolumn', scope: me },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 2,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PatternCode',
                    flex: 2,
                    text: 'Код шаблона',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Дата начала',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    text: 'Дата окончания',
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});