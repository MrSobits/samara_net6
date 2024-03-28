Ext.define('B4.view.realityobj.housingcommunalservice.AccountEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.hseaccounteditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 950,
    minWidth: 400,
    height: 500,
    minHeight: 200,
    maximizable: true,
    title: 'Форма редактирования лицевого счета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'panel',
                            title: 'Реквизиты лицевого счета',
                            bodyStyle: Gkh.bodyStyle,
                            padding: 5,
                            layout: 'column',
                            defaults: {
                                padding: 5,
                                columnWidth: 0.5
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'numberfield',
                                        labelAlign: 'right',
                                        labelWidth: 150,
                                        anchor: '100%',
                                        hideTrigger: true,
                                        keyNavEnabled: false,
                                        mouseWheelEnabled: false,
                                        allowDecimals: false,
                                        minValue: 0
                                    },
                                    items: [
                                        {
                                            name: 'Apartment',
                                            labelWidth: 200,
                                            fieldLabel: 'Номер квартиры'
                                        },
                                        {
                                            name: 'PaymentCode',
                                            labelWidth: 200,
                                            fieldLabel: 'Платежный код'
                                        },
                                        {
                                            name: 'ResidentsCount',
                                            labelWidth: 200,
                                            fieldLabel: 'Количество проживающих/прописанных'
                                        },
                                        {
                                            name: 'TemporaryGoneCount',
                                            labelWidth: 200,
                                            fieldLabel: 'Количество временно убывших'
                                        },
                                        {
                                            name: 'ApartmentArea',
                                            labelWidth: 200,
                                            fieldLabel: 'Общая площадь квартиры',
                                            allowDecimals: true,
                                            decimalSeparator: ','
                                        },
                                        {
                                            name: 'LivingArea',
                                            labelWidth: 200,
                                            fieldLabel: 'Жилая площадь',
                                            allowDecimals: true,
                                            decimalSeparator: ','
                                        },
                                        {
                                            name: 'RoomsCount',
                                            labelWidth: 200,
                                            fieldLabel: 'Количество комнат'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'AccountState',
                                            fieldLabel: 'Состояние счета',
                                            readOnly: true
                                        },
                                        //{
                                        //    xtype: 'textfield',
                                        //    name: 'HouseStatus',
                                        //    fieldLabel: 'Статус жилья',
                                        //    readOnly: true
                                        //},
                                        {
                                            xtype: 'checkbox',
                                            name: 'Living',
                                            fieldLabel: 'Нежилое помещение'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'Privatizied',
                                            fieldLabel: 'Приватизированная'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            title: 'Начисления лицевого счета',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                //{
                                //    xtype: 'container',
                                //    items: [{
                                //        xtype: 'datefield',
                                //        fieldLabel: 'Месяц',
                                //        name: 'month',
                                //        format: 'd.m.Y',
                                //        itemId: 'AccountChargeMonth',
                                //        labelAlign: 'right',
                                //        labelWidth: 50
                                //    }]
                                //},
                                {
                                    xtype: 'hseaccountchargegrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            title: 'Показания приборов учета',
                            itemId: 'accountMeteringDeviceValuePanel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'hseaccountmeteringdevicevaluegrid',
                                    flex: 1
                                }
                            ]
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