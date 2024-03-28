Ext.define('B4.view.requeststateperson.RSGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        
        'B4.ux.grid.plugin.HeaderFilters',
		'B4.ux.grid.column.Enum',
		'B4.ux.grid.toolbar.Paging',
    ],

    title: 'Запросы на редактирование',
	store: 'RequestState',
    alias: 'widget.requeststategrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    flex: 0.5,
					text: 'Пользователь',
					allowBlank: false
                   
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес'
                   
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    flex: 0.5
                },
				{
					xtype: 'gridcolumn',
					dataIndex: 'Description',
					flex: 1,
					text: 'Примечание'
				}
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }, 
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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