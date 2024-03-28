Ext.define('B4.view.subsidy.SubsidyMuRecordGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.subsidymunicipalityrecordgrid',
    requires: [
        'B4.store.subsidy.SubsidyMunicipalityRecord',
        'B4.grid.feature.Summary'
    ],

    store: 'subsidy.SubsidyMunicipalityRecord',
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            features: [{
                ftype: 'summary'
            }],
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubsidyYear',
                    flex: 1,
                    sortable: false,
                    text: 'Год',
                    width: 80,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetFund',
                    flex: 1,
                    text: 'Средства Фонда',
                    sortable: false,
                    minWidth: 120,
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetRegion',
                    flex: 1,
                    text: 'Средства региона',
                    sortable: false,
                    minWidth: 120,
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMunicipality',
                    flex: 1,
                    text: 'Средства МО',
                    sortable: false,
                    minWidth: 120,
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OtherSource',
                    flex: 1,
                    text: 'Иные источники',
                    sortable: false,
                    minWidth: 120,
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinanceNeedBefore',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Потребность в финансированиии (до корректировки)',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinanceNeedAfter',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Потребность в финансированиии (после корректировки)',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EstablishedTarif',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    text: 'Установленный тариф',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecommendedTarif',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    text: 'Рекомендуемый тариф для покрытия потребности',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecommendedTarifCollection',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Собираемость по рекомендуемому тарифу',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CalculatedCollection',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    hidden: true,
                    text: 'Расчетная собираемость',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnersLimit',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Прогнозируемая собираемость',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareBudgetFund',
                    sortable: false,
                    flex: 1,
                    minWidth: 70,
                    hidden: true,
                    text: 'Доля бюджета фонда',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareBudgetRegion',
                    sortable: false,
                    flex: 1,
                    minWidth: 70,
                    hidden: true,
                    text: 'Доля бюджета региона',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareBudgetMunicipality',
                    sortable: false,
                    flex: 1,
                    minWidth: 70,
                    hidden: true,
                    text: 'Доля бюджета МО',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareOtherSource',
                    sortable: false,
                    flex: 1,
                    minWidth: 70,
                    hidden: true,
                    text: 'Доля других истоников',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareOwnerFounds',
                    sortable: false,
                    flex: 1,
                    minWidth: 70,
                    hidden: true,
                    text: 'Доля средств собственников',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DeficitFromCorrect',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Дефицит/Профицит',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetFundBalance',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Остаток бюджет Фонда',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetRegionBalance',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Остаток бюджет субъекта',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMunicipalityBalance',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Остаток бюджет МО',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OtherSourceBalance',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Остаток бюджет иные источники',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnersMoneyBalance',
                    sortable: false,
                    flex: 1,
                    minWidth: 90,
                    hidden: true,
                    text: 'Остаток бюджет средства собственников',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});