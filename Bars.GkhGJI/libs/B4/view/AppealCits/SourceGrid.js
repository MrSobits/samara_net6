﻿Ext.define('B4.view.appealcits.SourceGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.appealCitsSourceGrid',
    store: 'appealcits.Source',
    itemId: 'appealCitsSourceGrid',

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
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueForm',
                    flex: 2,
                    text: 'Форма поступления'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceNumber',
                    width: 100,
                    text: 'Номер'
                },
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'RevenueDate',
                //    width: 100,
                //    text: 'Дата поступления',
                //    format: 'd.m.Y',
                //    renderer: function (v) {
                //        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                //            v = undefined;
                //        }
                //        return Ext.util.Format.date(v, 'd.m.Y');
                //    }
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SSTUDate',
                    width: 100,
                    text: 'Дата источника',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSource',
                    flex: 1,
                    text: 'Источник'
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
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});