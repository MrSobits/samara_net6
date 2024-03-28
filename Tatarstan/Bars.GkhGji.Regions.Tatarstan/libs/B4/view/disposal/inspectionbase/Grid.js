Ext.define('B4.view.disposal.inspectionbase.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalinspectionbasegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Основания проведения',
    store: 'disposal.InspectionBase',

    initComponent: function() {
        var me = this;
        
        Ext.applyIf(me,
            {
                columnLines: true,
                columns: [
                    {
                        xtype: 'b4editcolumn',
                        scope: me
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'InspectionBaseType',
                        text: 'Основание проведения',
                        flex: 1
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'OtherInspBaseType',
                        text: 'Иное основание проведения',
                        flex: 1
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'FoundationDate',
                        text: 'Дата основания',
                        format: 'd.m.Y',
                        width: 100
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RiskIndicator',
                        text: 'Индикатор риска',
                        flex: 1
                    },
                    {
                        xtype: 'b4deletecolumn',
                        scope: me
                    }
                ],
                plugins: [ Ext.create('B4.ux.grid.plugin.HeaderFilters') ],
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