Ext.define('B4.view.import.chesimport.SaldoCheckGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.chesimportsaldocheckgrid',

    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ImportPaymentType',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.selection.CheckboxModel',
        'Ext.ux.grid.FilterBar',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.filter.YesNo',
        'B4.store.import.chesimport.SaldoCheck',
    ],

    columnLines: true,
    title: 'Сверка сальдо',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.SaldoCheck'),
            numberFilter = {
                xtype: 'numberfield',
                allowDecimals: true,
                hideTrigger: true,
                minValue: Number.NEGATIVE_INFINITY,
                operand: CondExpr.operands.eq
            };

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 2,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС БАРС',
                    dataIndex: 'LsNum',
                    flex: 0.8,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС ЧЭС',
                    dataIndex: 'ChesLsNum',
                    flex: 0.8,
                    filter: { xtype: 'textfield' },
                    summaryRenderer: function (value) {
                        return '<b>Итого:</b>';
                    }
                },
                {
                    text: 'Сальдо ЧЭС',
                    dataIndex: 'ChesSaldo',
                    flex: 0.5,
                    summaryType: 'sum',
                    filter: numberFilter
                },
                {
                    text: 'Сальдо БАРС',
                    dataIndex: 'Saldo',
                    flex: 0.5,
                    summaryType: 'sum',
                    filter: numberFilter
                },
                {
                    text: 'Разница (ЧЭС - БАРС)',
                    dataIndex: 'DiffSaldo',
                    flex: 0.6,
                    summaryType: 'sum',
                    filter: numberFilter
                },
                {
                    width: 100,
                    dataIndex: 'IsImported',
                    text: 'Загружено',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: {
                        xtype: 'b4dgridfilteryesno',
                        operator: 'eq'
                    }
                },
            ],
            features: [{
                ftype: 'b4_summary'
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Загрузить сальдо',
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function (button) {
                                        button.up('grid').getStore().load();
                                    }
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
