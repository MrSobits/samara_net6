Ext.define('B4.view.chargessplitting.fuelenergyresrc.detail.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.fuelenergyresourcedetailpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',

        'B4.view.chargessplitting.fuelenergyresrc.detail.Grid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Карточка договора РСО',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    name: 'fuelEnergyResourceContractForm',
                    layout: 'vbox',
                    defaults: {
                        padding: '5',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Period',
                                    fieldLabel: 'Отчетный период'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальный район'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PublicServiceOrg',
                                    fieldLabel: 'РСО'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Charged',
                                    fieldLabel: 'Начислено за месяц'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Paid',
                                    fieldLabel: 'Оплачено за месяц'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Debt',
                                    fieldLabel: 'Задолженность на конец месяца'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PlanPayGas',
                                    fieldLabel: 'Планируемая оплата ГАЗ'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PlanPayElectricity',
                                    fieldLabel: 'Планируемая оплата Э/Э'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fuelenergyresourcedetailgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    name: 'PrintButton'
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