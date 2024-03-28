Ext.define('B4.view.fssp.courtordergku.PgmuAddressGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.fssp.courtordergku.PgmuAddress'
    ],
    
    alias: 'widget.pgmuaddressgrid',
    hiddenToolbar: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.fssp.courtordergku.PgmuAddress'),
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'single'
            });

        me.relayEvents(store, ['load'], 'pgmuAddressStore.');
        me.relayEvents(selModel, ['select'], 'pgmuAddressGrid.');

        Ext.applyIf(me, {
            columnLines: true,
            selModel: selModel,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErcCode',
                    text: 'Код ЕРЦ',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PostCode',
                    filter: { xtype: 'textfield' },
                    text: 'Индекс',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'District',
                    filter: { xtype: 'textfield' },
                    text: 'Район',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Town',
                    filter: { xtype: 'textfield' },
                    text: 'Город',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Street',
                    filter: { xtype: 'textfield' },
                    text: 'Улица',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'House',
                    filter: { xtype: 'textfield' },
                    text: 'Дом',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Building',
                    filter: { xtype: 'textfield' },
                    text: 'Корпус',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Apartment',
                    filter: { xtype: 'textfield' },
                    text: 'Квартира',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Room',
                    filter: { xtype: 'textfield' },
                    text: 'Комната',
                    flex: 1
                },

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
                    hidden: me.hiddenToolbar,
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Применить'
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