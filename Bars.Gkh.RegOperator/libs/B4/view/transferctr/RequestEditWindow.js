Ext.define('B4.view.transferctr.RequestEditWindow', {
    extend: 'B4.form.Window',
    alias:'widget.requesttransferctreditwin',
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ProgramCr',
        'B4.store.contragent.Bank',
        'B4.store.objectcr.PersonalAccount',
        'B4.store.ObjectCr',
        'B4.store.objectcr.ObjectCrBuilder',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramRequest',
        'B4.enums.TypePaymentRfCtr',       
        'B4.enums.KindPayment',
        'B4.store.objectcr.BuildContract',
        'B4.store.dict.FinanceSource',
        'B4.store.calcaccount.Regop',
        'B4.store.dict.ProgramCrType',
        'B4.store.objectcr.TypeWorkCr',
        'B4.view.transferctr.PaymentDetailGrid',
        'B4.view.transferctr.RequestPaymentInfoPanel',
        'B4.view.transferctr.RequestInfoPanel'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    width: 700,
    height: 600,
    itemId: 'requestTransferCtrEditWindow',
    title: 'Заявка на перечисление средств подрядчику',
    layout: 'fit',
    border: false,
    closeAction: 'destroy',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    tabbar: {
                        border: false
                    },
                                flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                            defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        bodyPadding: 5
                            },
                            items: [
                                {
                            xtype: 'requestinfopanel',
                            title: 'Реквизиты'
                            },
                                {
                            xtype: 'requestpaymentinfopanel',
                            title: 'Информация о платеже'
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
                            xtype: 'hidden',
                            name: 'Id'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    action: 'ExportToTxt',
                                    iconCls: 'icon-table-go',
                                    text: 'Сформировать документ'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});