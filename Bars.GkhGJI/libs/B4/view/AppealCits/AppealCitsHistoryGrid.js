Ext.define('B4.view.appealcits.AppealCitsHistoryGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appealcitshistorygrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.AppealOperationType',
        'B4.enums.TypeEntityLogging',
        'B4.ux.grid.column.Enum'
    ],

    store: 'appealcits.AppealCitsHistory',
    itemId: 'appealCitsHistoryGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AuditDate',
                    flex: 0.5,
                    text: 'Дата операции',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y H:i'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.AppealOperationType',
                    dataIndex: 'OperationType',
                    text: 'Тип операции',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeEntityLogging',
                    dataIndex: 'TypeEntityLogging',
                    text: 'Тип сущности',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OperatorLogin',
                    flex: 0.5,
                    text: 'Логин',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OperatorName',
                    flex: 0.5,
                    text: 'Имя пользователя',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Wording',
                    flex: 3,
                    text: 'Формулировка',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Json строка',
                    action: 'GetJsonString',
                    flex: 0.5,
                    items: [{
                        tooltip: 'Открыть',
                        iconCls: 'icon-fill-button',
                        icon: B4.Url.content('content/img/btnBrowse.png')
                    }]
                },
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
                            columns: 3,
                            items: [
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