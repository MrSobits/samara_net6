Ext.define('B4.view.regop.personal_account.BankImportPaymentWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.pabankimportpaymenteditwin',

    requires: [
        'B4.ux.button.Close'
    ],

    modal: true,
    layout: 'form',
    width: 420,
    bodyPadding: 5,
    title: 'Оплата по реестру оплат платежных агентов',
    closable: false,
    
    initComponent: function() {
        var me = this;

        var items;
        if (Gkh.config.RegOperator.GeneralConfig.IsPersonalAccountPaymentSingleField) {
            items=[{
                name: 'AllInfo',
                tpl: new Ext.XTemplate([
                    '<tr><td colspan="2" style="width:100%">',
                    "Период: {Period}<br>",
                    "Дата операции: {ImportDate:date('d.m.Y')}<br>",
                    "Дата сводного реестра: {DocumentDate:date('d.m.Y')}<br>",
                    "Номер сводного реестра: {DocumentNum}<br>",
                    "Код платежного агента: {PaymentAgentCode}<br>",
                    "Наименование платежного агента: {PaymentAgentName}<br>",
                    "Тип оплаты: {PaymentType}<br>",
                    "Сумма: {Amount}<br>",
                    "Дата оплаты: {PaymentDate:date('d.m.Y')}<br>",
                    "Дата подтверждения: {AcceptDate:date('d.m.Y')}<br>",
                    "Номер платежа в УС агента: {PaymentNumberUs}",
                    '</td></tr>'
                ]),
            }];
        } else {
            items = [
                {
                    name: 'Period',
                    fieldLabel: 'Период'
                },
                {
                    xtype: 'datefield',
                    name: 'ImportDate',
                    fieldLabel: 'Дата операции'
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата сводного реестра'
                },
                {
                    name: 'DocumentNum',
                    fieldLabel: 'Номер сводного реестра'
                },
                {
                    name: 'PaymentAgentCode',
                    fieldLabel: 'Код платежного агента'
                },
                {
                    name: 'PaymentAgentName',
                    fieldLabel: 'Наименование платежного агента'
                },
                {
                    name: 'PaymentType',
                    fieldLabel: 'Тип оплаты'
                },
                {
                    name: 'Amount',
                    fieldLabel: 'Сумма'
                },
                {
                    xtype: 'datefield',
                    name: 'PaymentDate',
                    fieldLabel: 'Дата оплаты'
                },
                {
                    xtype: 'datefield',
                    name: 'AcceptDate',
                    fieldLabel: 'Дата подтверждения'
                },
                {
                    name: 'PaymentNumberUs',
                    fieldLabel: 'Номер платежа в Системе ПА'
                }
            ];
        }

        Ext.applyIf(me, {
            defaults: {
                xtype: 'textfield',
                readOnly: true,
                labelAlign: 'right',
                labelWidth: 120
            },
            items: items,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('pabankimportpaymenteditwin').close();
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
    }
});