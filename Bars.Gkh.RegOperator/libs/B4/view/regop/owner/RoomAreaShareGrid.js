Ext.define('B4.view.regop.owner.RoomAreaShareGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.regop.owner.RoomAreaShare',
        'Ext.ux.CheckColumn',
        'B4.view.Control.GkhDecimalField'
    ],

    alias: 'widget.roomareasharegrid',

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.regop.owner.RoomAreaShare');

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Checked',
                    width: 40,
                    sortable: false
                },
                {
                    text: 'Номер квартиры',
                    dataIndex: 'RoomNum',
                    flex: 3,
                    filter: {
                        xtype: 'numberfield'
                    }
                },
                {
                    text: 'Номер комнаты',
                    dataIndex: 'ChamberNum',
                    flex: 3,
                    filter: {
                        xtype: 'numberfield'
                    }
                },
                {
                    text: 'Доля собственности',
                    dataIndex: 'AreaShare',
                    flex: 3,
                    editor: {
                        xtype: 'gkhdecimalfield',                        
                        negativeText: 'Значение не может быть отрицательным',
                        decimalPrecision: 5,
                        minValue: 0,
                        maxValue: 1
                    },
                    sortable: false
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        'beforeedit': function (cmp, e) {
                            return !!e.record.get('Checked');
                        }
                    }
                })
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ],
            viewConfig: {
                loadMask: true
            }

        });
        me.callParent(arguments);

    }
});