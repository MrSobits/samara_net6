Ext.define('B4.view.finactivity.ManagRealityObjGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.finmanagrogrid',
    store: 'finactivity.ManagRealityObj',
    itemId: 'finActivityManagRealityObjGrid',
    title: 'Управление по домам',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.grid.feature.Summary',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressName',
                    flex: 3,
                    text: 'Адрес объекта',
                    filter: { xtype: 'textfield' },
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaMkd',
                    flex: 1,
                    text: 'Общая площадь МКД',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PresentedToRepay',
                    itemId: 'colPresentedToRepay',
                    flex: 1,
                    text: 'Предъявлено к оплате (руб.)',
                    editor: 'gkhdecimalfield',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReceivedProvidedService',
                    itemId: 'colReceivedProvidedService',
                    flex: 1,
                    text: 'Получено за предоставленные услуги (руб.)',
                    editor: 'gkhdecimalfield',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumDebt',
                    itemId: 'colSumDebt',
                    flex: 1,
                    text: 'Сумма задолжености (руб.)',
                    editor: 'gkhdecimalfield',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumFactExpense',
                    flex: 1,
                    text: 'Сумма фактических расходов (руб.)',
                    editor: 'gkhdecimalfield',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumIncomeManage',
                    flex: 1,
                    text: 'Сумма дохода от управления (руб.)',
                    editor: 'gkhdecimalfield',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'saveManagRealityObjButton',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept'
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })

            ],
            features: [
                {
                    ftype: 'b4_summary'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});