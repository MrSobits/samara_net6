Ext.define('B4.view.effectivenessandperformanceindexvalue.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.CheckColumn'
    ],

    title: 'Значения показателей эффективности и результативности',
    store: 'EffectivenessAndPerformanceIndexValue',
    alias: 'widget.effectivenessandperformanceindexvaluegrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EffectivenessAndPerformanceIndexName',
                    flex: 6,
                    filter: { xtype: 'textfield' },
                    text: 'Показатель',
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalculationStartDate',
                    flex: 3,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    text: 'Дата начала расчета',
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalculationEndDate',
                    flex: 3,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    text: 'Дата окончания расчета',
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Значение',
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                                    xtype: 'button',
                                    text: 'Отправить в ТОР КНД',
                                    name: 'sendToTorButton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Получить из ТОР КНД',
                                    name: 'getFromTorButton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: this.store
                }
            ]
        });

        me.callParent(arguments);
    }
});