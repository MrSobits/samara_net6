Ext.define('B4.view.infoaboutpaymentcommunal.InlineGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],

    store: 'InfoAboutPaymentCommunal',
    itemId: 'infoAboutPaymentCommunalInlineGrid',
    alias: 'widget.infoaboutpaymentcommunalinlinegrid',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseServiceName',
                    flex: 2,
                    text: 'Наименование услуги'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProviderName',
                    flex: 3,
                    text: 'Поставщик услуги'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CounterValuePeriodStart',
                    flex: 1,
                    text: 'Показания счетчика на начало периода',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CounterValuePeriodEnd',
                    flex: 1,
                    text: 'Показания счетчика на конец периода',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Accrual',
                    flex: 1,
                    text: 'Начислено (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Payed',
                    flex: 1,
                    text: 'Оплачено (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Debt',
                    flex: 1,
                    text: 'Долг нарастающим платежом',
                    editor: 'gkhdecimalfield'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
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