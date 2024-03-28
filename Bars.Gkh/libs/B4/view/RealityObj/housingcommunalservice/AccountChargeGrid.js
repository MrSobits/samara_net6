Ext.define('B4.view.realityobj.housingcommunalservice.AccountChargeGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.hseaccountchargegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],

    store: 'realityobj.housingcommunalservice.AccountCharge',
    
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
                    dataIndex: 'Supplier',
                    text: 'Поставщик',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    dataIndex: 'Service',
                    text: 'Услуга',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    dataIndex: 'Tariff',
                    text: 'Тариф',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                //{
                //    xtype: 'datecolumn',
                //    format: 'd.m.Y',
                //    dataIndex: 'DateCharging',
                //    text: 'Дата начисления',
                //    flex: 1,
                //    filter: {
                //        xtype: 'datefield',
                //        operand: CondExpr.operands.eq,
                //        format: 'd.m.Y'
                //    }
                //},
                {
                    dataIndex: 'Expense',
                    text: 'Расход',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'CompleteCalc',
                    text: 'Полный расчет',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Underdelivery',
                    text: 'Недопоставка',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Charged',
                    text: 'Начислено',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'Recalc',
                    text: 'Перерасчет',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'InnerBalance',
                    text: 'Вх. сальдо',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'Changed',
                    text: 'Изменен',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Payment',
                    text: 'Оплата',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'ChargedPayment',
                    text: 'Начислено к оплате',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'OuterBalance',
                    text: 'Исх. сальдо',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [
                {
                    ftype: 'groupingsummarytotal',
                    groupHeaderTpl: '{name}',
                    startCollapsed: true
                }
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                }
                //,
                //{
                //    xtype: 'b4pagingtoolbar',
                //    displayInfo: true,
                //    store: this.store,
                //    dock: 'bottom'
                //}
            ]
        });

        me.callParent(arguments);
    }
});