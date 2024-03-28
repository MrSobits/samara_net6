Ext.define('B4.view.preventiveaction.task.PlannedActionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        
        'B4.store.preventiveaction.PreventiveActionTaskPlannedAction'
    ],

    alias: 'widget.plannedactiongrid',
    title: 'Запланированные действия',
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.preventiveaction.PreventiveActionTaskPlannedAction');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    text: 'Действие',
                    flex: 1,
                    editor: { 
                        xtype: 'textfield',
                        allowBlank: false
                    } 
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Commentary',
                    text: 'Комментарий',
                    flex: 1,
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    itemId: 'plannedActionSaveButton',
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});