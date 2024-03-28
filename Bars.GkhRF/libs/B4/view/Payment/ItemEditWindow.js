Ext.define('B4.view.payment.ItemEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.store.ManagingOrganization',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    itemId: 'paymentItemEditWindow',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                anchor: '100%',
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'column'
                    },
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ManagingOrganization',
                                    fieldLabel: 'Управляющая организация',
                                   

                                    store: 'B4.store.ManagingOrganization',
                                    allowBlank: false,
                                    editable: false,
                                    textProperty: "ContragentName",
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'IncomeBalance',
                                    fieldLabel: 'Входящее сальдо',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'OutgoingBalance',
                                    fieldLabel: 'Исходящее сальдо',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Recalculation',
                                    fieldLabel: 'Перерасчет прошлого периода',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            defaults: {
                                labelWidth: 150,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ChargeDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата начисления'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ChargePopulation',
                                    fieldLabel: 'Начислено населению',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'PaidPopulation',
                                    fieldLabel: 'Оплачено населением',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'TotalArea',
                                    fieldLabel: 'Общая площадь',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
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