Ext.define('B4.view.realityobj.govdecisionprotocol.Window', {
    extend: 'B4.form.Window',
    alias: 'widget.govdecisionprotocolwin',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    width: 750,
    bodyPadding: 5,
    title: 'Решение органов государственной власти',
    closeAction: 'hide',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                labelAlign: 'right',
                margin: '4 0'
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'RealityObject',
                    margin: '0 0'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер',
                            name: 'ProtocolNumber',
                            flex: 1,
                            labelWidth: 65
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Уполномоченное лицо',
                            name: 'AuthorizedPerson',
                            flex: 1,
                            labelWidth: 140
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата',
                            name: 'ProtocolDate',
                            flex: 1,
                            labelWidth: 65
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Управление домом',
                            name: 'RealtyManagement',
                            flex: 1,
                            labelWidth: 140
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Телефон уполномоченного лица',
                    name: 'AuthorizedPersonPhone',
                    labelWidth: 235
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Протокол',
                    name: 'ProtocolFile',
                    labelWidth: 65
                },
                {
                    xtype: 'checkbox',
                    margin: '10 0 15 5',
                    boxLabel: 'Способ формирования фонда на счету регионального оператора',
                    name: 'FundFormationByRegop',
                    disabled: true
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    margin: '8 0 4 5',
                    items: [
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Снос МКД',
                            width: 135,
                            chbcontrol: 'Destroy',
                            name: 'Destroy'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата',
                            chbgroup: 'Destroy',
                            name: 'DestroyDate',
                            disabled: true,
                            flex: 1,
                            labelWidth: 95
                        },
                        {
                            xtype: 'tbfill'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    margin: '4 0 10 5',
                    items: [
                        {
                            xtype: 'checkbox',
                            chbcontrol: 'Reconstruction',
                            width: 135,
                            boxLabel: 'Реконструкция МКД',
                            name: 'Reconstruction'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата с',
                            chbgroup: 'Reconstruction',
                            disabled: true,
                            name: 'ReconstructionStart',
                            flex: 1,
                            labelWidth: 95
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата по',
                            disabled: true,
                            chbgroup: 'Reconstruction',
                            name: 'ReconstructionEnd',
                            flex: 1,
                            labelWidth: 70
                        }
                    ]
                },
                {
                    xtype: 'checkbox',
                    chbcontrol: 'TakeLandForGov',
                    margin: '12 0 5 5',
                    boxLabel: 'Изъятие для государственных или муниципальных нужд зумельного участка, на котором расположен МКД',
                    name: 'TakeLandForGov'
                },
                {
                    xtype: 'datefield',
                    chbgroup: 'TakeLandForGov',
                    fieldLabel: 'Дата',
                    disabled: true,
                    width: '200px !important',
                    name: 'TakeLandForGovDate',
                    labelWidth: 70
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    margin: '15 0 10 5',
                    items: [
                        {
                            xtype: 'checkbox',
                            chbcontrol: 'TakeApartsForGov',
                            boxLabel: 'Изъятие каждого жилого помещения в доме',
                            name: 'TakeApartsForGov'
                        },
                        {
                            xtype: 'datefield',
                            chbgroup: 'TakeApartsForGov',
                            fieldLabel: 'Дата',
                            name: 'TakeApartsForGovDate',
                            disabled: true,
                            width: 300,
                            labelWidth: 70
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Минимальнальный размер взноса на КР',
                    name: 'MinFund',
                    labelWidth: 235
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Максимальный размер фонда',
                    readonly: true,
                    name: 'MaxFund',
                    labelWidth: 235
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
                                {
                                    xtype: 'b4savebutton'
                                }, {
                                    xtype: 'button',
                                    text: 'Сформировать уведомление'
                                }
                            ]
                        }, '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    type: 'stateBtn',
                                    itemId: 'stateDecBtn',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
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