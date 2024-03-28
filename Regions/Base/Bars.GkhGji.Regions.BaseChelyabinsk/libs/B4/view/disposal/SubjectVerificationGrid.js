Ext.define('B4.view.disposal.SubjectVerificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalsubjectverificationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Предметы проверки',
    store: 'disposal.DisposalVerificationSubject',

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    sortable: false,
                    dataIndex: 'Code',
                    text: 'Код',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    sortable: false,
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                                    xtype: 'b4addbutton',
                                    handler: function(btn) {
                                        var grid = btn.up('gridpanel');
                                        grid.fireEvent('gridaction', grid, 'add');
                                    }
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function (btn) {
                                        var grid = btn.up('gridpanel');
                                        grid.fireEvent('gridaction', grid, 'update');
                                    }
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