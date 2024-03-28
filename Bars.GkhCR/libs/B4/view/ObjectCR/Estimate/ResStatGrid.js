Ext.define('B4.view.objectcr.estimate.ResStatGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.resstatgrid',
    requires: [
        'B4.grid.feature.Summary',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.form.ComboBox',
        'B4.store.dict.UnitMeasure'
    ],

    title: 'Ведомость ресурсов',
    autoScroll: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.objectcr.estimate.ResStat');

        Ext.util.Format.thousandSeparator = ' ';

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 120,
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
                    width: 120,
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
                    flex: 2,
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
                    dataIndex: 'TotalCount',
                    renderer: Ext.util.Format.numberRenderer('0,00/i'),
                    width: 120,
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
                    width: 120,
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
                    width: 120,
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
                            columns: 3,
                            items: [
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    itemId: 'btnSaveRecs',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'checkbox',
                                    padding: '0 5 0 5',
                                    name: 'IsSumWithoutNds',
                                    fieldLabel: 'Сумма по ресурсам/материалам указана без НДС:',
                                    labelWidth: 260
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