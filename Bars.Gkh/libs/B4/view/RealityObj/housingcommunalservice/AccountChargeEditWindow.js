Ext.define('B4.view.realityobj.housingcommunalservice.AccountChargeEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.hseaccountchargeeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 400,
    minWidth: 400,
    height: 482,
    minHeight: 482,
    bodyPadding: 5,
    title: 'Редактирование начисления лицевого счета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'numberfield',
                labelAlign: 'right',
                labelWidth: 130,
                anchor: '100%',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                decimalSeparator: ',',
                minValue: 0
            },
            items: [
                {
                    name: 'Supplier',
                    xtype: 'textfield',
                    fieldLabel: 'Поставщик'
                },
                {
                    xtype: 'textfield',
                    name: 'Service',
                    fieldLabel: 'Услуга'
                },
                {
                    name: 'Tariff',
                    fieldLabel: 'Тариф'
                },
                {
                    xtype: 'datefield',
                    name: 'DateCharging',
                    fieldLabel: 'Дата начисления',
                    format: 'd.m.Y',
                    hideTrigger: false
                },
                {
                    name: 'Expense',
                    fieldLabel: 'Расход'
                },
                {
                    name: 'CompleteCalc',
                    fieldLabel: 'Полный расчет'
                },
                {
                    name: 'Underdelivery',
                    fieldLabel: 'Недопоставка'
                },
                {
                    name: 'Charged',
                    fieldLabel: 'Начислено'
                },
                {
                    name: 'Recalc',
                    fieldLabel: 'Перерасчет'
                },
                {
                    name: 'InnerBalance',
                    fieldLabel: 'Вх.сальдо'
                },
                {
                    name: 'Changed',
                    fieldLabel: 'Изменен'
                },
                {
                    name: 'Payment',
                    fieldLabel: 'Оплата'
                },
                {
                    name: 'ChargedPayment',
                    fieldLabel: 'Начислено к оплате'
                },
                {
                    name: 'OuterBalance',
                    fieldLabel: 'Исх.сальдо'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});