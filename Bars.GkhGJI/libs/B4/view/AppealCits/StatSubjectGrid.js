Ext.define('B4.view.appealcits.StatSubjectGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.appealCitsStatSubjectGrid',
    store: 'appealcits.StatSubject',
    itemId: 'appealCitsStatSubjectGrid',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Subject',
                    flex: 2,
                    text: 'Тематика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Subsubject',
                    flex: 1,
                    text: 'Подтематика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Feature',
                    flex: 1,
                    text: 'Характеристика'
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
                }
            ]
        });

        me.callParent(arguments);
    }
});