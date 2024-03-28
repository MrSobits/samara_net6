Ext.define('B4.view.resolution.PayFineEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minHeight: 100,
    maxHeight: 100,
    bodyPadding: 5,
    itemId: 'resolutionPayFineEditWindow',
    title: 'Форма редактирования оплаты штрафа',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'PayReg',
                    fieldLabel: 'Оплата',
                    store: 'B4.store.resolution.PaymentsListForPayFine',
                    editable: false,
                    flex: 1,
                    itemId: 'dfPayReg',
                    textProperty: 'PaymentId',
                    idProperty: 'Id',
                    allowBlank: true,
                    readOnly: false,
                    labelWidth: 100,
                    labelAlign: 'left',
                    //width: 400,
                    columns: [
                        { text: 'УИН', dataIndex: 'SupplierBillID', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИД платежа', dataIndex: 'PaymentId', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Сумма', dataIndex: 'Amount', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Основание', dataIndex: 'Purpose', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Идентификатор плательщика', dataIndex: 'PayerId', flex: 1, filter: { xtype: 'textfield' } },
                        //{
                        //    xtype: 'gridcolumn', text: 'Тип плательщика', dataIndex: 'PayerType', flex: 1, renderer: function (val) {
                        //        return B4.enums.PayerType.displayRenderer(val);
                        //    },
                        //    filter: {
                        //        xtype: 'combobox',
                        //        store: B4.enums.PayerType.getItemsWithEmpty([null, '-']),
                        //        operand: CondExpr.operands.eq,
                        //        editable: false
                        //    }
                        //},
                        { text: 'Наименование плательщика', dataIndex: 'PayerName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Назначение платежа', dataIndex: 'Purpose', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Дата платежа', dataIndex: 'PaymentDate', flex: 1, filter: { xtype: 'datefield' } }

                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Сохранить',
                            tooltip: 'Сохранить оплату штрафа',
                            iconCls: 'icon-accept',
                            width: 90,
                            itemId: 'btnSavePayFine'
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    itemId: 'btnClose'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});