Ext.define('B4.view.billingaddressmatching.FiasGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    alias: 'widget.importedfiasgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.billingaddressmatching.FiasAddress");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    text: 'Код адреса',
                    dataIndex: 'Id',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
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
                    text: 'Литер',
                    dataIndex: 'Letter',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Корпус',
                    dataIndex: 'Housing',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Секция',
                    dataIndex: 'Building',
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
                }
            ]
        });

        me.callParent(arguments);
    }
});