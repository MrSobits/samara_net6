Ext.define('B4.view.subsidy.SubsidyMuRecordGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.subsidymunicipalityrecordgrid',
    requires: [
        'B4.store.subsidy.SubsidyMunicipalityRecord',
        'B4.grid.feature.Summary'
    ],

    store: 'subsidy.SubsidyMunicipalityRecord',
    enableColumnHide: true,
    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            groupingFeature = Ext.create('Ext.grid.feature.Grouping', {
                groupHeaderTpl: ' ',
                collapsible: false
            }),
            numberEditor = {
                xtype: 'numberfield',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: true,
                decimalSeparator: ',',
                decimalPrecision: 2,
                minValue: 0
            };

        Ext.applyIf(me, {
            columnLines: true,
            features: [
                groupingFeature
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
                    itemId: 'gcBudgetFcr',
                    dataIndex: 'BudgetFcr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Средства ГК ФСР ЖКХ, руб.',
                    editor: numberEditor,
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
                    itemId: 'gcBudgetRegion',
                    dataIndex: 'BudgetRegion',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Региональный бюджет, руб.',
                    editor: numberEditor,
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcBudgetMu',
                    dataIndex: 'BudgetMunicipality',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Бюджет муниципального образования, руб.',
                    editor: numberEditor,
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm')) {
                            return Ext.util.Format.currency(value, null, 2);
                        }

                        return "-";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcOwnerSource',
                    dataIndex: 'OwnerSource',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec-allrows',
                    text: 'Средства собственников, руб.',
                    editor: numberEditor,
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
                    dataIndex: 'BudgetCr',
                    flex: 1,
                    text: 'Итоговый бюджет на капитальный ремонт, руб.',
                    sortable: false,
                    minWidth: 120,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NeedFinance',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Потребность в финансировании, руб.',
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
                    dataIndex: 'Deficit',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Остаток, руб.',
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
                    dataIndex: 'CorrNeedFinance',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Потребность в финансировании после корректировки, руб.',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrDeficit',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Остаток после корректировки, руб.',
                    renderer: function (value) {
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