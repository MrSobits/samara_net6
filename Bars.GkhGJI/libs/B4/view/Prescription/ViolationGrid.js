﻿Ext.define('B4.view.prescription.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.prescriptionViolationGrid',
    store: 'prescription.Violation',
    itemId: 'prescriptionViolationGrid',

    initComponent: function () {
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
                    dataIndex: 'ViolationGjiPin',
                    text: 'Код ПиН',
                    width: 100,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    flex: 1.3,
                    text: 'Текст нарушения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    flex: 1,
                    text: 'Мероприятие',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    name: 'DateFactRemoval',
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    width: 120,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        readOnly: true
                    }
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
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'prescriptionViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'updateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'setCommonRemovalDate',
                                    iconCls: 'icon-cross',
                                    text: 'Обновить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'pagingtoolbar',
                    store: 'prescription.Violation',
                    dock: 'bottom',
                    displayInfo: true,
                    displayMsg: 'Всего записей {2}',
                    getPagingItems: function () {
                        return [];
                    },
                    onLoad: function () {
                        Ext.suspendLayouts();
                        this.updateInfo();
                        Ext.resumeLayouts(true);
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});