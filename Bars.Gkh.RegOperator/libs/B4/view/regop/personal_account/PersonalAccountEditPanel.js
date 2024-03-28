Ext.define('B4.view.regop.personal_account.PersonalAccountEditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.personalaccounteditpanel',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.ChangeValue',
        'B4.view.regop.personal_account.PersonalAccountOperationGrid',
        'B4.view.regop.personal_account.PersonalAccountPaymentGrid',
        'B4.form.EnumCombo',
        'B4.enums.RoomOwnershipType',
        'B4.enums.PersAccServiceType',
        'B4.store.regop.ChargePeriod'
    ],

    closable: true,
    title: 'Общие сведения',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    trackResetOnLoad: true,
    header: false,
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '100%',
                    title: 'Абонент',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    margin: '0 0 5 0',
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        xtype: 'textfield',
                                        labelWidth: 190,
                                        readOnly: true,
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            itemId: 'txFio',
                                            name: 'OwnerName',
                                            fieldLabel: 'ФИО/Наименование абонента'
                                        },
                                        {
                                            name: 'RoomAddress',
                                            fieldLabel: 'Адрес'
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                xtype: 'textfield',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    name: 'RoomArea',
                                                    fieldLabel: 'Площадь',
                                                    flex: 0.7,
                                                    labelWidth: 190
                                                },
                                                {
                                                    labelWidth: 130,
                                                    name: 'AreaShare',
                                                    flex: 0.5,
                                                    itemId: 'txAreaShare',
                                                    fieldLabel: 'Доля собственности'
                                                },
                                                {
                                                    labelWidth: 50,
                                                    name: 'Tariff',
                                                    flex: 0.3,
                                                    fieldLabel: 'Тариф'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    margin: '0 0 5 0',
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        xtype: 'textfield',
                                        readOnly: true,
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'CashPayCenter',
                                            fieldLabel: 'Расчетно-кассовый центр'
                                        },
                                        {
                                            name: 'AccountFormVariant',
                                            fieldLabel: 'Способ формирования фонда'
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'OwnershipType',
                                                    fieldLabel: 'Тип собственности',
                                                    enumName: 'B4.enums.RoomOwnershipType',
                                                    labelWidth: 190,
                                                    readOnly: true,
                                                    flex: 0.6
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'ServiceType',
                                                    store: B4.enums.PersAccServiceType.getStore(),
                                                    displayField: 'Display',
                                                    valueField: 'Value',
                                                    fieldLabel: 'Тип услуги',
                                                    value: 10,
                                                    labelWidth: 100,
                                                    flex: 0.4
                                                }
                                            ]
                                        }
                                    ]
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
                    margin: '5 0 0 0',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%',
                        readOnly: true
                    },
                    title: 'Лицевой счет',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                xtype: 'datefield',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Номер ЛС',
                                    name: 'PersonalAccountNum',
                                    flex: 1
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    flex: 1,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Внешний номер ЛС',
                                            name: 'PersAccNumExternalSystems',
                                            labelWidth: 170,
                                            labelAlign: 'right',
                                            flex: 2,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'changevalbtn',
                                            flex: 1,
                                            itemId: 'btnPersAccNumExternalSystems',
                                            valueFieldXtype: 'textfield',
                                            className: 'BasePersonalAccount',
                                            propertyName: 'PersAccNumExternalSystems',
                                            margins: '0 0 0 5',
                                            valueFieldSelector: '[name=PersAccNumExternalSystems]',
                                            windowContainerSelector: '#' + me.getId()
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    flex: 1,
                                    items: [
                                        {
                                            name: 'OpenDate',
                                            fieldLabel: 'Дата открытия',
                                            format: 'd.m.Y',
                                            labelWidth: 170,
                                            labelAlign: 'right',
                                            xtype: 'datefield',
                                            flex: 2,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'changevalbtn',
                                            flex: 1,
                                            itemId: 'btnOpenDate',
                                            valueFieldXtype: 'datefield',
                                            className: 'BasePersonalAccount',
                                            propertyName: 'OpenDate',
                                            margins: '0 0 0 5',
                                            valueFieldSelector: '[name="OpenDate"]',
                                            windowContainerSelector: '#' + me.getId()
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    flex: 1,
                                    items: [
                                        {
                                            name: 'CloseDate',
                                            fieldLabel: 'Дата закрытия',
                                            format: 'd.m.Y',
                                            labelWidth: 170,
                                            labelAlign: 'right',
                                            xtype: 'datefield',
                                            flex: 2,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'changevalbtn',
                                            flex: 1,
                                            itemId: 'btnCloseDate',
                                            valueFieldXtype: 'datefield',
                                            className: 'BasePersonalAccount',
                                            propertyName: 'CloseDate',
                                            margins: '0 0 0 5',
                                            valueFieldSelector: '[name="CloseDate"]',
                                            windowContainerSelector: '#' + me.getId(),
                                            showWindow: function () {
                                                var me = this,
                                                    win = me.editWindow,
                                                    winCfg,
                                                    renderTo = Ext.getBody(),
                                                    valFieldCfg,
                                                    openDateField,
                                                    openDate;

                                                if (me.fireEvent('beforeshowwindow', me) === true) {
                                                    if (Ext.isString(me.windowContainerSelector)) {
                                                        renderTo = Ext.ComponentQuery.query(me.windowContainerSelector, B4.getBody().getActiveTab());
                                                        if (Ext.isArray(renderTo)) {
                                                            renderTo = renderTo[0];
                                                        }
                                                        if (!renderTo) {
                                                            throw "Не удалось найти контейнер для формы списка по селектору " + me.windowContainerSelector;
                                                        }

                                                        renderTo = Ext.isFunction(renderTo.getEl) ? renderTo.getEl() : (renderTo.dom ? renderTo : null);
                                                    }

                                                    openDateField = renderTo.down('[name=OpenDate]');
                                                    if (openDateField) {
                                                        openDate = openDateField.getValue();
                                                    }

                                                    if (!win) {
                                                        valFieldCfg = me.valueFieldConfig || {
                                                            xtype: me.valueFieldXtype,
                                                            fieldLabel: 'Новое значение',
                                                            hideTrigger: false,
                                                            minValue: openDate || 0,
                                                            maxValue: new Date(),
                                                            allowBlank: false,
                                                            decimalPrecision: me.decimalPrecision,
                                                            decimalSeparator: me.decimalSeparator,
                                                            listeners: {
                                                                change: function (field, newValue) {
                                                                    field.up().down('[name=factDate]').setValue(newValue);
                                                                }
                                                            }
                                                        };

                                                        valFieldCfg.name = 'value';

                                                        winCfg = {
                                                            modal: true,
                                                            width: 400,
                                                            height: 150,
                                                            closeAction: 'hide',
                                                            title: 'Смена значения',
                                                            renderTo: renderTo,
                                                            items: {
                                                                xtype: 'form',
                                                                border: 0,
                                                                defaults: {
                                                                    margin: '10 5 10 5'
                                                                },
                                                                items: [
                                                                    {
                                                                        xtype: 'datefield',
                                                                        name: 'factDate',
                                                                        hidden: true
                                                                    },
                                                                    valFieldCfg,
                                                                    {
                                                                        xtype: 'b4filefield',
                                                                        name: 'BaseDoc',
                                                                        fieldLabel: 'Документ-основание'
                                                                    }
                                                                ],
                                                                dockedItems: [
                                                                    {
                                                                        xtype: 'toolbar',
                                                                        dock: 'top',
                                                                        items: [
                                                                            {
                                                                                xtype: 'b4savebutton',
                                                                                listeners: {
                                                                                    click: {
                                                                                        fn: me.changeValue,
                                                                                        scope: me
                                                                                    }
                                                                                }
                                                                            },
                                                                            { xtype: 'tbfill' },
                                                                            {
                                                                                xtype: 'b4closebutton',
                                                                                listeners: {
                                                                                    click: {
                                                                                        fn: me.closeWindow,
                                                                                        scope: me
                                                                                    }
                                                                                }
                                                                            }
                                                                        ]
                                                                    }
                                                                ]
                                                            }
                                                        };

                                                        if (me.windowConfig) {
                                                            Ext.apply(winCfg, me.windowConfig);
                                                        }
                                                        win = me.editWindow = Ext.widget('window', winCfg);
                                                    }
                                                    win.show();
                                                }
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 0',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 170,
                                readOnly: true,
                                labelAlign: 'right',
                                xtype: 'textfield',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'ChargedBaseTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Начислено взносов по минимальному тарифу всего'
                                },
                                {
                                    name: 'ChargedDecisionTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Начислено взносов по тарифу решения всего'
                                },
                                {
                                    name: 'ChargedPenalty',
                                    hasDetails: true,
                                    fieldLabel: 'Начислено пени всего'
                                },
                                {
                                    name: 'TotalCharge',
                                    fieldLabel: 'Итого начислено'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 0',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 170,
                                readOnly: true,
                                labelAlign: 'right',
                                xtype: 'textfield',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'PaymentBaseTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Уплачено взносов по минимальному тарифу всего'
                                },
                                {
                                    name: 'PaymentDecisionTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Уплачено взносов по тарифу решения всего'
                                },
                                {
                                    name: 'PaymentPenalty',
                                    hasDetails: true,
                                    fieldLabel: 'Уплачено пени всего'
                                },
                                {
                                    name: 'TotalPayment',
                                    fieldLabel: 'Итого уплачено'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 0',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 170,
                                readOnly: true,
                                labelAlign: 'right',
                                xtype: 'textfield',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'DebtBaseTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Задолженность по взносам всего'
                                },
                                {
                                    name: 'DebtDecisionTariff',
                                    hasDetails: true,
                                    fieldLabel: 'Задолженность по взносам тарифа решения, всего'
                                },
                                {
                                    name: 'DebtPenalty',
                                    hasDetails: true,
                                    fieldLabel: 'Задолженность пени всего'
                                },
                                {
                                    name: 'TotalDebt',
                                    fieldLabel: 'Итого задолженность'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'hbox'
                    },
                    margin: '5 0 0 0',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        readOnly: true
                    },
                    name: 'PerfWorkChargeBalanceFieldSet',
                    title: 'Зачет средств за ранее выполненные работы',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Оставшаяся сумма зачета средств за работы',
                            name: 'PerfWorkChargeBalance',
                            width: 300
                        },
                        {
                            xtype: 'textfield',
                            labelWidth: 120,
                            hasDetails: true,
                            fieldLabel: 'Сумма зачтенных средств',
                            name: 'PerfWorkCreditedBalance',
                            width: 300
                        }

                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    flex:1,
                    activeTab: 0,
                    items: [
                        {
                            xtype: 'paoperationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'papaymentgrid'
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
                            items: [
                                {
                                    text: 'Переходы',
                                    itemId: 'btnredirect',
                                    iconCls: 'icon-arrow-out',
                                    menu: {
                                        xtype: 'menu',
                                        items: [
                                            {
                                                text: 'Перейти к помещению',
                                                itemId: 'btnredirecttoroom',
                                                action: 'redirecttoroom',
                                                iconCls: 'icon-arrow-out'
                                            },
                                            {
                                                text: 'Перейти к абоненту',
                                                itemId: 'btnredirecttoowner',
                                                action: 'redirecttoowner',
                                                iconCls: 'icon-arrow-out'
                                            }
                                        ]
                                    }
                                },
                                {
                                    itemId: 'btnReportPersonalAccount',
                                    text: 'Отчет по ЛС'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            fieldLabel: 'Выгрузить платежный документ',
                            itemId: 'sfPaymentDoc',
                            windowContainerSelector: '#' + me.getId(),
                            windowCfg: {
                                modal: true
                            },
                            labelWidth: 175,
                            columns: [
                                { header: 'Наименование', dataIndex: 'Name', flex: 2 },
                                {
                                    header: 'Закрыт',
                                    dataIndex: 'IsClosed',
                                    flex: 1,
                                    renderer: function (v) {
                                        if (v === true) {
                                            return 'Закрыт';
                                        }
                                        return 'Открыт';
                                    }
                                }
                            ],
                            store: Ext.create('B4.store.regop.ChargePeriod')
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Не считать должником',
                            fieldStyle: 'vertical-align: middle;',
                            style: 'font-size: 11px !important;',
                            margin: '-2 0 0 10',
                            readOnly: true,
                            width: 175,
                            name: 'IsNotDebtor'
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Заключен договор рассрочки',
                            fieldStyle: 'vertical-align: middle;',
                            style: 'font-size: 11px !important;',
                            margin: '-2 0 0 10',
                            readOnly: true,
                            width: 175,
                            name: 'InstallmentPlan'
                        }, '->',
                        {
                            xtype: 'buttongroup',
                            name: 'ClaimWorkButtons',
                            defaults: {
                                xtype: 'button',
                                iconCls: 'icon-script-go',
                            },
                            items: [
                                {
                                    text: 'ПИР',
                                    action: 'pir',
                                    name: 'Pir',
                                    hidden: Gkh.config.ClaimWork.Enabled === false
                                },
                                {
                                    text: 'Реструктуризация',
                                    action: 'restruct',
                                    name: 'Restructuring',
                                    hidden: Gkh.config.ClaimWork.Enabled === false
                                },
                                {
                                    text: 'Мировое соглашение',
                                    action: 'amicagr',
                                    name: 'AmicableAgreement',
                                    hidden: Gkh.config.ClaimWork.Enabled === false
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'State',
                                    text: 'Статус',
                                    disabled: true
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