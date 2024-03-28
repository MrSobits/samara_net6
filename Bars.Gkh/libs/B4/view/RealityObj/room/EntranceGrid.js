Ext.define('B4.view.realityobj.room.EntranceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.entrancegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.realty.RoomType',
        'B4.enums.RoomOwnershipType',
        'B4.form.ComboBox',
        'B4.store.realityobj.Entrance'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.Entrance'),
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'SINGLE'
            });

        Ext.applyIf(me, {
            store: store,
            selModel: selModel,
            columnLines: true,
            columns: [
                {
                    dataIndex: 'Number',
                    text: 'Номер подъезда',
                    width: 100
                },
                {
                    dataIndex: 'RealEstateType',
                    flex: 1,
                    text: 'Тип дома'
                },
                {
                    dataIndex: 'Tariff',
                    text: 'Тариф',
                    sortable: false,
                    width: 100
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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