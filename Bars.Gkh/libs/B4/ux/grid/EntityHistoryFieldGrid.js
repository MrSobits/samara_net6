Ext.define('B4.ux.grid.EntityHistoryFieldGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.entityhistoryfieldgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKindChangeLog',
        'B4.store.EntityHistoryField',
        'Ext.ux.grid.FilterBar'
    ],

    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.EntityHistoryField');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FieldName',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Наименование поля'
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