﻿Ext.define('B4.view.subsidy.SubsidyRecordGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.subsidyrecordgrid',
    requires: [
        'B4.store.subsidy.SubsidyRecord',
        //'B4.grid.feature.Summary'
    ],

    cls: 'x-large-head',

    store: 'subsidy.SubsidyRecord',
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        var groupingFeature = Ext.create('Ext.grid.feature.Grouping', {
            groupHeaderTpl: ' ',
            collapsible: false
        });

        Ext.applyIf(me, {
            columnLines: true,
            features: [
                groupingFeature
                //,{ ftype: 'summary' }
            ],
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
                    },
                    renderer: function (value, meta, rec) {
                        var result = value;
                        if (value == 0 && rec.get('IsSummary')) {
                            result = 'Итого';
                        }
                        return result;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOwnerCollection',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Плановая сумма сбора средств собственников, руб.',
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm'))
                            return Ext.util.Format.currency(value, null, 2);

                        return "-";
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOwnerPercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Плановая собираемость, %',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0,
                        maxValue: 100
                    },
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        } 

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotReduceSizePercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Неснижаемый размер регионального фонда, %',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0,
                        maxValue: 100
                    },
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }
                        
                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerSumForCr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Средства собственников на капитальный ремонт, руб.',
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm'))
                            return Ext.util.Format.currency(value, null, 2);
                        return "-";
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
                    text: 'Региональный бюджет, руб.',
                    tdCls: 'b-editable-spec',
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
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMunicipality',
                    flex: 1,
                    tdCls: 'b-editable-spec',
                    text: 'Бюджет муниципальных образований, руб.',
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
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetFcr',
                    flex: 1,
                    text: 'Средства ГК ФСР ЖКХ, руб.',
                    sortable: false,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
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
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetOtherSource',
                    flex: 1,
                    text: 'Средства иных источников, руб.',
                    tdCls: 'b-editable-spec',
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
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetCr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Итоговый бюджет на капитальный ремонт, руб',
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
                    dataIndex: 'CorrectionFinance',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Расчетная потребность в финансировании капитального ремонта, руб',
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
                    dataIndex: 'AdditionalExpences',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Доп. расходы, руб',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BalanceAfterCr',
                    xtype: 'gridcolumn',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Остаток  средств после проведения КР на конец года, руб',
                    renderer: function (value) {
                        var result = Ext.util.Format.currency(value, null, 2);
                        return value < 0 ? Ext.util.Format.currency(0, null, 2) : result;
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
                loadMask: true,
                getRowClass: function (record) {
                    var result = 'x-subsidy-row';
                    if (record.get('IsShortTerm')) {
                        result += ' x-subsidy-spec-row';
                    }
                    if (record.get('IsSummary')) {
                        result = 'x-summary';
                    }
                    return result;
                }
            }
        });

        me.callParent(arguments);
    }
});