Ext.define('B4.view.competition.LotBidGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.competitionlotbidgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.competition.LotBid',
        'B4.ux.grid.filter.YesNo'
    ],

    title: 'Заявки',
    
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.competition.LotBid');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IncomeDate',
                    width: 150,
                    text: 'Дата поступления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Builder',
                    xtype: 'gridcolumn',
                    flex: 1,
                    text: 'Участник',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Price',
                    width: 150,
                    text: 'Цена заявки',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    dataIndex: 'Points',
                    width: 150,
                    text: 'Количество баллов',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsWinner',
                    width: 150,
                    text: 'Победитель',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno' }
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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