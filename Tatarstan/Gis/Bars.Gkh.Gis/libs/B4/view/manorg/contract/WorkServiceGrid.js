﻿Ext.define('B4.view.manorg.contract.WorkServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeWork'
    ],

    title: 'Работы/услуги организации по управлению домом',
    closable: false,
    gridStore: '',

    initComponent: function () {
        var me = this,
            exactStore = Ext.create(me.gridStore);

        Ext.applyIf(me, {
            columnLines: true,
            store: exactStore,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Тип работ',
                    enumName: 'B4.enums.TypeWork',
                    filter: true
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'PaymentAmount',
                    text: 'Размер платы (цена) за услуги/работы по управлению домом',
                    format: '0.00',
                    flex: 4,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    allowBlank: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    text: 'Дата начала предоставления',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    allowBlank: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    allowBlank: false
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
                    store: exactStore,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});