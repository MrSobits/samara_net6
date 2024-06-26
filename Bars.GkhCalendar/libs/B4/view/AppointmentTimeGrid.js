﻿Ext.define('B4.view.AppointmentTimeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appointmenttimegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeOrganisation',
        'B4.enums.DayOfWeekRus'
    ],

    title: 'Время приёма',
    store: 'AppointmentTimeGridStore',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.DayOfWeekRus',
                //    dataIndex: 'DayOfWeek',
                //    text: 'День недели',
                //    flex: 1,
                //    filter: true,
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartTime',
                    flex: 1,
                    text: 'StartTime',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndTime',
                    flex: 1,
                    text: 'EndTime',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'StarPauseTime',
                //    flex: 1,
                //    text: 'StarPauseTime',
                //    format: 'd.m.Y',
                //    filter: {
                //        xtype: 'textfield',
                //    },
                //},
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'EndPauseTime',
                //    flex: 1,
                //    text: 'EndPauseTime',
                //    format: 'd.m.Y',
                //    filter: {
                //        xtype: 'textfield',
                //    },
                //},
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});