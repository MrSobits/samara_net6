Ext.define('B4.view.gkuinfo.TarifsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Тарифы ЖКУ',
    alias: 'widget.gkuinfotarifgrid',
    closable: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: ['Service', 'Tariff', 'Month', 'Supplier'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'RealityObject',
                    listAction: 'ListGkuInfoTarifs'
                },
                autoLoad: true
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    text: 'Услуга',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    text: 'Тариф',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Month',
                    text: 'Месяц',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Supplier',
                    text: 'Поставщик',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Месяц',
                            name: 'month',
                            format: 'd.m.Y',
                            labelAlign: 'right',
                            labelWidth: 50
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