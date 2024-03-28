Ext.define('B4.view.gisaddressmatching.FiasGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    alias: 'widget.fiasgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.gisaddressmatching.FiasAddress");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    text: 'Населенный пункт',
                    dataIndex: 'CityName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Район',
                    dataIndex: 'RegionName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Улица',
                    dataIndex: 'StreetName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер дома',
                    dataIndex: 'Number',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            dockedItems: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    name: 'fiasAddrInfo'
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});