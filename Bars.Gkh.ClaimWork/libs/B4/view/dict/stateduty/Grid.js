Ext.define('B4.view.dict.stateduty.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.statedutygrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.store.dict.StateDuty'
    ],

    closable: true,
    title: 'Госпошлина',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.StateDuty');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    header: 'Тип суда',
                    dataIndex: 'CourtType',
                    enumName: 'B4.enums.CourtType',
                    filter: true,
                    flex: 1
                },
                {
                    header: 'Формула',
                    dataIndex: 'Formula',
                    flex: 3
                },
                {
                     xtype: 'b4deletecolumn'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                { xtype: 'b4pagingtoolbar', store: store, dock: 'bottom', displayInfo: true }
            ]
        });

        me.callParent(arguments);
    }
});