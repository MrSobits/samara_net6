Ext.define('B4.view.administration.printcerthistory.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',        
        'B4.ux.grid.plugin.HeaderFilters',
		'B4.ux.grid.toolbar.Paging'
    ],

	title: 'История печати справок',
	store: 'administration.PrintCertHistory',
	alias: 'widget.printCertHistoryGrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccNum',
                    flex: 1,
					text: 'Номер лицевого счета',
					filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
					text: 'Адрес',
					filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    flex: 0.5,
					text: 'Тип',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Name',
					flex: 1,
					text: 'Наименование/ФИО',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'PrintDate',
					flex: 0.5,
					text: 'Дата печати отчета',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Username',
					flex: 0.5,
					text: 'Имя пользователя',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Role',
					flex: 0.5,
					text: 'Роль пользователя',
					filter: { xtype: 'textfield' }
				}
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            columns: 2,
                            items: [
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