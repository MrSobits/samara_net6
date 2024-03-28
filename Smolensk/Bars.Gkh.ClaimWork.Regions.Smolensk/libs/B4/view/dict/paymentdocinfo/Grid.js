﻿Ext.define('B4.view.dict.paymentdocinfo.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.PaymentDocInfo'
    ],

    title: 'Информация для физ.лиц',
    alias: 'widget.paymentdocinfogrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.PaymentDocInfo');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
               {
                    xtype: 'b4editcolumn',
                    scope: me
               },
               {
                    text: 'Район',
                    dataIndex: 'Municipality',
                   flex: 1,
                   sortable: false,
                   filter: { xtype: 'textfield' }
               },
               {
                   text: 'Муниципальное образование',
                   dataIndex: 'MoSettlement',
                   flex: 1,
                   filter: { xtype: 'textfield' },
                   sortable: false,
                   renderer: function (val) {
                       return val && val.Name ? val.Name : '';
                   }
               },
               {
                   text: 'Населенный пункт',
                   dataIndex: 'LocalityName',
                   flex: 1,
                   filter: { xtype: 'textfield' }
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    text: 'Дом',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата действия с',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата действия по',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});