Ext.define('B4.view.regop.personal_account.BankStatPaymentWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.pabankstatpaymenteditwin',

    requires: [
        'B4.ux.button.Close',
        'B4.store.DistributionType',
        'B4.form.ComboBox'
    ],

    modal: true,
    layout: 'form',
    width: 420,
    bodyPadding: 5,
    title: 'Оплата по банковской выписке',
    closable: false,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DistributionType');

        store.on('beforeload', function (s, operation) {
            operation.params = operation.params || {};
            operation.params.isForPaymentsWindow = true;
        });

        var items;
        if (Gkh.config.RegOperator.GeneralConfig.IsPersonalAccountPaymentSingleField) {
            items = [
                {
                    name: 'AllInfo',
                    hidden: !Gkh.config.RegOperator.GeneralConfig.IsPersonalAccountPaymentSingleField,
                    tpl: new Ext.XTemplate([
                        '<tr><td colspan="2" style="width:100%">',
                        "Период: {Period}<br>",
                        "Дата операции: {OperationDate:date('d.m.Y')}<br>",
                        "Дата документа: {DocumentDate:date('d.m.Y')}<br>",
                        "Дата поступления/списания: {DateReceipt:date('d.m.Y')}<br>",
                        "Дата распределения: {DistributionDate:date('d.m.Y')}<br>",
                        "Сумма: {Amount}<br>",
                        "Тип операции: {DistributionCodeText}",
                        '</td></tr>'
                    ]),
                },
                {
                    name: 'DistributionCode',
                    hidden:true,
                    fieldLabel: 'Тип распределения',
                    store: store,
                    xtype: 'b4combobox'
                }
            ];
        } else {
            items = [
                {
                    name: 'Period',
                    fieldLabel: 'Период'
                },
                {
                    xtype: 'datefield',
                    name: 'OperationDate',
                    fieldLabel: 'Дата операции'
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа'
                },
                {
                    xtype: 'datefield',
                    name: 'DateReceipt',
                    fieldLabel: 'Дата поступления/списания'
                },
                {
                    xtype: 'datefield',
                    name: 'DistributionDate',
                    fieldLabel: 'Дата распределения'
                },
                {
                    name: 'Amount',
                    fieldLabel: 'Сумма'
                },
                {
                    name: 'DistributionCode',
                    fieldLabel: 'Тип распределения',
                    store: store,
                    xtype: 'b4combobox'
                }
            ];

        }

        Ext.applyIf(me, {
            defaults: {
                xtype: 'textfield',
                readOnly: true,
                labelAlign: 'right',
                labelWidth: 120,
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
                                        click: function(btn) {
                                            btn.up('pabankstatpaymenteditwin').close();
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