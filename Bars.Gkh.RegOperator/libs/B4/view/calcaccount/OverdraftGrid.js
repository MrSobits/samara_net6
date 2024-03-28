Ext.define('B4.view.calcaccount.OverdraftGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.calcaccount.Credit'
    ],

    title: 'Реестр овердрафтов',
    alias: 'widget.calcaccountoverdraftgrid',
    closable: true,
    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.calcaccount.Overdraft'),
            decimalFilter = {
                xtype: 'numberfield',
                operand: CondExpr.operands.eq,
                decimalSeparator: ','
            },
            dateFilter = {
                xtype: 'datefield',
                operand: CondExpr.operands.eq,
                format: 'd.m.Y'
            };

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата установки',
                    filter: dateFilter
                },
                {
                    dataIndex: 'Account',
                    flex: 1,
                    text: 'Р/с для установки овердрафта',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'AccountOwner',
                    flex: 1,
                    text: 'Владелец счета',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'OverdraftLimit',
                    width: 100,
                    text: 'Лимит по овердрафту',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'PercentRate',
                    width: 100,
                    text: 'Процентная ставка',
                    filter: decimalFilter
                },
                {
                    dataIndex: 'OverdraftPeriod',
                    width: 100,
                    text: 'Срок беспроцентного овердрафта',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: false
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