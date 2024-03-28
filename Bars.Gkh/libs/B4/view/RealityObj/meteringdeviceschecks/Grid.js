﻿Ext.define('B4.view.realityobj.meteringdeviceschecks.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.realityobj.MeteringDevicesChecks'
    ],

    title: 'Приборы учета',
    alias: 'widget.meteringDevicesChecksGrid',
    closable: true,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.realityobj.MeteringDevicesChecks');

            Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    text: 'Прибор учёта',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MarkMeteringDevice',
                    flex: 1,
                    text: 'Марка прибора учёта',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDateCheck',
                    flex: 1,
                    text: 'Дата окончания проверки',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }

                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'NextDateCheck',
                    flex: 1,
                    text: 'Плановая дата следующей проверки',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
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