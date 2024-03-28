Ext.define('B4.view.objectcr.estimate.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.estimategrid',
    requires: [
        'B4.grid.feature.Summary',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.view.Control.GkhButtonImport',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Сметы',
    autoScroll: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.objectcr.Estimate');
        
        Ext.util.Format.thousandSeparator = ' ';

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 2,
                    text: 'Номер',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 250
                    },
                    filter: { xtype: 'textfield' },
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 3,
                    text: 'Обоснование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед. изм.',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OnUnitCount',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Кол. на ед.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalCount',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Кол. всего',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OnUnitCost',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Стоимость на ед.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalCost',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Общая стоимость',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseSalary',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Осн. з/п',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MachineOperatingCost',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Эк. маш.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MechanicSalary',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'З/п мех.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MaterialCost',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Материалы',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseWork',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Т/з осн.раб.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MechanicWork',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    flex: 1,
                    text: 'Т/з мех.',
                    editor: { xtype: 'gkhdecimalfield' },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            features: [{
                ftype: 'b4_summary'
            }],
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
                            columns: 4,
                            items: [
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    actionName: 'btnSaveRecs',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'checkbox',
                                    actionName: 'cbPrimary',
                                    boxLabel: 'Основные позиции',
                                    checked: true,
                                    labelWidth: 150
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