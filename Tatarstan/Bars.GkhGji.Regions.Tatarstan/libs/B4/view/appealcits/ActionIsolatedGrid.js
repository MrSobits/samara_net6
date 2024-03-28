Ext.define('B4.view.appealcits.ActionIsolatedGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.appealcitsactionisolatedgrid',
    
    requires: [
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeObjectAction',
        'B4.enums.KindAction'
    ],
    
    store: 'actionisolated.TaskActionCitsAppeal',
    title: 'Мероприятия без взаимодействия',
    closable: false,
    
    initComponent: function(){
        var me = this;
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    text: 'Дата',
                    flex: 1,
                    filter: { xtype: 'datefield', format: 'd.m.Y'}
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeObject',
                    text: 'Объект мероприятия',
                    enumName: 'B4.enums.TypeObjectAction',
                    flex: 2,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'KindAction',
                    text: 'Вид мероприятия',
                    enumName: 'B4.enums.KindAction',
                    flex: 2,
                    filter: true
                },
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
                                        click: function(){
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