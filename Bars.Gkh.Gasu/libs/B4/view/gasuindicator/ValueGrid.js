Ext.define('B4.view.gasuindicator.ValueGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.Periodicity',
        'B4.enums.EbirModule',
        'B4.store.GasuIndicator',
        'B4.store.dict.Municipality',
        'B4.form.ComboBox'
    ],

    title: '',
    alias: 'widget.gasuindicatorvaluegrid',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GasuIndicatorValue');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.EbirModule',
                    dataIndex: 'EbirModule',
                    flex: 1,
                    text: 'Модуль ЕБИР',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GasuIndicatorName',
                    flex: 1,
                    text: 'Показатель ГАСУ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед.измерения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.Periodicity',
                    dataIndex: 'Periodicity',
                    flex: 1,
                    text: 'Периодичность',
                    filter: true
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PeriodStart',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Начало периода',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
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
                    dataIndex: 'Value',
                    flex: 1,
                    text: 'Значение показателя',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2
                    },
                    filter: {
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
                        if (value)
                            return Ext.util.Format.currency(value, null, 2);

                        return "";
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
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