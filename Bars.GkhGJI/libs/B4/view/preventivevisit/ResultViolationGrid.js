Ext.define('B4.view.preventivevisit.ResultViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.preventivevisitresultviolationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',    
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'     
    ],

    title: 'Нарушения',
    store: 'preventivevisit.ResultViolation',
    itemId: 'preventivevisitResultViolationGrid',

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
                    dataIndex: 'CodePin',
                    flex: 0.5,
                    text: 'ПиН'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pprf',
                    flex: 1,
                    text: 'Документы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiName',
                    flex: 2,
                    text: 'Нарушение'
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'preventivevisitViolatioSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
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