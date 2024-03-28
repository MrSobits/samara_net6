Ext.define('B4.view.regop.report.PaymentDocumentSnapshotInfoWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paymentdocinfowindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.store.regop.report.AccountPaymentInfoSnapshot'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Документ на оплату',
    width: 1024,
    height: 768,
    bodyPadding: 5,
    modal: true,

    autoScroll: true,
    shrinkWrap: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.report.AccountPaymentInfoSnapshot');

        store.on('beforeload', function(st, opts) {
            return this.fireEvent('beforeload', this, st, opts);
        }, me);

        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 100, labelAlign: 'right' },
                    items: [
                        { name: 'DocNum', fieldLabel: 'Номер документа' },
                        { name: 'DocDate', fieldLabel: 'От' },
                        { name: 'PeriodName', fieldLabel: 'Период' }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Плательщик',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                            items: [
                                { name: 'PayerName', fieldLabel: 'Плательщик', xtype: 'textarea' },
                                { name: 'RoomAddress', fieldLabel: 'Адрес', xtype: 'textarea' }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                            items: [
                                { name: 'OwnerInn', fieldLabel: 'ИНН плательщика' },
                                { name: 'AccountNumber', fieldLabel: 'Лицевой счет' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Получатель',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                            items: [
                                { name: 'ReceiverName', fieldLabel: 'Получатель', xtype: 'textarea' },
                                { name: 'ReceiverAddress', fieldLabel: 'Адрес', xtype: 'textarea' }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            layout: 'hbox',
                            defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                            items: [
                                { name: 'ReceiverInn', fieldLabel: 'ИНН получателя' },
                                { name: 'ReceiverKpp', fieldLabel: 'КПП получателя' }
                            ]
                        },
                        { name: 'ReceiverAccountNumber', fieldLabel: 'Расчетный счет' },
                        { name: 'ReceiverBankAddress', fieldLabel: 'Адрес банка' }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    title: 'Итоговые суммы',
                    defaults: { xtype: 'textfield', readOnly: true, flex: 1, labelWidth: 150, labelAlign: 'right' },
                    items: [
                        { name: 'Charge', fieldLabel: 'Взносы за кап. ремонт', value: 0 },
                        { name: 'Penalty', fieldLabel: 'Пени за несвоевременную оплату', value: 0 },
                        { name: 'Recalc', fieldLabel: 'Перерасчет', value: 0 }
                    ]
                },
                {
                    xtype: 'b4grid',
                    minHeight: 300,
                    flex: 1,
                    store: store,
                    columns: [
                        { header: 'Номер ЛС', flex: 1, dataIndex: 'AccountNumber', filter: { xtype: 'textfield' } },
                        { header: 'Адрес помещения', flex: 3, dataIndex: 'RoomAddress', filter: { xtype: 'textfield' } },
                        { header: 'Тип помещения', flex: 1, dataIndex: 'RoomType', xtype: 'b4enumcolumn', enumName: 'B4.enums.realty.RoomType' },
                        { header: 'Площадь помещения', flex: 1, dataIndex: 'Area', filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                        { header: 'Тариф', flex: 1, dataIndex: 'Tariff', filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                        { header: 'Наименование работ', flex: 1, dataIndex: 'Services', filter: { xtype: 'textfield' } },
                        { header: 'Сумма начислений', flex: 1, dataIndex: 'ChargeSum', filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                        { header: 'Сумма начислений пени', flex: 1, dataIndex: 'PenaltySum', filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        { xtype: 'b4updatebutton' }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Выгрузить документ',
                                    action: 'download'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть',
                                    listeners: {
                                        'click': function(btn) {
                                            btn.up('paymentdocinfowindow').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    loadAccounts: function() {
        this.down('b4grid').getStore().load();
    }
});