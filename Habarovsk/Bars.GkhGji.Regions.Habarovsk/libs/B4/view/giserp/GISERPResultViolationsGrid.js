Ext.define('B4.view.giserp.GISERPResultViolationsGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.giserpresultviolationsgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ERPVLawSuitType',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
    ],

    title: 'Нарушения',
    store: 'smev.GISERPResultViolations',

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
                    dataIndex: 'VIOLATION_NOTE',
                    text: 'Наименование',
                    flex: 2
                }, 
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ERPVLawSuitType',
                    dataIndex: 'VLAWSUIT_TYPE_ID',
                    text: 'Вид сведений',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'VIOLATION_ACT',
                    text: 'НПД',
                    flex: 1
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CODE',
                    text: 'Предписание',
                    flex: 1
                }, 
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DATE_APPOINTMENT',
                    flex: 0.5,
                    text: 'Дата предписания',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
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