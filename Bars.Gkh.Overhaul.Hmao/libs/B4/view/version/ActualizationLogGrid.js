Ext.define('B4.view.version.ActualizationLogGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.actualizationloggrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.VersionActualizeType'
    ],

    closable: false,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.version.VersionActualizeLogRecord');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'DateAction',
                    text: 'Дата действия',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.VersionActualizeType',
                    dataIndex: 'ActualizeType',
                    flex: 1,
                    text: 'Тип актуализации',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    flex: 1,
                    text: 'Действие',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InputParams',
                    flex: 1,
                    text: 'Входные параметры',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkCode',
                    flex: 1,
                    text: 'Код работы',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Ceo',
                    flex: 1,
                    text: 'ООИ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'PlanYear',
                    flex: 1,
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'ChangePlanYear',
                    flex: 1,
                    text: 'Изменение: Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'PublishYear',
                    flex: 1,
                    text: 'Опубликованный год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'ChangePublishYear',
                    flex: 1,
                    text: 'Изменение: Опубликованный год',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    flex: 1,
                    text: 'Объем',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChangeVolume',
                    flex: 1,
                    text: 'Изменение: Объем',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChangeSum',
                    flex: 1,
                    text: 'Изменение: Сумма',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    dataIndex: 'ChangeNumber',
                    flex: 1,
                    text: 'Изменение: Номер',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '0 10 0 0',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStart',
                                            fieldLabel: 'Период внесения изменений с',
                                            format: 'd.m.Y',
                                            value: new Date(),
                                            labelWidth: 170,
                                            flex: 0.65
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEnd',
                                            fieldLabel: 'по',
                                            format: 'd.m.Y',
                                            value: new Date(),
                                            labelWidth: 30,
                                            flex: 0.35
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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