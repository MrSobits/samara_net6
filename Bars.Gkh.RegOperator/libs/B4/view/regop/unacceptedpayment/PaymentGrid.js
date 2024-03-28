Ext.define('B4.view.regop.unacceptedpayment.PaymentGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.regop.UnacceptedPayment',
        'B4.enums.PaymentType'
    ],

    alias: 'widget.unacceptedpaymentgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.UnacceptedPayment');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    text: 'Счет',
                    dataIndex: 'PersonalAccount',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Тип оплаты',
                    dataIndex: 'PaymentType',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.PaymentType.displayRenderer(val);
                    }
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    flex: 1,
                    renderer: function(val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата оплаты',
                    dataIndex: 'PaymentDate',
                    format: 'd.m.Y H:i',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
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