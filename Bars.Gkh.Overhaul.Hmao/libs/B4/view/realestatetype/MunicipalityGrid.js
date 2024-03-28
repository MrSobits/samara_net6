Ext.define('B4.view.realestatetype.MunicipalityGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.retmunicipalitygrid',

    requires: [
        'B4.store.realestatetype.Municipality',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete'
    ],

    store: 'realestatetype.Municipality',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {

            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'b4deletecolumn'
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});