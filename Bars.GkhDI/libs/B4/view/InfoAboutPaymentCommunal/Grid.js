Ext.define('B4.view.infoaboutpaymentcommunal.Grid', {
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
    itemId: 'infoAboutPaymentCommunalGrid',
    alias: 'widget.infoaboutpaymentcommunalgrid',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseServiceName',
                    flex: 1,
                    text: 'Наименование услуги'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProviderName',
                    flex: 1,
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
                    dataIndex: 'TotalConsumption',
                    flex: 1,
                    text: 'Общий объем потребления',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Accrual',
                    flex: 1,
                    text: 'Начислено потребителям (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Payed',
                    flex: 1,
                    text: 'Оплачено потребителями (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Debt',
                    flex: 1,
                    text: 'Задолженность потребителей',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccrualByProvider',
                    flex: 1,
                    text: 'Начислено поставщиком (поставщиками) коммунального ресурса (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayedToProvider',
                    flex: 1,
                    text: 'Оплачено поставщику (поставщикам) коммунального ресурса (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtToProvider',
                    flex: 1,
                    text: 'Задолженность перед поставщиком (поставщиками) коммунального ресурса (руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReceivedPenaltySum',
                    flex: 1,
                    text: 'Сумма пени и штрафов, полученных от потребителей (руб.)',
                    editor: 'gkhdecimalfield'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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