Ext.define('B4.view.realityobj.entrance.RoomGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjentranceroomgrid',
    requires: [
        'B4.store.realityobj.Room',
        'B4.ux.grid.toolbar.Paging',
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.Room', {
                fields: [
                    { name: 'RoomNum', sortType: 'asInt' },
                    { name: 'Area' }
                ]
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { text: '№ квартиры/помещения', flex: 1, dataIndex: 'RoomNum' },
                { text: 'Общая площадь', flex: 1, dataIndex: 'Area' }
            ],
            viewConfig: { loadMask: true },
            dockedItems: [
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