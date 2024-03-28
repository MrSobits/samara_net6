Ext.define('B4.view.creditorgservicecondition.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],
    cls: 'x-large-head',
    title: 'Условия обслуживания кредитными организациями',
    store: 'CreditOrgServiceCondition',
    alias: 'widget.creditOrgServiceCondGrid',
    closable: true,

    initComponent: function() {
        var me = this,
            dateFilter = {
                xtype: 'datefield',
                operand: CondExpr.operands.eq,
                format: 'd.m.Y'
            },
            numberFilter = {
                xtype: 'numberfield',
                hideTrigger: true,
                operand: CondExpr.operands.eq,
                decimalSeparator: ','
            };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditOrgName',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CashServiceSize',
                    flex: 1,
                    text: 'Размер расчётно-кассового обслуживания',
                    filter: numberFilter
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CashServiceDateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата с',
                    filter: dateFilter
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CashServiceDateTo',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата по',
                    filter: dateFilter
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OpeningAccPay',
                    flex: 1,
                    text: 'Плата за открытие счета',
                    filter: numberFilter
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OpeningAccDateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата с',
                    filter: dateFilter
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OpeningAccDateTo',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата по',
                    filter: dateFilter
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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