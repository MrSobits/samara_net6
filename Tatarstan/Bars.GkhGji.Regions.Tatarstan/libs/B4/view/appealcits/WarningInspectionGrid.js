Ext.define("B4.view.appealcits.WarningInspectionGrid", {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.appealCitsWarningInspectionGrid',
    
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.appealcits.WarningInspection'
    ],
    
    title: 'Предостережения',
    closable: false,
    store: 'appealcits.WarningInspection',

    initComponent: function() {
        var me = this;
        
        Ext.applyIf(me, {
            columnLines: true,
            store: me.store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер предостережения',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DocumentDate',
                    text: 'Дата предостережения',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealtyObject',
                    text: 'Объект проверки',
                    flex: 2,
                    filter: { xtype: 'textfield' }
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        click: function()
                                        {
                                            me.getStore().load();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});