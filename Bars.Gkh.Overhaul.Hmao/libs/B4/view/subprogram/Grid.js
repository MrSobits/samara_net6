Ext.define('B4.view.subprogram.Grid',
{
	extend: 'B4.ux.grid.Panel',
	requires: [
		'B4.ux.button.Update',
		'B4.ux.button.Add',
		'B4.ux.grid.column.Edit',
		'B4.ux.grid.column.Delete',
		'B4.ux.grid.toolbar.Paging',
		'B4.ux.grid.plugin.HeaderFilters',
		'Ext.ux.RowExpander'
	],
	alias: 'widget.subprogramgrid',
	title: 'Критерии попадания в подпрограмму',
	store: 'SubProgramCriterias',
	initComponent: function()
	{
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
				header: 'Название',
				dataIndex: 'Name',
				flex: 0.2,
				filter:
				{
					xtype: 'textfield'
				},
			},
            {
				header: 'Описание',
				dataIndex: 'Description',
				flex: 0.6,
				filter:
				{
					xtype: 'textfield'
				},
			},
			{
				xtype: 'gridcolumn',
				dataIndex: 'Operator',
				flex: 0.2,
				text: 'Оператор'
            },
            {
                xtype: 'b4deletecolumn',
                scope: me
            }],
			plugins: [
				Ext
				.create(
					'B4.ux.grid.plugin.HeaderFilters'
				)
			],
			viewConfig:
			{
				loadMask: true
			},
			dockedItems: [
			{
				xtype: 'toolbar',
				dock: 'top',
				name: 'buttons',
				items: [
				{
					xtype: 'buttongroup',
					columns: 3,
					items: [
					{
						xtype: 'b4addbutton'
					},
					{
						xtype: 'b4updatebutton',
						handler: function()
						{
							var me = this;
							me.up(
									'grid'
								)
								.getStore()
								.load();
						}
					}, ]
				}]
			},
			{
				xtype: 'b4pagingtoolbar',
				displayInfo: true,
				store: this
					.store,
				dock: 'bottom'
			}]
		});
		me.callParent(
			arguments);
	}
});