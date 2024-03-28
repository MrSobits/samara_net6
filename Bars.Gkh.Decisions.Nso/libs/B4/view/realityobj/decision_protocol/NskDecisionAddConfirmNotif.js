Ext.define('B4.view.realityobj.decision_protocol.NskDecisionAddConfirmNotif', {
    extend: 'B4.form.Window',
    alias: 'widget.nskdecisionaddconfirmnotif',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.Control.GkhDecimalField'
    ],

    modal: true,
    title: 'Уведомление о выборе способа формирования фонда капитального ремонта',
    border: false,
    width: 800,
    height: 600,
    closeAction: 'destroy',
    autoScroll: true,
    bodyPadding: 5,
    defaults: {
        labelWidth: 200,
        labelAlign: 'right'
    },
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'hidden',
                    name: 'Protocol'
                },
                {
                    xtype: 'hidden',
                    name: 'ProtocolId'
                },
                {
                    xtype: 'hidden',
                    name: 'RealObjId'
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Уведомление',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Исх. номер',
                                    name: 'Number'
                                }, {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата',
                                    name: 'Date'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Документ уведомления',
                            name: 'Document'
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Протокол',
                            name: 'ProtocolFile'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Муниципальный район',
                    name: 'Mu',
                    readOnly: true

                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Муниципальное образование',
                    name: 'MoSettlement',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margin: '0 0 5 0',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес',
                            name: 'Address',
                            readOnly: true,
                            flex: 1,
                            labelWidth: 200,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            fieldLabel: 'Площадь',
                            width: 200,
                            name: 'AreaLivingNotLivingMkd',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Способ управления МКД',
                    name: 'Manage',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Способ формирования фонда',
                    name: 'FormFundType',
                    readOnly: true
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Информация о владельце счета',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование организации',
                            name: 'OrgName',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Почтовый адрес',
                            name: 'PostAddress',
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 190
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ИНН',
                                    name: 'Inn',
                                    readOnly: true
                                }, {
                                    xtype: 'textfield',
                                    fieldLabel: 'КПП',
                                    name: 'Kpp',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 190
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОГРН',
                                    name: 'Ogrn',
                                    readOnly: true
                                }, {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОКТМО',
                                    name: 'Oktmo',
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Кредитная организация',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование',
                            name: 'CreditOrgName',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Почтовый адрес',
                            name: 'CreditOrgAddress',
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'БИК',
                                    name: 'CreditOrgBik',
                                    readOnly: true
                                }, {
                                    xtype: 'textfield',
                                    fieldLabel: 'Корсчет',
                                    name: 'CreditOrgCorAcc',
                                    readOnly: true
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
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ИНН',
                                    name: 'CreditOrgInn',
                                    readOnly: true
                                }, {
                                    xtype: 'textfield',
                                    fieldLabel: 'КПП',
                                    name: 'CreditOrgKpp',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОГРН',
                                    name: 'CreditOrgOgrn',
                                    readOnly: true
                                }, {
                                    xtype: 'textfield',
                                    fieldLabel: 'ОКТМО',
                                    name: 'CreditOrgOktmo',
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Специальный счёт',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер счета',
                            name: 'AccountNum'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата открытия',
                                    name: 'OpenDate'
                                }, {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата закрытия',
                                    name: 'CloseDate'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Справка банка',
                            name: 'BankDoc'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Отметка о принятии ГЖИ',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                flex: 1,
                                labelWidth: 190,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Входящий №',
                                    name: 'IncomeNum'
                                }, {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата регистрации',
                                    name: 'RegistrationDate'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '0 0 5 0',
                            defaults: {
                                labelWidth: 190,
                                flex: 1,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkboxfield',
                                    boxLabel: 'Оригинал уведомления поступил',
                                    itemId: 'cbOriginalIncome',
                                    name: 'OriginalIncome'
                                }, {
                                    xtype: 'checkboxfield',
                                    boxLabel: 'Копия справки поступила',
                                    itemId: 'cbCopyIncome',
                                    name: 'CopyIncome'
                                }, {
                                    xtype: 'checkboxfield',
                                    boxLabel: 'Копия протокола поступила',
                                    itemId: 'cbCopyProtocolIncome',
                                    name: 'CopyProtocolIncome'
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    cmd: 'ToProtocol',
                                    text: 'Протокол'
                                },
                                {
                                    xtype: 'button',
                                    action: 'DownloadNotification',
                                    iconCls: 'icon-disk',
                                    text: 'Скачать уведомление'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'StateButton',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(btn) {
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