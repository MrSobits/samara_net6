﻿Ext.define('B4.view.longtermprobject.accrualsaccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.accrualsaccountgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Cчета начислений',
    store: 'account.Accruals',
    closable: true,

    initComponent: function () {
        var me = this,
            numberfield = {
                xtype: 'numberfield',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: true,
                decimalSeparator: ','
            },
            numberRenderer = function(val) {
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
                    flex: 1,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OpenDate',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата открытия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                     xtype: 'datecolumn',
                     dataIndex: 'CloseDate',
                     format: 'd.m.Y',
                     width: 100,
                     text: 'Дата закрытия',
                     filter: {
                         xtype: 'datefield',
                         operand: CondExpr.operands.eq,
                         format: 'd.m.Y'
                     }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalDebit',
                    flex: 1,
                    text: 'Итого начислено',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalCredit',
                    flex: 1,
                    text: 'Итого оплачено',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Balance',
                    flex: 1,
                    text: 'Входящее сальдо',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OpeningBalance',
                    flex: 1,
                    text: 'Исходящее сальдо',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LastOperationDate',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата последней операции',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
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