Ext.define('B4.view.econfeasibilitycalcresult.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.econfeasibilitycalcresultgrid',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.EconFeasibilityResult',
        'B4.ux.grid.column.Enum'

    ],

    title: 'Протокол расчета целесообразности',
    store: 'EconFeasibilityCalcResult',
   // itemId: 'econfeasibilitycalcresultGrid',
    closable: true,

    initComponent: function() {
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
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'МО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MoSettlement',
                    flex: 1,
                    text: 'НП',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Adress',
                    flex: 1,
                    text: 'Адрес дома',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearStart',
                    flex: 1,
                    text: 'Год начала',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearEnd',
                    flex: 1,
                    text: 'Год окончания',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotatRepairSumm',
                    flex: 1,
                    text: 'Полная стоимость ремонта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SquareCost',
                    flex: 1,
                    text: 'Средняя стоимость кв.м.',
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalSquareCost',
                    flex: 1,
                    text: 'Средняя стоимость всех помешений'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CostPercent',
                    flex: 1,
                    text: 'Процент',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Decision',
                    flex: 1,
                    filter: true,
                    enumName: 'B4.enums.EconFeasibilityResult',
                    header: 'Решение',
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
                            columns: 2,
                            items: [
                                //{
                                //    xtype: 'b4addbutton'
                                //},
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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