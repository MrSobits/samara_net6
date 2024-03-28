Ext.define('B4.view.objectcr.performedworkact.PaymentDetailsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.perfworkactpaymentdetailsgrid',
    requires: [
        'B4.grid.feature.Summary',
        'B4.view.Control.GkhDecimalField',
        'B4.store.objectcr.performedworkact.PaymentDetails',
        'B4.enums.ActPaymentSrcFinanceType'
    ],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.performedworkact.PaymentDetails');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SrcFinanceType',
                    flex: 1,
                    text: 'Источник поступления',
                    renderer: function (val) {
                        return B4.enums.ActPaymentSrcFinanceType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Balance',
                    width: 100,
                    text: 'Сальдо, руб.'
                    //Джамиля сказала убрать округление
                    //renderer: function (val) {
                    //    return Ext.util.Format.currency(val);
                    //}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Payment',
                    width: 100,
                    text: 'Оплата, руб.',
                    //renderer: function (val) {
                    //    return Ext.util.Format.currency(val);
                    //},
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.00
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});