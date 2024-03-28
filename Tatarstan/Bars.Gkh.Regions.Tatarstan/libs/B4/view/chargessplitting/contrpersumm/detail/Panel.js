Ext.define('B4.view.chargessplitting.contrpersumm.detail.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.contrpersummdetailpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',

        'B4.view.chargessplitting.contrpersumm.detail.Grid'
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
                    name: 'periodSummForm',
                    layout: 'hbox',
                    defaults: {
                        padding: '5',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'container',
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
                                    name: 'Service',
                                    fieldLabel: 'Услуга'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ChargedResidents',
                                    fieldLabel: 'Начислено жителям'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальный район'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PaidResidents',
                                    fieldLabel: 'Оплачено жителями'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TransferredPubServOrg',
                                    fieldLabel: 'Перечислено РСО'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ManagingOrganization',
                                    fieldLabel: 'УО'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ChargedManOrg',
                                    fieldLabel: 'Начислено УО'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PaidManOrg',
                                    fieldLabel: 'Оплачено УО'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PublicServiceOrg',
                                    fieldLabel: 'РСО'
                                }   
                            ]
                        }
                    ]
                },
                {
                    xtype: 'contractperiodsummarydetailgrid',
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
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            name: 'uoState',
                            items: [
                                {
                                    xtype: 'displayfield',
                                    labelWidth: 70,
                                    fieldLabel: 'Статус УО'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'btnUoState',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            name: 'rsoState',
                            items: [
                                {
                                    xtype: 'displayfield',
                                    labelWidth: 70,
                                    fieldLabel: 'Статус РСО'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'btnRsoState',
                                    menu: []
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