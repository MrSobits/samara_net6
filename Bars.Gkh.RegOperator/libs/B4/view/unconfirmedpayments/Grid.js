Ext.define('B4.view.unconfirmedpayments.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.unconfirmedpaymentsgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.YesNo',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Неподтвержденные оплаты',
    store: 'B4.store.UnconfirmedPayments',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccount',
                    text: 'Лицевой счет',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    format: '0.00',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Guid',
                    text: 'Guid',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }, 
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'PaymentDate',
                    text: 'Дата оплаты',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BankBik',
                    text: 'БИК Банка',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BankName',
                    text: 'Наименование Банка',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    dataIndex: 'IsConfirmed',
                    text: 'Подтверждена',
                    width: 100,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    text: 'Файл',
                    flex: 1
                    //filter: { xtype: 'textfield' }
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
                            columns: 1,
                            items: [
                                { xtype: 'b4updatebutton' }
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