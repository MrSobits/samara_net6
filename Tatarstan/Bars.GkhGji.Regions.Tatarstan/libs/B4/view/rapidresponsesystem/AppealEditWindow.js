Ext.define('B4.view.rapidresponsesystem.AppealEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.view.rapidresponsesystem.AppealGeneralInfo'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    bodyPadding: 5,
    itemId: 'soprAppealEditWindow',
    title: 'Обращение',
    closeAction: 'hide',
    closable: true,
    trackResetOnLoad: true,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                readOnly: true,
                                defaults: {
                                    labelAlign: 'right',
                                    labelWidth: 200,
                                    readOnly: true
                                }
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Number',
                                    fieldLabel: 'Номер обращения'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Address',
                                            fieldLabel: 'Место возникновения проблемы',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'redirectToHouse',
                                            cls: 'icon-right-arrow-center',
                                            flex: 0.05,
                                            border: 0,
                                            tooltip: 'Перейти в карточку дома'
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    margin: '5 0 0 0',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ContragentName',
                                            fieldLabel: 'Контрагент',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'redirectToContragent',
                                            cls: 'icon-right-arrow-center',
                                            flex: 0.05,
                                            border: 0,
                                            tooltip: 'Перейти в карточку контрагента'
                                        },
                                    ]
                                }
                            ],
                            flex: 2
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'AppealDate',
                                    fieldLabel: 'Дата  обращения',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ReceiptDate',
                                    fieldLabel: 'Дата поступления в СОПР',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ControlPeriod',
                                    fieldLabel: 'Контрольный срок',
                                    format: 'd.m.Y'
                                }
                            ],
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    margin: '10 0 0 0',
                    height: 400,
                    enableTabScroll: true,
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            xtype: 'appealgeneralinfo'
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Взять в работу',
                                    action: 'TakeToWork',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'button',
                                    action: 'NotifyGji',
                                    iconCls: 'icon-arrow-right',
                                    text: 'Уведомить ГЖИ о проведенной работе'
                                }
                            ]
                        },
                        { xtype: 'tbfill'},
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
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