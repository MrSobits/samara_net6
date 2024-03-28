Ext.define('B4.view.requeststateperson.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        
        'B4.ux.grid.plugin.HeaderFilters',
		'B4.ux.grid.column.Enum',
		'B4.ux.grid.toolbar.Paging',
		'B4.enums.RequestStatePersonEnum'
    ],

    title: 'Получатели запросов на редактирование',
	store: 'RequestStatePerson',
	alias: 'widget.requestStatePersonGrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
					text: 'ФИО',
					allowBlank: false,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    text: 'Должность',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Email',
                    flex: 1,
                    text: 'Электронная почта',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 100
                    }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Description',
					flex: 1,
					text: 'Примечание',
					editor: {
						xtype: 'textfield',
						maxLength: 300
					}
				},
				{
					xtype: 'b4enumcolumn',
					dataIndex: 'Status',
					enumName: 'B4.enums.RequestStatePersonEnum',
					flex: 1,
					text: 'Статус',
					editor: {
						xtype: 'b4enumcombo',
						enumName: 'B4.enums.RequestStatePersonEnum'
					}
				},
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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