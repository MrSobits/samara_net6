﻿Ext.define('B4.view.protocolgji.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.protocolgjiViolationGrid',
    store: 'protocolgji.Violation',
    itemId: 'protocolgjiViolationGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    dataIndex: 'CodesPin',
                    text: 'Пункты НПД',
                    width: 80,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'ViolationGji',
                    flex: 1,
                    text: 'Текст нарушения',
                    filter: { xtype: 'textfield' },
                    renderer: function(val, metaData) {
                        metaData.tdAttr = 'data-qtip="' + val + '"';
                        return val;
                    }
                },
                {
                    dataIndex: 'ActDescription',
                    flex: 1,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Features',
                    flex: 0.5,
                    text: 'Характеристика нарушений',
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
                    itemId: 'cdfDatePlanRemoval'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    width: 130,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    itemId: 'cdfDateFactRemoval'
                },
                {
                    dataIndex: 'Description',
                    text: 'Примечание',
                    flex: 0.5,
                    filter: { xtype: 'textfield' },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
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
                                    itemId: 'protocolViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'updateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
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