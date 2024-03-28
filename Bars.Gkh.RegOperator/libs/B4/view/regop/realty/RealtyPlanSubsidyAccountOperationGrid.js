Ext.define('B4.view.regop.realty.RealtyPlanSubsidyAccountOperationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.regop.realty.RealtyPlanSubsidyAccountOperation'
    ],
    alias: 'widget.realtyplansubsidyoperationgrid',

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.regop.realty.RealtyPlanSubsidyAccountOperation');

        Ext.apply(me, {
            cls: 'x-large-head',
            columns: [
                {
                    text: 'Месяц',
                    dataIndex: 'DateString',
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Федеральный стандарт',
                    dataIndex: 'FederalStandardFee',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Тариф на МО ',
                    dataIndex: 'Tariff',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Площадь помещений',
                    dataIndex: 'Area',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Кол-во',
                    dataIndex: 'Days',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    }
                },               
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                }
            ],


            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});