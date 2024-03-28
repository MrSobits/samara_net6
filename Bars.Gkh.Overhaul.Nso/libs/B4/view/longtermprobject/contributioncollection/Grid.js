Ext.define('B4.view.longtermprobject.contributioncollection.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.contributioncollectiongrid',
    
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],
    
    title: 'Показатели сбора взносов на КР',
    
    store: 'ContributionCollection',
    closable: true,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    format: 'm.Y',
                    flex: 1,
                    text: 'Месяц сбора взносов',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'm.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccount',
                    flex: 1,
                    text: 'ЛС собственника дома',                    
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MinContributions',
                    flex: 1,
                    text: 'Минимальный взнос на КР',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaOwnerAccount',
                    flex: 1,
                    text: 'Площадь помещения собственника ЛС',
                    filter: { xtype: 'textfield' },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumMinContributions',
                    flex: 1,
                    text: 'Общая сумма по минимальному взносу',
                    filter: { xtype: 'textfield' },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SetContributions',
                    flex: 1,
                    text: 'Установленный взнос на КР',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaOwnerAccount',
                    flex: 1,
                    text: 'Площадь помещения собственника ЛС',
                    filter: { xtype: 'textfield' },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumSetContributions',
                    flex: 1,
                    text: 'Общая сумма по установленному взносу',
                    filter: { xtype: 'textfield' },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DifferenceSumContributions',
                    flex: 1,
                    text: 'Показатель разницы сумм взносов на КР',
                    filter: { xtype: 'textfield' },
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
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [
                {
                    ftype: 'groupingsummarytotal',
                    groupHeaderTpl: '{name}'
                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});