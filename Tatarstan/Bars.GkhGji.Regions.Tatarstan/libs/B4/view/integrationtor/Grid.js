Ext.define('B4.view.integrationtor.Grid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.integrationtorgrid',

        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.store.integrationtor.TorTask',
            'B4.ux.grid.column.Enum',
            'B4.enums.TypeRequest',
            'B4.enums.TypeObject',
            'B4.enums.TorTaskState'
        ],

        title: 'Интеграция с ТОР КНД',
        closable: true,

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.integrationtor.TorTask');
            Ext.applyIf(me, {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'b4enumcolumn',
                        dataIndex: 'SendObject',
                        text: 'Объект',
                        flex: 1,
                        enumName: 'B4.enums.TypeObject',
                        filter: true
                    },
                    {
                        xtype: 'b4enumcolumn',
                        dataIndex: 'TypeRequest',
                        text: 'Метод',
                        flex: 1,
                        enumName: 'B4.enums.TypeRequest',
                        filter: true
                    },
                    {
                        xtype: 'b4enumcolumn',
                        dataIndex: 'TaskState',
                        text: 'Статус задачи',
                        flex: 1,
                        enumName: 'B4.enums.TorTaskState',
                        filter: true
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TorId',
                        text: 'Идентификатор в ТОР КНД',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'RegistrationTorDate',
                        text: 'Дата присвоения идентификатора в ТОР КНД',
                        flex: 1,
                        format: 'd.m.Y H:i:s',
                        filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                    },
                    {
                        xtype: 'actioncolumn',
                        name: 'getRequest',
                        text: 'Запрос',
                        width: 50,
                        align: 'center',
                        icon: 'content/img/searchfield-icon.png'
                    },
                    {
                        xtype: 'actioncolumn',
                        name: 'getResponse',
                        text: 'Ответ',
                        width: 50,
                        align: 'center',
                        icon: 'content/img/searchfield-icon.png'
                    },
                    {
                        xtype: 'actioncolumn',
                        text: 'Лог',
                        name: 'getLog',
                        width: 40,
                        align: 'center',
                        icon: 'content/img/icons/disk.png'
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
                                        xtype: 'b4updatebutton',
                                        listeners: {
                                            click: function () {
                                                store.load();
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        name: 'getSubjectsButton',
                                        text: 'Получить субъекты'
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