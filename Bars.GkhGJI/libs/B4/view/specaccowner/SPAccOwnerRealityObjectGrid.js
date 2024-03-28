Ext.define('B4.view.specaccowner.SPAccOwnerRealityObjectGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.specaccownerrobjectgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Специальные счета',
    store: 'specaccowner.SPAccOwnerRealityObject',
    //  itemId: 'risExportTaskDocumentGJIGrid',

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
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'МО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес МКД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SpacAccNumber',
                    flex: 1,
                    text: 'Номер счета'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditOrg',
                    flex: 1,
                    text: 'Банк'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 0.5,
                    text: 'Дата открытия',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    flex: 0.5,
                    text: 'Дата закрытия',
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