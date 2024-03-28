Ext.define('B4.view.program.CorrectionResultGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.correctionresultgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.form.ComboBox',
        
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.grid.filter.YesNo'
    ],

    title: 'Результат корректировки',
    closable: true,
    cls: 'x-grid-header',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.CorrectionResult');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    tooltip: 'Изменить номер очередности',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
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
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstPlanYear',
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
                    dataIndex: 'PlanYear',
                    text: 'Скорректированный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 140
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1,
                        operand: CondExpr.operands.eq
                    },
                    width: 140
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'FixedYear',
                    width: 150,
                    text: 'Фиксация скорректированного года',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno' }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    if (record.get('PublishYearExceeded')) {
                        return 'back-orange';
                    }
                    
                    switch (record.get('TypeResult')) {
                    case 50:
                        return 'back-coralyellow';
                    case 40:
                        return 'back-coralred';
                    }
                    
                    return '';
                }
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
                                },
                                {
                                    xtype: 'button',
                                    action: 'CreateShortProgram',
                                    text: 'Сформировать краткосрочную программу',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'button',
                                    action: 'ActualizeProgram',
                                    text: 'Актуализировать Версию',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Массовое изменение года',
                                    textAlign: 'left',
                                    action: 'massyearchange'
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