Ext.define('B4.view.appealcits.AppealCitsInfoGrid', {
	extend: 'B4.ux.grid.Panel',
	requires: [
		'B4.ux.grid.plugin.HeaderFilters',
		'B4.ux.grid.toolbar.Paging',
		'B4.ux.button.Update',
		'B4.enums.AppealOperationType'
	],

	alias: 'widget.appealcitsinfogrid',
	title: 'Лог обращений',
	store: 'appealcits.AppealCitsInfo',

	initComponent: function () {
		var me = this;

		Ext.applyIf(me, {
		    columnLines: true,
			columns: [
				{
					xtype: 'gridcolumn',
					dataIndex: 'DocumentNumber',
					flex: 1,
					text: 'Номер обращения',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'datecolumn',
					dataIndex: 'AppealDate',
					flex: 1,
					text: 'Дата обращения',
					format: 'd.m.Y',
					filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Correspondent',
					flex: 1,
					text: 'Корреспондент',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'OperationType',
					flex: 1,
					text: 'Тип операции',
					renderer: function (val) {
						return B4.enums.AppealOperationType.displayRenderer(val);
					},
					filter: {
						xtype: 'combobox',
						store: B4.enums.AppealOperationType.getItemsWithEmpty([null, '-']),
						operand: CondExpr.operands.eq,
						editable: false
					}
				},
				{
					xtype: 'gridcolumn',
					dataIndex: 'Operator',
					flex: 1,
					text: 'Оператор',
					filter: { xtype: 'textfield' }
				},
				{
					xtype: 'datecolumn',
					dataIndex: 'OperationDate',
					flex: 1,
					text: 'Дата операции',
					format: 'd.m.Y H:i:s',
					filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
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