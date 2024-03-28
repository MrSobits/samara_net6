Ext.define('B4.view.actcheck.ActionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActCheckActionType'
    ],

    alias: 'widget.actcheckactiongrid',
    itemId: 'actCheckActionGrid',
    title: 'Действия',

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            store: 'actcheck.Action' + me.itemIdInnerMessage,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActionType',
                    enumName: 'B4.enums.ActCheckActionType',
                    text: 'Вид действия',
                    flex: 0.75
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    text: 'Дата',
                    format: 'd.m.Y',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreationPlace',
                    text: 'Место проведения',
                    flex: 1,
                    renderer: function (value) {
                        return value.AddressName;
                    }
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});