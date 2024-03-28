Ext.define('B4.ux.grid.EntityChangeLogGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.entitychangeloggrid',
    title: 'История изменений',

    closable: false,


    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.EntityChangeLog',
        'B4.enums.ActionKindChangeLog',
        'Ext.ux.grid.FilterBar'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.EntityChangeLog');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'User',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Пользователь'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'ChangeDate',
                    width: 180,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    },
                    text: 'Дата и время изменения'
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActionKind',
                    flex: 1,
                    text: 'Действие',
                    enumName: 'B4.enums.ActionKindChangeLog',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Property',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Атрибут'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldValue',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Старое значение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewValue',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Новое значение'
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
                                    handler: function() {
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