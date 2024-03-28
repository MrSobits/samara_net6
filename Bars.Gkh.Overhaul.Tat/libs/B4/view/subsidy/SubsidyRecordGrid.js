Ext.define('B4.view.subsidy.SubsidyRecordGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.subsidyrecordgrid',
    requires: [
        'B4.store.subsidy.SubsidyRecord'
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
                    width: 50,
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
                    dataIndex: 'BudgetFcr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Средства ГК ФСР ЖКХ, руб.',
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
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Региональный бюджет, руб.',
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
                    dataIndex: 'BudgetMunicipality',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Бюджет муниципального образования, руб.',
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
                    dataIndex: 'OwnerSource',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Средства собственников, руб.',
                    renderer: function (value, meta, rec) {
                        if (rec.get('IsShortTerm'))
                            return Ext.util.Format.currency(value, null, 2);
                        return "-";
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    cls: 'x-grid-header-wrapp'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetCr',
                    flex: 1,
                    text: 'Итоговый бюджет на капитальный ремонт, руб.',
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