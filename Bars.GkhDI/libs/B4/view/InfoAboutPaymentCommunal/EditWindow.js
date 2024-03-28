Ext.define('B4.view.infoaboutpaymentcommunal.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 600,
    height: 480,
    bodyPadding: 5,
    itemId: 'infoAboutPaymentCommunalEditWindow',
    title: 'Сведения об оплатах коммунальных услуг',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeContractDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    height: 100,
                    region: 'north',
                    defaults: {
                        labelWidth: 280,
                        anchor: '100%',
                        labelAlign: 'left'
                    },
                    items: [
                    {
                        xtype: 'displayfield',
                        name: 'BaseServiceName',
                        fieldLabel: 'Наименование услуги',
                        maxLength: 300
                    },
                    {
                        xtype: 'displayfield',
                        name: 'ProviderName',
                        fieldLabel: 'Поставщик услуги',
                        maxLength: 300
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'CounterValuePeriodStart',
                        fieldLabel: 'Показания счетчика на начало периода'
                        
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'CounterValuePeriodEnd',
                        fieldLabel: 'Показания счетчика на конец периода'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'TotalConsumption',
                        fieldLabel: 'Общий обьем потребления'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'Accrual',
                        fieldLabel: 'Начислено потребителям (руб.)'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'Payed',
                        fieldLabel: 'Оплачено потребителями (руб.)'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'Debt',
                        fieldLabel: 'Задолженность потребителей'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'AccrualByProvider',
                        fieldLabel: 'Начислено поставщиком (поставщиками) коммунального ресурса (руб.)'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'PayedToProvider',
                        fieldLabel: 'Оплачено поставщику (поставщикам) коммунального ресурса (руб.)'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'DebtToProvider',
                        fieldLabel: 'Задолженность перед поставщиком (поставщиками) коммунального ресурса (руб.)'
                    },
                    {
                        xtype: 'gkhdecimalfield',
                        name: 'ReceivedPenaltySum',
                        fieldLabel: 'Сумма пени и штрафов, полученных от потребителей (руб.)'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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