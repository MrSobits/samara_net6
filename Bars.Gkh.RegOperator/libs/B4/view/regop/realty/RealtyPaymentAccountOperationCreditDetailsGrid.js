Ext.define('B4.view.regop.realty.RealtyPaymentAccountOperationCreditDetailsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realtypayaccopercreddetailsgrid',
    
    requires: [
        'B4.enums.ActPaymentSrcFinanceType',
        'B4.store.regop.realty.RealtyPaymentAccountOperationBySources'
    ],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.realty.RealtyPaymentAccountOperationBySources');

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
                    dataIndex: 'Sum',
                    width: 100,
                    text: 'Сумма, руб.',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.00
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});