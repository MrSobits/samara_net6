﻿Ext.define('B4.view.actcheck.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.actCheckViolationGrid',
    title: 'Нарушения',
    store: 'actcheck.Violation',
    itemId: 'actCheckViolationGrid',
    border: true,
    selectionSavingBuffer: 10,
    viewConfig: {
        autoFill: true
    },
    clicksToEdit: 1,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiName',
                    flex: 1,
                    text: 'Нарушение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionDescription',
                    flex: 1,
                    text: 'Подробнее'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionAction',
                    flex: 1,
                    text: 'Мероприятие'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                /* В акте нарушения можно только просматривать
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'actViolationGridAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },*/
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