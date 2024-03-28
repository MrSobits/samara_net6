Ext.define('B4.view.program.CorrectionResultDetailsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.correctionresultdetailsgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'ООИ',
    closable: false,
    cls: 'x-grid-header',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.CorrectionResult');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjectName',
                    flex: 1,
                    text: 'ООИ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstPlanYear',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 120
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    text: 'Скорректированный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 150
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublishYear',
                    text: 'Год публикации',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 120
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    
                    if (record.get('PublishYear') > 0 && record.get('PublishYear') < record.get('PlanYear')) {
                        return 'back-orange';
                    }
                    
                    switch (record.get('TypeResult')) {
                    case 50:
                        return 'back-coralyellow';
                    case 40:
                        return 'back-coralred';
                    }
                    
                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});