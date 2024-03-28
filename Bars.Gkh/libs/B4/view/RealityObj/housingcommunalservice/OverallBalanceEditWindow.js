Ext.define('B4.view.realityobj.housingcommunalservice.OverallBalanceEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.hseoverallbalanceeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 400,
    minWidth: 400,
    minHeight: 352,
    height: 352,
    bodyPadding: 5,
    title: 'Редактирование',
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
                labelWidth: 160,
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                decimalSeparator: ',',
                minValue: 0
            },
            items: [
                {
                    xtype: 'datefield',
                    hideTrigger: false,
                    name: 'DateCharging',
                    fieldLabel: 'Дата начисления'
                },
                {
                    xtype: 'textfield',
                    name: 'Service',
                    fieldLabel: 'Услуга'
                },
                {
                    name: 'InnerBalance',
                    fieldLabel: 'Вх. Сальдо',
                    minValue: Number.NEGATIVE_INFINITY
                },
                {
                    name: 'MonthCharge',
                    fieldLabel: 'Начислено за месяц'
                },
                {
                    name: 'Payment',
                    fieldLabel: 'К оплате'
                },
                {
                    name: 'Paid',
                    fieldLabel: 'Оплачено'
                },
                {
                    name: 'OuterBalance',
                    fieldLabel: 'Исх. Сальдо',
                    minValue: Number.NEGATIVE_INFINITY
                },
                {
                    name: 'CorrectionCoef',
                    fieldLabel: 'Коэффициент коррекции',
                    decimalPrecision: 7
                },
                {
                    name: 'HouseExpense',
                    fieldLabel: 'Расход по дому'
                },
                {
                    name: 'AccountsExpense',
                    fieldLabel: 'Расход по лицевым счетам'
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