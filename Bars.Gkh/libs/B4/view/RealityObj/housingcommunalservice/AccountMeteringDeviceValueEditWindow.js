Ext.define('B4.view.realityobj.housingcommunalservice.AccountMeteringDeviceValueEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.hseaccountmeteringdevicevalueeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 400,
    minWidth: 400,
    height: 320,
    minHeight: 320,
    bodyPadding: 5,
    title: 'Редактирование показаний приборов учета',
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
                    xtype: 'textfield',
                    name: 'Service',
                    fieldLabel: 'Услуга'
                },
                {
                    xtype: 'textfield',
                    name: 'MeterSerial',
                    fieldLabel: 'Номер ПУ'
                },
                {
                    xtype: 'textfield',
                    name: 'MeterType',
                    fieldLabel: 'Тип прибора учета'
                },
                {
                    xtype: 'datefield',
                    name: 'CurrentReadingDate',
                    fieldLabel: 'Дата снят. тек.',
                    format: 'd.m.Y',
                    hideTrigger: false
                },
                {
                    xtype: 'datefield',
                    name: 'PrevReadingDate',
                    fieldLabel: 'Дата снят. пред.',
                    format: 'd.m.Y',
                    hideTrigger : false
                },
                {
                    name: 'CurrentReading',
                    fieldLabel: 'Показание тек.'
                },
                {
                    name: 'PrevReading',
                    fieldLabel: 'Показание пред.'
                },
                {
                    name: 'Expense',
                    fieldLabel: 'Расход'
                },
                {
                    name: 'PlannedExpense',
                    fieldLabel: 'Плановый расход'
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