Ext.define('B4.view.objectcr.FinanceSourceResGrid', {
    extend: 'B4.ux.grid.Panel',
    alias:'widget.financesourceresgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],

    title: 'Средства источников финансирования',
    closable: true,

    initComponent: function () {
        var me = this,
        store = Ext.create('B4.store.objectcr.FinanceSourceRes');
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWorkCr',
                    flex: 1,
                    text: ' Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMu',
                    flex: 1,
                    text: 'Бюджет МО',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMuIncome',
                    flex: 1,
                    text: 'Поступило по Бюджету МО',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                     xtype: 'gridcolumn',
                     dataIndex: 'BudgetMuPercent',
                     flex: 1,
                     text: 'Процент МО',
                     renderer: function (val) {
                         return val ? Ext.util.Format.currency(val) : '';
                     }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetSubject',
                    text: 'Бюджет субъекта',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetSubjectIncome',
                    flex: 1,
                    text: 'Поступило по Бюджету субъекта',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetSubjectPercent',
                    flex: 1,
                    text: 'Процент БС',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerResource',
                    flex: 1,
                    text: 'Средства собственника',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FundResource',
                    text: 'Средства фонда',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FundResourceIncome',
                    flex: 1,
                    text: 'Поступило по Средствам фонда',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FundResourcePercent',
                    flex: 1,
                    text: 'Процент СФ',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OtherResource',
                    flex: 1,
                    hidden: true,
                    text: 'Иные источники',
                    renderer: function (val) {
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
                    groupHeaderTpl: '{name}'
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
                            columns: 3,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});