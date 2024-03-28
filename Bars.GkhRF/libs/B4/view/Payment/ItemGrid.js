Ext.define('B4.view.payment.ItemGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: null,
    store: null,
    itemId: 'paymentItemGrid',
    closable: true,

    initComponent: function() {
        var me = this;
        var customNumberRenderer = function(val) {
            if (val != null && val.toString().indexOf('.') == -1)
                return val.toString();
            return Ext.util.Format.number(val, '0.00');
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManagingOrganizationName',
                    flex: 3,
                    text: 'Управляющая организация'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ChargeDate',
                    width: 110,
                    format: 'd.m.Y',
                    text: 'Дата начисления'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomeBalance',
                    flex: 1,
                    text: 'Входящее сальдо',
                    renderer: customNumberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Recalculation',
                    flex: 1,
                    text: 'Перерасчет прошлого периода',
                    renderer: customNumberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OutgoingBalance',
                    flex: 1,
                    text: 'Исходящее сальдо',
                    renderer: customNumberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePopulation',
                    flex: 1,
                    text: 'Начислено населению',
                    renderer: customNumberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidPopulation',
                    flex: 1,
                    text: 'Оплачено населением',
                    renderer: customNumberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalArea',
                    flex: 1,
                    text: 'Общая площадь',
                    renderer: customNumberRenderer
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
                    itemId: 'toolbarPayment',
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