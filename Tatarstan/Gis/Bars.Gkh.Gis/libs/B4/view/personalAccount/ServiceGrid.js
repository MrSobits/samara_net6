Ext.define('B4.view.personalAccount.ServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personalAccount_service_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.PersonalAccountService'
    ],

    title: 'Услуги по лицевому счету',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccountService');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'gridcolumn', dataIndex: 'Service', flex: 1, text: 'Услуга', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Supplier', flex: 1, text: 'Поставщик', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'Tariff', flex: 1, text: 'Формула', filter: { xtype: 'textfield' } }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 1,
                    items: [
                        { xtype: 'b4updatebutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});