Ext.define('B4.view.realityobj.decisionhistory.JobYearsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.decisionhistoryjobyearsgrid',
    
    requires: [
        'B4.ux.button.Update',
        'B4.store.realityobj.decisionhistory.JobYears'
    ],

    title: 'Перечень работ по КР МКД',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.decisionhistory.JobYears');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    text: 'Наименование работы',
                    flex: 1,
                    dataIndex: 'JobName',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Установленная норма',
                    flex: 1,
                    dataIndex: 'PlanYear',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Принятое решение',
                    flex: 1,
                    dataIndex: 'UserYear',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Протокол',
                    flex: 1,
                    dataIndex: 'Protocol',
                    filter: {
                        xtype: 'textfield'
                    }
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
                            columns: 2,
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