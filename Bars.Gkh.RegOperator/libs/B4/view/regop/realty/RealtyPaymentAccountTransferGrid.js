Ext.define('B4.view.regop.realty.RealtyPaymentAccountTransferGrid', {
    extend: 'B4.ux.grid.Panel',
    title: 'Операции по оплате',
    itemId: 'gridtest',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.regop.TransferForPaymentAccount',
        'B4.form.EnumCombo',
        'B4.enums.regop.PaymentOperationType',
        'B4.enums.regop.OperationStatus',
        'B4.ux.grid.column.Enum'
    ],
    alias: 'widget.realtypayatranfsergrid',
    type: null,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.regop.TransferForPaymentAccount');

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата',
                    dataIndex: 'OperationDate',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    text: 'Тип операции',
                    dataIndex: 'Reason',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: me.type == 'debet'
                        ? 'Плательщик'
                        : me.type == 'credit'
                            ? 'Получатель/основание'
                            : '',
                    dataIndex: 'OriginatorName',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Сумма (руб.)',
                    dataIndex: 'Amount',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!val) {
                            return '';
                        }

                        var rounded = Ext.util.Format.round(val, 2);
                        return Ext.util.Format.currency(rounded);
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