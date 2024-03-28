Ext.define('B4.view.realityobj.housingcommunalservice.MeteringDeviceValueEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.hsemeteringdevicevalueeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 400,
    minWidth: 400,
    height: 333,
    minHeight: 333,
    bodyPadding: 5,
    title: 'Редактирование показаний общедомовых приборов учета',
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
                labelWidth: 120,
                anchor: '100%',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: false,
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
                    fieldLabel: 'Тип ПУ'
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
                    hideTrigger: false
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
                    name: 'NonLivingExpense',
                    fieldLabel: 'Расход по нежилым помещениям'
                },
                {
                    name: 'Expense',
                    fieldLabel: 'Расход'
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