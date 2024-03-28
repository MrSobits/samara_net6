Ext.define('B4.view.administration.executionaction.ResultGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.executionactionresultgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.ExecutionActionStatus',
        'Ext.ux.grid.FilterBar',
        'B4.store.administration.executionaction.ExecutionActionResult'
    ],

    title: 'Результаты действий',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.executionaction.ExecutionActionResult');

        Ext.apply(me, {
            store: store,
            columns: {
                items: [
                    {
                        xtype: 'actioncolumn',
                        width: 20,
                        tooltip: 'Переход к результатам',
                        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                        handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                            var me = this,
                                scope = me.origScope;
                            // Если scope не задан в конфиге, то берем грид которому принадлежит наша колонка
                            if (!scope)
                                scope = me.up('grid');

                            scope.fireEvent('rowaction', scope, 'gotoresult', rec);
                        }
                    },
                    {
                        header: 'Наименование',
                        dataIndex: 'Name',
                        flex: 2,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        header: 'Описание',
                        dataIndex: 'Description',
                        flex: 5,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        header: 'Дата запуска',
                        xtype: 'datecolumn',
                        format: 'd.m.Y H:i:s',
                        dataIndex: 'StartDate',
                        width: 130,
                        filter: {
                            xtype: 'datefield',
                            operand: CondExpr.operands.eq,
                            format: 'd.m.Y H:i:s'
                        }
                    },
                    {
                        header: 'Дата завершения',
                        xtype: 'datecolumn',
                        format: 'd.m.Y H:i:s',
                        dataIndex: 'EndDate',
                        width: 130,
                        filter: {
                            xtype: 'datefield',
                            operand: CondExpr.operands.eq,
                            format: 'd.m.Y H:i:s'
                        }
                    },
                    {
                        xtype: 'b4enumcolumn',
                        enumName: 'B4.enums.ExecutionActionStatus',
                        filter: true,
                        header: 'Статус',
                        dataIndex: 'Status',
                        width: 170,
                    },
                    {
                        header: 'Время выполнения',
                        dataIndex: 'Duration',
                        width: 120
                    },
                ]
            },
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
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
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        var me = this;
                                        me.up('grid').getStore().load();
                                    }
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