Ext.define('B4.ux.grid.EntityHistoryInfoGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.entityhistoryinfogrid',
    title: 'История изменений',

    closable: false,

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKindChangeLog',
        'B4.store.EntityHistoryInfo',
        'Ext.ux.grid.FilterBar'
    ],

    enableColumnHide: true,
    groupType: null,
    parentName: null,
    entityName: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.EntityHistoryInfo', {
                groupType: me.groupType,
                parentName: me.parentName,
                entityName: me.entityName
            });

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    icon: B4.Url.content('content/img/icons/application_form_magnify.png'),
                    tooltip: 'Детальная информация',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'EditDate',
                    width: 180,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    },
                    text: 'Дата изменения'
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActionKind',
                    width: 100,
                    text: 'Действие',
                    enumName: 'B4.enums.ActionKind',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Username',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Имя пользователя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Login',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Логин пользователя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IpAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'IP'
                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        me.getStore().load();
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