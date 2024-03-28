Ext.define('B4.view.realityobj.entrance.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjentrancegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',

        'B4.store.realityobj.Entrance'
    ],

    title: 'Сведения о подъездах',
    layout: 'fit',
    closable: true,
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.Entrance');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'Number',
                    text: 'Номер подъезда',
                    flex: 1
                },
                {
                    dataIndex: 'RealEstateType',
                    text: 'Тип дома',
                    flex: 1
                },
                {
                    dataIndex: 'Tariff',
                    text: 'Тариф',
                    flex: 1,
                    sortable: false,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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