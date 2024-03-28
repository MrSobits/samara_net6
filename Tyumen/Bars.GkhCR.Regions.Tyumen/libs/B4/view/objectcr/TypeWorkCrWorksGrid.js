Ext.define('B4.view.objectcr.TypeWorkCrWorksGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.typeworkcrworksgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Работы',
    store: 'objectcr.TypeWorkCrWorks',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 0.5,
                    text: 'Работа'
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 0.5,
                    text: 'Год'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    flex: 0.5,
                    text: 'Стоимость',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'actioncolumn',
                    action: 'deleteWork',
                    width: 18,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/delete.png'),
                    tooltip: 'Удалить'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
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
                                    xtype: 'b4savebutton'
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