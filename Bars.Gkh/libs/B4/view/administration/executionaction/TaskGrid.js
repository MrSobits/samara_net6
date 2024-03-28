Ext.define('B4.view.administration.executionaction.TaskGrid',
{
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.ux.RowExpander'
    ],

    alias: 'widget.executionactiontaskgrid',
    title: 'Запланированные действия',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.executionaction.ExecutionActionTask');

        Ext.applyIf(me,
        {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'Login',
                    text: 'Пользователь'
                },
                {
                    header: 'Наименование',
                    dataIndex: 'Name',
                    flex: 5,
                    filter: { xtype: 'textfield' },
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'TriggerName',
                    text: 'Периодичность'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'CreateDate',
                    text: 'Время добавления'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'StartDate',
                    text: 'Дата начала действия задачи'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'EndDate',
                    text: 'Дата окончания действия задачи'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                    name: 'buttons',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        var me = this;
                                        me.up('grid').getStore().load();
                                    }
                                },
                                {
                                    xtype: 'button',
                                    name: 'QueueInfo',
                                    text: 'Информация об очереди',
                                    icon: B4.Url.content('content/img/icons/information.png'),
                                },
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