Ext.define('B4.view.specialobjectcr.TypeWorkCrGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.feature.GroupingSummaryTotal',
        
        'B4.store.specialobjectcr.TypeWorkCr',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.enums.TypeWork'
    ],

    alias: 'widget.typeworkspecialcrgrid',
    
    title: 'Виды работ',
    closable: false,

    // необходимо для того чтобы не работали восстановления для грида поскольку колонки показываются и скрываются динамически
    provideStateId: Ext.emptyFn,
    stateful: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.specialobjectcr.TypeWorkCr');

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Тип работы',
                    renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 3,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumMaterialsRequirement',
                    flex: 1,
                    text: 'Потребность материалов (руб.)',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasPsd',
                    flex: 1,
                    text: 'Наличие ПСД',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    text: 'Краткосрочная программа',
                    flex: 3,
                    name: 'groupCr',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UnitMeasureName',
                            width: 80,
                            text: 'Ед. изм.'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Volume',
                            flex: 1,
                            text: 'Объем',
                            width: 80,
                            renderer: function(val) {
                                return val ? Ext.util.Format.currency(val) : '';
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Sum',
                            width: 100,
                            text: 'Сумма (руб.)',
                            renderer: function(val) {
                                return val ? Ext.util.Format.currency(val) : '';
                            },
                            summaryType: 'sum',
                            summaryRenderer: function(val) {
                                return val ? Ext.util.Format.currency(val) : '';
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});