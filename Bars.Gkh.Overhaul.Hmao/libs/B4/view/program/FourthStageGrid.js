Ext.define('B4.view.program.FourthStageGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.programfourthstagegrid',

    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.program.FourthStage',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Municipality'
    ],

    title: 'Результат корректировки',
    closable: false,

    features: [{
        ftype: 'b4_summary'
    }],
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.program.FourthStage', {
                autoLoad: true
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Municipality',
                //    width: 160,
                //    text: 'Муниципальное образование',
                //    filter: {
                //        xtype: 'b4combobox',
                //        operand: CondExpr.operands.eq,
                //        storeAutoLoad: false,
                //        hideLabel: true,
                //        editable: false,
                //        valueField: 'Name',
                //        emptyItem: { Name: '-' },
                //        url: '/Municipality/ListWithoutPaging'
                //    },
                //    summaryRenderer: function () {
                //        return Ext.String.format('Итого:');
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjectName',
                    flex: 1,
                    text: 'ООИ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function(value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 120
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrectionYear',
                    text: 'Скорректированный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 120
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq,
                        allowDecimals: false
                    }
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
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },

                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'b4updatebutton'
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            itemId: 'btnExport'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'PublishDpkr',
                                            text: 'Версия для публикации',
                                            iconCls: 'icon-table-go'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'component',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            name: 'DateCalcCorrection',
                                            width: 250,
                                            padding: '5 0 0 0',
                                            text: ''
                                        }
                                    ]
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