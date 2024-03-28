Ext.define('B4.view.administration.risdataexport.ExportTaskGrid',
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

    alias: 'widget.risdataexporttaskgrid',
    title: 'Запланированные задачи',

    plugins: [
        {
            ptype: 'rowexpander',
            pluginId: 'rowExpander',
            expandOnDblClick: false,
            rowBodyTpl: [
                '<p>',
                '<tpl if="EntityGroupCodeList">',
                '<b>Выбранные секции: </b>',
                '<tpl for="EntityGroupCodeList">',
                '<p>{.}</p>',
                '</tpl>',
                '</tpl>',
                '</p>'
            ]
        }
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.risdataexport.FormatDataExportTask');

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
                    width: 75,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'Id',
                    text: 'Id'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'Login',
                    text: 'Пользователь'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
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