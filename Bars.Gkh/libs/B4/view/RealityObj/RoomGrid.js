Ext.define('B4.view.realityobj.RoomGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjroomgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.realty.RoomType',
        'B4.enums.RoomOwnershipType'
    ],

    title: 'Сведения о помещениях',
    store: 'realityobj.Room',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomNum',
                    flex: 1,
                    text: '№ квартиры/помещения'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    flex: 1,
                    text: 'Общая площадь'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Тип помещения',
                    renderer: function (val) {
                        return B4.enums.realty.RoomType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnershipType',
                    flex: 1,
                    text: 'Тип собственности',
                    renderer: function (val) {
                        return B4.enums.RoomOwnershipType.displayRenderer(val);
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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