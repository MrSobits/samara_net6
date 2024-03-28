Ext.define('B4.view.suggestion.TransitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.suggestion.Transition',
        'B4.enums.ExecutorType',
        'B4.ux.grid.column.Enum'
    ],

    alias: 'widget.transitiongrid',

    title: 'Настройка правил перехода',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.suggestion.Transition');

        Ext.apply(me, {
            store: store,

            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'InitialExecutorType',
                    text: 'Смена исполнителя с',
                    enumName: 'B4.enums.ExecutorType',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TargetExecutorType',
                    text: 'Смена исполнителя на',
                    enumName: 'B4.enums.ExecutorType',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExecutionDeadline',
                    text: 'Срок исполнения (дней)',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    name: 'ValidateBP',
                                    text: 'Проверка БП'
                                },
                                {
                                    xtype: 'button',
                                    name: 'RunBP',
                                    text: 'Запуск процесса'
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