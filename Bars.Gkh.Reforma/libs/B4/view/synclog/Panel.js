Ext.define('B4.view.synclog.Panel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.synclogpanel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.TypeIntegration'
    ],

    title: 'Лог интеграции с Реформой ЖКХ',

    layout: 'border',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    title: 'Сессии',
                    region: 'west',
                    xtype: 'b4grid',
                    width: 800,
                    split: true,
                    store: 'SyncSession',
                    itemId: 'sessionGrid',
                    columns: [
                        {
                            text: 'Начало',
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            dataIndex: 'StartTime',
                            width: 135,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            }
                        },
                        {
                            text: 'Окончание',
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            dataIndex: 'EndTime',
                            width: 135,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            }
                        },
                        {
                            text: 'Тип интеграции',
                            dataIndex: 'TypeIntegration',
                            width: 145,
                            filter: {
                                xtype: 'b4combobox',
                                items: B4.enums.TypeIntegration.getItemsWithEmpty([null, '-']),
                                editable: false,
                                operand: CondExpr.operands.eq,
                                valueField: 'Value',
                                displayField: 'Display'
                            },
                            renderer: function (val) { return B4.enums.TypeIntegration.displayRenderer(val); }
                        },
                        {
                            text: 'Идентификатор',
                            dataIndex: 'SessionId',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'SyncSession',
                            dock: 'bottom'
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ]
                },
                {
                    title: 'Действия',
                    region: 'center',
                    xtype: 'b4grid',
                    store: 'SyncAction',
                    itemId: 'actionGrid',
                    disabled: true,
                    columns: [
                        {
                            xtype: 'b4editcolumn'
                        },
                        {
                            text: 'Дата',
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            dataIndex: 'RequestTime',
                            width: 130,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            }
                        },
                        {
                            text: 'Действие',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Всего пакетов',
                            dataIndex: 'Total',
                            width: 90
                        },
                        {
                            text: 'Ошибок',
                            dataIndex: 'Failed',
                            width: 90
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'SyncAction',
                            dock: 'bottom'
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});