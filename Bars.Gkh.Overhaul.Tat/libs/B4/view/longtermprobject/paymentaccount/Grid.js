Ext.define('B4.view.longtermprobject.paymentaccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paymentaccountgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.account.Payment'
    ],

    title: 'Счета оплат',
    closable: true,

    initComponent: function() {
        var me = this,
            numberfield = {
                xtype: 'numberfield',
                operand: CondExpr.operands.eq,
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: true,
                decimalSeparator: ','
            },
            numberRenderer = function (val) {
                return val ? Ext.util.Format.currency(val) : '';
            },
            store = Ext.create('B4.store.account.Payment');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});