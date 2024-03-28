Ext.define('B4.view.longtermprobject.realaccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realaccountgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Реальные счета',
    store: 'account.Real',
    closable: true,

    initComponent: function() {
        var me = this,
            numberfield = {
                xtype: 'numberfield',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: true,
                decimalSeparator: ','
            },
            numberRenderer = function (val) {
                return val ? Ext.util.Format.currency(val) : '';
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
                    dataIndex: 'Number',
                    text: 'Номер',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OpenDate',
                    text: 'Дата открытия',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CloseDate',
                    text: 'Дата закрытия',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalIncome',
                    text: 'Итого по приходу',
                    width: 200,
                    flex: 1,
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalOut',
                    text: 'Итого по расходу',
                    width: 200,
                    filter: numberfield,
                    flex: 1,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Balance',
                    text: 'Сальдо по счету',
                    width: 200,
                    filter: numberfield,
                    renderer: numberRenderer,
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LastOperationDate',
                    text: 'Дата последней операции по счету',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountOwner',
                    text: 'Владелец счета',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OverdraftLimit',
                    text: 'Лимит по овердрафту',
                    filter: numberfield,
                    flex: 1,
                    renderer: numberRenderer
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