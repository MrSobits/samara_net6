Ext.define('B4.view.econfeasibilitycalcresult.WorkGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.econfeasibilityworkgrid',

    requires: [
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

    ],

    title: 'Работы',
    store: 'EconFeasibilityCalcWork',
    itemId: 'econfeasibilityWorkGrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
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
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                }               
               
            ],
                                  


            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                //{
                //    xtype: 'toolbar',
                //    dock: 'top',
                //    items: [
                //        {
                //            xtype: 'buttongroup',
                //            columns: 2,
                //            items: [
                //                //{
                //                //    xtype: 'b4addbutton'
                //                //},
                //                {
                //                    xtype: 'b4updatebutton'
                //                }
                //            ]
                //        }
                //    ]
                //},
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