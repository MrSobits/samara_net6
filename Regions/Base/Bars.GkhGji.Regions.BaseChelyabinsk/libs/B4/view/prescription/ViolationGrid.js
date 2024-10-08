﻿Ext.define('B4.view.prescription.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'CodesPin',
                    text: 'Пункты НПД',
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
                    dataIndex: 'Description',
                    flex: 1.3,
                    text: 'Нарушение для редактирования',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1500
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Features',
                    flex: 0.5,
                    text: 'Характеристика нарушений',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    flex: 0.5,
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
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'NotificationDate',
                    text: 'Срок уведомления',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanExtension',
                    text: 'Продленный срок',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    width: 130,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    iconCls: 'icon-arrow-out',
                                    text: 'Установить плановую дату всем нарушениям'
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