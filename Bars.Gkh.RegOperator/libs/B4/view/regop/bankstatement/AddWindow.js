Ext.define('B4.view.regop.bankstatement.AddWindow', {
    extend: 'B4.form.Window',

    modal: true,

    width: 750,
    bodyPadding: 5,
    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.enums.MoneyDirection',
        'B4.enums.regop.TypeCalcAccount',
        'B4.store.contragent.ContragentForSelect',
        'B4.store.contragent.Bank'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Добавление банковской операции',
    alias: 'widget.rbankstatementaddwin',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 130,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            name: 'DocumentDate'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата поступления/списания',
                            format: 'd.m.Y',
                            labelWidth: 175,
                            name: 'DateReceipt'
                        }
                    ]
                },
                 {
                     xtype: 'container',
                     layout: 'hbox',
                     padding: '5 0 5 0',
                     defaults: {
                         labelAlign: 'right',
                         labelWidth: 130,
                         allowBlank: false,
                         flex: 1
                     },
                     items: [
                         {
                             xtype: 'textfield',
                             fieldLabel: 'Номер документа',
                             name: 'DocumentNum',
                             maxLength: 100
                         },
                         {
                             xtype: 'numberfield',
                             fieldLabel: 'Сумма',
                             name: 'Sum',
                             hideTrigger: true,
                             minValue: 0,
                             labelWidth: 175,
                             decimalSeparator: ','
                         }
                     ]
                 },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Назначение платежа',
                    name: 'PaymentDetails',
                    labelAlign: 'right',
                    labelWidth: 130,
                    maxLength: 250
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 130,
                        flex: 1,
                        editable: false
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Приход/расход',
                            name: 'MoneyDirection',
                            items: B4.enums.MoneyDirection.getItems()
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'File'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Реквизиты плательщика',
                    type: 'Payer',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Payer'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Наименование контрагента',
                                    name: 'PayerName',
                                    labelAlign: 'right',
                                    labelWidth: 160,
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    action: 'SelectContragent',
                                    text: 'Выбрать из контрагентов',
                                    iconCls: 'icon-add',
                                    width: 160,
                                    margin: '0 0 0 5',
                                    styleHtmlContent: 'font-size: 10px'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'PayerNameSelectField',
                            hidden: true,
                            labelAlign: 'right',
                            labelWidth: 160,
                            windowCfg: { modal: true },
                            store: 'B4.store.contragent.ContragentForSelect',
                            idProperty: 'Name',
                            columns: [
                                {
                                    xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальный район', flex: 1, filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                change: function (field, newValue) {
                                    if (typeof newValue != 'undefined') {
                                        field.up('fieldset[type = Payer]').down('textfield[name = PayerName]').setValue(newValue.Name);
                                        field.up('fieldset[type = Payer]').down('textfield[name = PayerInn]').setValue(newValue.Inn);
                                        field.up('fieldset[type = Payer]').down('textfield[name = PayerKpp]').setValue(newValue.Kpp);
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 60
                            },
                            items: [
                                {
                                    fieldLabel: 'Р/С',
                                    name: 'PayerAccountNum',
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    action: 'SelectPayerAccountNum',
                                    text: 'Выбрать',
                                    width: 60,
                                    margin: '0 0 0 2',
                                    styleHtmlContent: 'font-size: 10px'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Р/С',
                                    name: 'PayerAccountNumContragentSelectField',
                                    windowCfg: {
                                        modal: true
                                    },
                                    hidden: true,
                                    store: 'B4.store.contragent.Bank',
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            flex: 2,
                                            hidden: true,
                                            text: 'Кредитная организация',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Bik',
                                            width: 100,
                                            text: 'БИК',
                                            hidden: true,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Okonh',
                                            flex: 1,
                                            text: 'ОКОНХ',
                                            hidden: true,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Okpo',
                                            flex: 1,
                                            text: 'ОКПО',
                                            hidden: true,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'CorrAccount',
                                            flex: 1,
                                            text: 'Корр. счет',
                                            hidden: true,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'SettlementAccount',
                                            flex: 1,
                                            text: 'Расч. счет',
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    listeners: {
                                        beforeload: function (field, options, store) {
                                            options.params = options.params || {};

                                            var payerField = field.up('fieldset[type=Payer]').down('b4selectfield[name=PayerNameSelectField]'),
                                                payerId;

                                            if (payerField && payerField.value) {
                                                payerId = payerField.value.Id;
                                                options.params.contragentId = payerId;
                                            }

                                        },
                                        change: function (field, newValue) {
                                            if (typeof newValue != 'undefined') {
                                                field.up('fieldset[type=Payer]').down('textfield[name=PayerAccountNum]').setValue(newValue.SettlementAccount);
                                                field.up('fieldset[type=Payer]').down('textfield[name=PayerBik]').setValue(newValue.Bik);
                                                field.up('fieldset[type=Payer]').down('textfield[name=PayerCorrAccount]').setValue(newValue.CorrAccount);
                                                field.up('fieldset[type=Payer]').down('textfield[name=PayerBank]').setValue(newValue.Name);
                                            }
                                        }
                                    },
                                    idProperty: 'SettlementAccount',
                                    textProperty: 'SettlementAccount'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Р/С',
                                    name: 'PayerAccountNumSelectField',
                                    hidden: true,
                                    store: 'B4.store.CalcAccount',
                                    columns: [
                                        {
                                            dataIndex: 'TypeAccount',
                                            flex: 1,
                                            text: 'Тип счета',
                                            filter: {
                                                xtype: 'b4enumcombo',
                                                includeNull: true,
                                                enumName: 'B4.enums.regop.TypeCalcAccount'
                                            },
                                            renderer: function (val) {
                                                return B4.enums.regop.TypeCalcAccount.displayRenderer(val);
                                            }
                                        },
                                        {
                                            dataIndex: 'AccountNumber',
                                            flex: 1,
                                            text: 'Расч. счет',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Address',
                                            flex: 1,
                                            text: 'Адрес',
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    listeners: {
                                        change: function (field, newValue) {
                                            if (typeof newValue != 'undefined' && newValue) {
                                                field.up('fieldset[type=Payer]').down('textfield[name=PayerAccountNum]').setValue(newValue.AccountNumber);
                                            }
                                        }
                                    },
                                    idProperty: 'SettlementAccount',
                                    textProperty: 'SettlementAccount'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'ИНН',
                                    labelWidth: 40,
                                    name: 'PayerInn',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 60,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'БИК',
                                    name: 'PayerBik'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'КПП',
                                    name: 'PayerKpp'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Корр.счет',
                                    name: 'PayerCorrAccount'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Банк',
                            name: 'PayerBank',
                            labelAlign: 'right',
                            labelWidth: 60
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Реквизиты получателя',
                    type: 'Recipient',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Recipient'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Наименование контрагента',
                                    name: 'RecipientName',
                                    labelAlign: 'right',
                                    labelWidth: 160,
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    action: 'SelectRecipientContragent',
                                    text: 'Выбрать из контрагентов',
                                    iconCls: 'icon-add',
                                    width: 160,
                                    margin: '0 0 0 5',
                                    styleHtmlContent: 'font-size: 10px'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'RecipientNameSelectField',
                            hidden: true,
                            labelAlign: 'right',
                            labelWidth: 160,
                            store: 'B4.store.contragent.ContragentForSelect',
                            idProperty: 'Name',
                            columns: [
                                {
                                    xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальный район', flex: 1, filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                change: function (field, newValue) {
                                    if (typeof newValue != 'undefined') {
                                        field.up('fieldset[type=Recipient]').down('textfield[name=RecipientName]').setValue(newValue.Name);
                                        field.up('fieldset[type=Recipient]').down('textfield[name=RecipientInn]').setValue(newValue.Inn);
                                        field.up('fieldset[type=Recipient]').down('textfield[name=RecipientKpp]').setValue(newValue.Kpp);
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 60
                            },
                            items: [
                                {
                                    fieldLabel: 'Р/С',
                                    name: 'RecipientAccountNum',
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    action: 'SelectRecipientAccountNum',
                                    text: 'Выбрать',
                                    width: 60,
                                    margin: '0 0 0 2',
                                    styleHtmlContent: 'font-size: 10px'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Р/С',
                                    name: 'RecipientAccountContragentSelectField',
                                    hidden: true,
                                    windowCfg: {
                                        modal: true
                                    },
                                    store: 'B4.store.contragent.Bank',
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            flex: 2,
                                            hidden: true,
                                            text: 'Кредитная организация',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Bik',
                                            width: 100,
                                            hidden: true,
                                            text: 'БИК',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Okonh',
                                            flex: 1,
                                            hidden: true,
                                            text: 'ОКОНХ',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Okpo',
                                            flex: 1,
                                            hidden: true,
                                            text: 'ОКПО',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'CorrAccount',
                                            flex: 1,
                                            hidden: true,
                                            text: 'Корр. счет',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'SettlementAccount',
                                            flex: 1,
                                            text: 'Расч. счет',
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    listeners: {
                                        beforeload: function (field, options, store) {
                                            options.params = options.params || {};

                                            var payerField = field.up('fieldset[type=Recipient]').down('b4selectfield[name=RecipientNameSelectField]'),
                                                payerId;

                                            if (payerField && payerField.value) {
                                                payerId = payerField.value.Id;
                                                options.params.contragentId = payerId;
                                                options.params.fromBankStatement = true;
                                            }

                                        },
                                        change: function (field, newValue) {
                                            if (typeof newValue != 'undefined') {
                                                field.up('fieldset[type=Recipient]').down('textfield[name=RecipientAccountNum]').setValue(newValue.SettlementAccount);
                                                field.up('fieldset[type=Recipient]').down('textfield[name=RecipientBik]').setValue(newValue.Bik);
                                                field.up('fieldset[type=Recipient]').down('textfield[name=RecipientCorr]').setValue(newValue.CorrAccount);
                                                field.up('fieldset[type=Recipient]').down('textfield[name=RecipientBank]').setValue(newValue.Name);
                                            }
                                        }
                                    },
                                    idProperty: 'SettlementAccount',
                                    textProperty: 'SettlementAccount'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Р/С',
                                    name: 'RecipientAccountSelectField',
                                    hidden: true,
                                    store: 'B4.store.CalcAccount',
                                    columns: [
                                        {
                                            dataIndex: 'TypeAccount',
                                            flex: 1,
                                            text: 'Тип счета',
                                            filter: {
                                                xtype: 'b4enumcombo',
                                                includeNull: true,
                                                enumName: 'B4.enums.regop.TypeCalcAccount'
                                            },
                                            renderer: function (val) {
                                                return B4.enums.regop.TypeCalcAccount.displayRenderer(val);
                                            }
                                        },
                                        {
                                            dataIndex: 'AccountNumber',
                                            flex: 1,
                                            text: 'Расч. счет',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Address',
                                            flex: 1,
                                            text: 'Адрес',
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    listeners: {
                                        change: function (field, newValue) {
                                            if (typeof newValue != 'undefined' && newValue) {
                                                field.up('fieldset[type=Recipient]').down('textfield[name=RecipientAccountNum]').setValue(newValue.AccountNumber);
                                            }
                                        }
                                    },
                                    idProperty: 'SettlementAccount',
                                    textProperty: 'SettlementAccount'
                                },
                                {
                                    fieldLabel: 'ИНН',
                                    name: 'RecipientInn',
                                    flex: 1,
                                    labelWidth: 40
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 60,
                                flex: 1
                            },
                            items: [
                                {
                                    fieldLabel: 'БИК',
                                    name: 'RecipientBik'
                                },
                                {
                                    fieldLabel: 'КПП',
                                    name: 'RecipientKpp'
                                },
                                {
                                    fieldLabel: 'Корр.счет',
                                    name: 'RecipientCorr'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Банк',
                            name: 'RecipientBank',
                            labelAlign: 'right',
                            labelWidth: 60
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
                            items: [{ xtype: 'b4savebutton' }]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{
                                xtype: 'b4closebutton',
                                listeners: {
                                    'click': function () {
                                        me.close();
                                    }
                                }
                            }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});