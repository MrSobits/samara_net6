Ext.define('B4.view.actcheck.ActRemovalGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.YesNoNotSet'
    ],

    alias: 'widget.actCheckActRemovalGrid',
    store: 'actcheck.ActRemoval',
    itemId: 'actCheckActRemovalGrid',

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
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y', 
                    width: 150,
                    text: 'Дата'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ParentDocumentName',
                    flex: 1,
                    text: 'Предписание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeRemoval',
                    flex: 1,
                    text: 'Нарушение устранены',
                    renderer: function (val) { return B4.enums.YesNoNotSet.displayRenderer(val); }
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