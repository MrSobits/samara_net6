Ext.define('B4.view.bankstatement.PaymentOrderOutGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeFinanceSource'
    ],

    title: 'Расход',
    store: 'bankstatement.PaymentOrderOut',
    itemId: 'paymentOrderOutGrid',
    closable: true,
    
    initComponent: function() {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'DocumentNum',
                   text: 'Номер',
                   width: 50
               },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BidNum',
                    text: 'Номер заявки',
                    width: 50
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BidDate',
                    text: 'Дата заявки',
                    format: 'd.m.Y',
                    width: 90
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма по документу',
                    width: 80,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeFinanceSource',
                    renderer: function (val) { return B4.enums.TypeFinanceSource.displayRenderer(val); },
                    text: 'Разрез финансирования',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayerContragentName',
                    text: 'Плательщик',
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReceiverContragentName',
                    text: 'Получатель',
                    flex: 1
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});