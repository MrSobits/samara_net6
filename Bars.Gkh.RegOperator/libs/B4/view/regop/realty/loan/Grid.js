Ext.define('B4.view.regop.realty.loan.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realtyloangrid',
   
    requires: [
        'B4.ux.button.Update',
        'B4.store.regop.realty.loan.LoanReceiver',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.feature.Summary',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum'
    ],
    title: 'Займы',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.realty.loan.LoanReceiver');

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LoanDate',
                    text: 'Дата займа',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function() {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    text: 'Источник займа',
                    dataIndex: 'Sources',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Программа КР',
                    dataIndex: 'ProgramCr',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: ' Сумма',
                    dataIndex: 'LoanSum',
                    xtype: 'numbercolumn',
                    width: 100,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Погашено',
                    dataIndex: 'LoanReturnedSum',
                    xtype: 'numbercolumn',
                    width: 100,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Задолженность',
                    dataIndex: 'DebtSum',
                    xtype: 'numbercolumn',
                    width: 100,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Факт. дата закрытия',
                    xtype: 'datecolumn',
                    dataIndex: 'FactEndDate',
                    format: 'd.m.Y',
                    width: 150,
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    text: 'Файл',
                    dataIndex: 'Id',
                    renderer: function(val) {
                        if (val > 0) {
                            return '<a href="' + B4.Url.action('GetDisposal', 'RealityObjectLoan', { Id: val }) + '" style="color: #08c;">Файл</a>';
                        }
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
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        click: function(btn) {
                                            btn.up('grid').getStore().load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Осуществить возврат займов',
                                    iconCls: 'icon-money-add',
                                    action: 'returnloan'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    pluginId: 'paging',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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