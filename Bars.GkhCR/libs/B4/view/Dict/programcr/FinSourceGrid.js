Ext.define('B4.view.dict.programcr.FinSourceGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.programcrfinsourcegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Разрезы финансирования по программе',
    store: 'dict.ProgramCrFinSource',
    itemId: 'programCrFinSourceGrid',
    closable: false,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinanceSourceName',
                    flex: 1,
                    text: 'Наименование'
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