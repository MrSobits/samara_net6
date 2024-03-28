Ext.define('B4.view.regop.personal_account.PersonalAccountOperationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.paoperationgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.column.Number',
        'Ext.grid.column.Date',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.store.regop.personal_account.PeriodSummaryInfo'
    ],

    title: 'Операции',
    store: 'regop.personal_account.PeriodSummaryInfo',

    margin: '10 0 0 0',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PeriodSummaryInfo');
        store.pageSize = 500;

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    dataIndex: 'Period',
                    text: 'Период',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoIn',
                    text: 'Входящее сальдо',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ChargedByBaseTariff',
                    text: 'Начислено',
                    format: '0.00',
                    hidden: true,
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'TariffPayment',
                    text: 'Оплачено по тарифу в текущем периоде',
                    format: '0.00',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'PerformedWorkCharged',
                    text: 'Зачет средств за работы',
                    format: '0.00',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Recalc',
                    text: 'Перерасчет',
                    format: '0.00',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'CurrTariffDebt',
                    text: 'Текущая задолженность',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'OverdueTariffDebt',
                    text: 'Просроченная задолженность',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoOut',
                    text: 'Исходящие сальдо',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoChange',
                    text: 'Изменение сальдо',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoInFromServ',
                    text: 'Входящее сальдо из файла',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoChangeFromServ',
                    text: 'Изменение сальдо из файла',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SaldoOutFromServ',
                    text: 'Исходящее сальдо из файла',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Протокол',
                    icon: B4.Url.content('content/img/icons/pencil_go.png'),
                    tooltip: 'Протокол',
                    type: 'Protocol',
                    width: 100,
                    handler: function (grid, rowIndex, colIndex, el, e, rec) {
                        Ext.History.add(Ext.String.format("personal_calculation_protocol/{0}/{1}", rec.get('PeriodId'), rec.get('AccountId')));
                    },
                    renderer: function () {
                        return 'Протокол ';
                    },
                    scope: me
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});