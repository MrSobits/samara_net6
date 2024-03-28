Ext.define('B4.view.manorg.contract.TransferEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.enums.ContractStopReasonEnum',
        'B4.enums.ManOrgTransferContractFoundation',
        'B4.enums.ManOrgSetPaymentsOwnersFoundation'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 870,
    maxWidth: 870,
    width: 870,
    minHeight: 350,
    height: 830,
    bodyPadding: '5 5 0 5',
    itemId: 'manorgTransferEditWindow',
    title: 'Форма редактирования договора управления (передача управления)',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'tabpanel',
                    padding: 0,
                    border: false,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        autoScroll: true
                    },
                    frame: true,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Сведения о договоре',
                            padding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Реквизиты',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ManOrgJskTsj',
                                            itemId: 'tfJskTsj',
                                            fieldLabel: 'Управление передано от',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'ContractFoundation',
                                            fieldLabel: 'Основание',
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            store: B4.enums.ManOrgTransferContractFoundation.getStore()
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RealityObjectName',
                                            fieldLabel: 'Жилой дом',
                                            itemId: 'tfRealityObject',
                                            readOnly: true,
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Протокол общего собрания',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ProtocolNumber',
                                                    fieldLabel: 'Номер протокола',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ProtocolDate',
                                                    fieldLabel: 'от',
                                                    labelWidth: 50,
                                                    maxWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'ProtocolFileInfo',
                                            fieldLabel: 'Протокол общего собрания'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Договор управления',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                allowBlank: false,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumber',
                                                    fieldLabel: 'Номер документа',
                                                    labelWidth: 150,
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentDate',
                                                    fieldLabel: 'от',
                                                    labelWidth: 50,
                                                    format: 'd.m.Y',
                                                    maxWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                labelAlign: 'right',
                                                labelWidth: 150,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    name: 'StartDate',
                                                    fieldLabel: 'Дата начала управления',
                                                    allowBlank: false
                                                },
                                                {
                                                    name: 'PlannedEndDate',
                                                    fieldLabel: 'Плановая дата окончания',
                                                    labelWidth: 160,
                                                },
                                                {
                                                    name: 'EndDate',
                                                    fieldLabel: 'Дата окончания управления',
                                                    labelWidth: 170
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileInfo',
                                            fieldLabel: 'Договор управления'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Note',
                                            fieldLabel: 'Примечание',
                                            height: 40,
                                            maxLength: 300,
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: { type: 'hbox', align: 'stretch' },
                                    defaults: {
                                        xtype: 'fieldset',
                                        layout: { type: 'vbox', align: 'stretch' },
                                        flex: 1,
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 200
                                        }
                                    },
                                    items: [
                                        {
                                            title: 'Периоды ввода показаний приборов учета',
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'InputMeteringDeviceValuesBeginDate',
                                                    fieldLabel: 'День месяца начала ввода показаний по ПУ (от 1 до 30)',
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'radiogroup',
                                                    itemId: 'thisMonthInputMeteringDeviceValuesBeginDateRb',
                                                    name: 'ThisMonthInputMeteringDeviceValuesBeginDate',
                                                    width: 500,
                                                    padding: '0 0 5 0',
                                                    labelWidth: 180,
                                                    columns: 2,
                                                    items: [
                                                        {
                                                            name: 'ThisMonthInputMeteringDeviceValuesBeginDate',
                                                            boxLabel: 'Текущего месяца',
                                                            inputValue: true,
                                                            itemId: 'ThisMonth'
                                                        },
                                                        {
                                                            name: 'ThisMonthInputMeteringDeviceValuesBeginDate',
                                                            boxLabel: 'Следующего месяца',
                                                            itemId: 'NextMonth',
                                                            inputValue: false
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'InputMeteringDeviceValuesEndDate',
                                                    fieldLabel: 'День месяца окончания ввода показаний по ПУ (от 2 до 31)',
                                                    maxValue: 31,
                                                    minValue: 2,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'radiogroup',
                                                    itemId: 'thisMonthInputMeteringDeviceValuesEndDateRb',
                                                    name: 'ThisMonthInputMeteringDeviceValuesEndDate',
                                                    width: 500,
                                                    padding: '0 0 5 0',
                                                    labelWidth: 180,
                                                    columns: 2,
                                                    items: [
                                                        {
                                                            name: 'ThisMonthInputMeteringDeviceValuesEndDate',
                                                            boxLabel: 'Текущего месяца',
                                                            inputValue: true,
                                                            itemId: 'ThisMonth'
                                                        },
                                                        {
                                                            name: 'ThisMonthInputMeteringDeviceValuesEndDate',
                                                            boxLabel: 'Следующего месяца',
                                                            itemId: 'NextMonth',
                                                            inputValue: false
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            title: 'Периоды выставления платежных документов',
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'DrawingPaymentDocumentDate',
                                                    fieldLabel: 'День месяца (от 1 до 30)',
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'radiogroup',
                                                    itemId: 'docDateMonth',
                                                    name: 'ThisMonthPaymentDocDate',
                                                    width: 500,
                                                    padding: '0 0 5 0',
                                                    labelWidth: 180,
                                                    columns: 2,
                                                    items: [
                                                        {
                                                            name: 'ThisMonthPaymentDocDate',
                                                            boxLabel: 'Текущего месяца',
                                                            inputValue: true,
                                                            itemId: 'ThisMonth'
                                                        },
                                                        {
                                                            name: 'ThisMonthPaymentDocDate',
                                                            boxLabel: 'Следующего месяца',
                                                            itemId: 'NextMonth',
                                                            inputValue: false
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            title: 'Периоды внесения платы за ЖКУ',
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'PaymentServicePeriodDate',
                                                    fieldLabel: 'День месяца (от 1 до 30)',
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'radiogroup',
                                                    itemId: 'thisMonthPaymentServiceDateRb',
                                                    name: 'ThisMonthPaymentServiceDate',
                                                    width: 500,
                                                    padding: '0 0 5 0',
                                                    labelWidth: 180,
                                                    columns: 2,
                                                    items: [
                                                        {
                                                            name: 'ThisMonthPaymentServiceDate',
                                                            boxLabel: 'Текущего месяца',
                                                            inputValue: true,
                                                            itemId: 'ThisMonth'
                                                        },
                                                        {
                                                            name: 'ThisMonthPaymentServiceDate',
                                                            boxLabel: 'Следующего месяца',
                                                            itemId: 'NextMonth',
                                                            inputValue: false
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Расторжение договора',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'ContractStopReason',
                                            fieldLabel: 'Основание завершения обслуживания',
                                            allowBlank: false,
                                            displayField: 'Display',
                                            store: B4.enums.ContractStopReasonEnum.getStore(),
                                            valueField: 'Value'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TerminateReason',
                                            fieldLabel: 'Основание расторжения',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            padding: '5 0 0 0',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'TerminationDate',
                                                    fieldLabel: 'Дата расторжения'
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'TerminationFile',
                                                    fieldLabel: 'Файл'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Сведения о плате',
                            name: 'PaymentData',
                            padding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Сведения о размере платы',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                allowBlank: false,
                                                labelWidth: 150,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'StartDatePaymentPeriod',
                                                    fieldLabel: 'Дата начала периода',
                                                    padding: '0 5 5 0',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'EndDatePaymentPeriod',
                                                    fieldLabel: 'Дата окончания периода',
                                                    padding: '0 0 5 0',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            name: 'PaymentAmount',
                                            labelWidth: 382,
                                            fieldLabel: 'Размер платы (цена) за услуги, работы по управлению домом',
                                            decimalSeparator: ',',
                                            minValue: 0,
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Протокол общего собрания собственников',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            editable: false,
                                            labelWidth: 250,
                                            name: 'SetPaymentsFoundation',
                                            fieldLabel: 'Основание установления размера платы за содержание жилого помещения',
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            items: B4.enums.ManOrgSetPaymentsOwnersFoundation.getItemsWithEmpty([null, '-'])
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'PaymentProtocolFile',
                                            fieldLabel: 'Протокол'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'PaymentProtocolDescription',
                                            fieldLabel: 'Описание',
                                            height: 40
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'RevocationReason',
                                    fieldLabel: 'Причина аннулирования',
                                    margin: '5px',
                                    labelWidth: 162,
                                    height: 40
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
                            columns: 1,
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});