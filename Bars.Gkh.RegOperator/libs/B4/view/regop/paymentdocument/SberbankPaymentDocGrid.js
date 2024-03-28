Ext.define('B4.view.regop.paymentdocument.SberbankPaymentDocGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.sberbankpaymentdocgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',
        'B4.form.SelectField',
    ],
    closable: true,
    title: 'Реестр квитанций на оплату Сбербанку',

    initComponent: function () {
        var me = this, store = Ext.create('B4.store.regop.paymentdocument.SberbankPaymentDoc');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Period',
                    flex: 1,
                    text: 'Период',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccNumber',
                    flex: 1,
                    text: 'Лицевой счет',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата и время последнего формирования',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'LastDate',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Count',
                    flex: 1,
                    text: 'Количество сформированных',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GUID',
                    flex: 1,
                    text: 'GUID пользователя',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentDocFile',
                    width: 100,
                    text: 'Квитанция',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                    items:
                    {
                        xtype: 'buttongroup',
                        items: [
                            {
                                xtype: 'button',
                                text: 'Сформировать реестр',
                                textAlign: 'left',
                                itemId: 'createReestrButton',
                                actionName: 'createReestr',
                                tooltip: 'Сформировать реестр',
                                iconCls: 'icon-accept'
                            },
                            {
                                xtype: 'b4updatebutton',
                                handler: function (button, e) {
                                    button.up('sberbankpaymentdocgrid').getStore().load();
                                }
                            }
                        ]
                    }
                },
                ,
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});