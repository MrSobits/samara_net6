Ext.define('B4.view.administration.executionaction.HistoryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.executionactionhistorygrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.ExecutionActionStatus',
        'Ext.ux.grid.FilterBar'
    ],

    storeName: 'B4.store.administration.executionaction.ExecutionActionStore',

    title: 'Результаты действий',

    initComponent: function() {
        var me = this,
            store = Ext.create(me.storeName);

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Переход к результатам',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.origScope;

                        // Если scope не задан в конфиге, то берем грид которому принадлежит наша колонка
                        if (!scope)
                            scope = this.up('grid');

                        scope.fireEvent('rowaction', scope, 'gotoresult', rec);
                    },
                    sortable: false
                },
                {
                    header: 'Id',
                    dataIndex: 'Id',
                    width: 70,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    sortable: false
                },
                {
                    header: 'Дата постановки в очередь',
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'CreateDate',
                    width: 150,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    },
                    sortable: false
                },
                {
                    header: 'Наименование',
                    dataIndex: 'Name',
                    flex: 5,
                    filter: { xtype: 'textfield' },
                    sortable: false
                },
                {
                    header: 'Описание',
                    dataIndex: 'Description',
                    flex: 6,
                    filter: { xtype: 'textfield' },
                    sortable: false
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ExecutionActionStatus',
                    filter: true,
                    header: 'Статус',
                    dataIndex: 'Status',
                    flex: 3,
                    sortable: false
                },
                {
                    header: 'Время выполнения',
                    dataIndex: 'Duration',
                    width: 120,
                    sortable: false
                },
            ],
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
                                    xtype: 'b4updatebutton'
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