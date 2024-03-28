Ext.define('B4.view.objectcr.estimate.EstimateCalculationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.grid.feature.Summary',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'B4.enums.EstimationType'
    ],

    alias: 'widget.objectcrestimateregdetailgrid',
    title: 'Сметный расчет по работе',
    closable: true,

    initComponent: function () {
        var me = this,
        store = Ext.create('B4.store.objectcr.estimate.EstimateRegisterDetail');
        Ext.util.Format.thousandSeparator = ' ';

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 200,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        var record;
                        if (type == 'click' && e.target.localName == 'img') {
                            record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: me,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'EstimationType',
                    flex: 1,
                    text: 'Тип сметы',
                    enumName: 'B4.enums.EstimationType',
                    editor: {
                        xtype: 'b4enumcombo',
                        enumName: 'B4.enums.EstimationType'
                    }
                },
                {
                    dataIndex: 'TypeWorkCrName',
                    flex: 3,
                    text: 'Наименование работы'
                },
                {
                    dataIndex: 'FinanceSourceName',
                    flex: 2,
                    text: 'Разрез финансирования'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalEstimate',
                    flex: 1,
                    text: 'Итого по смете',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'TotalEstimateSum',
                    flex: 1,
                    text: 'Сумма по смете',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'TotalResourceSum',
                    text: 'Сумма по ресурсам',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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