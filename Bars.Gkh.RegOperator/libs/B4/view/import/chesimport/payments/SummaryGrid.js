Ext.define('B4.view.import.chesimport.payments.SummaryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.chesimportpaymentssummarygrid',

    requires: [
        'B4.ux.button.Update',
        'B4.enums.regop.WalletType',
        'B4.store.import.chesimport.payments.Summary'
    ],

    
    columnLines: true,
    title: 'Детализация',

    _decimalStyle: '<div style="font-size: 11px; line-height: 13px;">{0}</div>',
    _textStyle: '<div style="font-size: 11px; line-height: 13px; text-align: right">{0}</div>',

    initComponent: function () {
        var me = this,
            store = me.store || Ext.create('B4.store.import.chesimport.payments.Summary'),
            decimalRenderer = function (val, cls, record) {
                return record.get('Name') === 'Количество' ? val : Ext.util.Format.currency(val || 0);
            };

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    sortable: false,
                    text: '',
                    width: 150,
                    renderer: function(val) {
                        return Ext.String.format(me._textStyle, val + ':');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Paid',
                    text: 'Оплачено',
                    width: 120,
                    renderer: decimalRenderer,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cancelled',
                    text: 'Отмена оплат',
                    width: 120,
                    renderer: decimalRenderer,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Refund',
                    text: 'Возврат',
                    width: 120,
                    renderer: decimalRenderer,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Итого',
                    width: 120,
                    renderer: decimalRenderer,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Count',
                    text: 'Количество',
                    sortable: false,
                    width: 120,
                    renderer: function (val, cls, record) {
                        if (record.get('Name') === 'Количество') {
                            cls.style = 'background: #c5c5c5;'
                            return '';
                        }
                        return val;
                    }
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
