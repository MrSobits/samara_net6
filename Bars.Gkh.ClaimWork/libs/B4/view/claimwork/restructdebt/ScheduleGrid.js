Ext.define('B4.view.claimwork.restructdebt.ScheduleGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save', 
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.claimwork.restructdebt.Schedule',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'График реструктуризации',
    minHeight: 300,
    alias: 'widget.restructschedulegrid',

    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.restructdebt.Schedule', {
                groupField: 'PersonalAccountNum',
            });

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalDebtSum',
                    flex: 1,
                    text: 'Общая сумма долга',
                    renderer: function() {},
                    summaryType: 'max',
                    summaryRenderer: function (value) {
                        return Ext.String.format('<b>{0}</b>', Math.round(value * 100) / 100);
                    },
                    filter: { xtype: 'numberfield', hideTrigger: true }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PlanedPaymentDate',
                    format: 'd.m.Y',
                    flex:1,
                    text: 'Планируемая дата оплаты',
                    filter: { xtype: 'datefield', format: 'd.m.Y' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanedPaymentSum',
                    flex: 1,
                    text: 'Сумма к оплате (руб.)',
                    filter: { xtype: 'numberfield', hideTrigger: true }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PaymentDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Фактическая дата оплаты',
                    filter: { xtype: 'datefield', format: 'd.m.Y' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentSum',
                    flex: 1,
                    text: 'Оплачено (руб.)',
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.String.format('<b>{0}</b>', Math.round(value * 100) / 100);
                    },
                    filter: { xtype: 'numberfield', hideTrigger: true }
                }
            ],
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
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            features: [{
                ftype: 'groupingsummary',
                collapsible: false,
                hideGroupedHeader: true,
                enableGroupingMenu: false,
                enableNoGroups: false,
                depthToIndent: 0,
                groupHeaderTpl: 'Номер ЛС: {name}',
                
            }],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});
