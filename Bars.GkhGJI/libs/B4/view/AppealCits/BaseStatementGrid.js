Ext.define('B4.view.appealcits.BaseStatementGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.baseStatementAppCitsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.GjiTextValuesOverride'
    ],

    title: 'Проверки',
    store: 'appealcits.BaseStatement',
    itemId: 'baseStatementAppCitsGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    width: 80,
                    filter: { xtype: 'textfield' }
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'DocumentNumber',
                     flex: 1,
                     filter: { xtype: 'textfield' },
                     text: B4.GjiTextValuesOverride.getText('Номер распоряжения/решения')
                 },
				{
					xtype: 'gridcolumn',
					dataIndex: 'DocumentGjiInspector',
                    flex: 1,
                    text: 'Сотрудник',
                    filter: { xtype: 'textfield' }
				},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: B4.GjiTextValuesOverride.getText('Дата распоряжения'),
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    width: 100
                },
                 {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealtyObject',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Объект проверки'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: false
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