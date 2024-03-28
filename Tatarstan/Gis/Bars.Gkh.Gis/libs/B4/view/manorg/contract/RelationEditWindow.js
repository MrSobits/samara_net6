Ext.define('B4.view.manorg.contract.RelationEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.relationrditwindow',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.enums.ContractStopReasonEnum'
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
    height: 645,
    bodyPadding: 5,
    itemId: 'manOrgContractRelationEditWindow',
    title: 'Форма редактирования договора управления (передача управления)',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 160
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
                                        editable: false,
                                        name: 'ManagingOrganization',
                                        itemId: 'sfManorg',
                                        fieldLabel: 'Управляющая компания',
                                        textProperty: 'ContragentName',

                                        store: 'B4.store.ManagingOrganization',
                                        columns: [
                                            { text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } }
                                        ],
                                        allowBlank: false
                                    },
                                    {
                                        xtype: 'combobox',
                                        editable: false,
                                        name: 'ContractFoundation',
                                        fieldLabel: 'Основание',
                                        displayField: 'Display',
                                        valueField: 'Value',
                                        store: B4.enums.ManOrgTransferContractFoundation.getStore()
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
                                            border: false,
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
                                            border: false,
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
                                                    labelWidth: 170
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
                                            //height: 40,
                                            maxLength: 300,
                                            flex: 1
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
                            title: 'Сведения об услугах',
                            padding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults:
                            {
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'relationadditionservicegrid',
                                    border: true,
                                    padding: '0 0 5 0'
                                },
                                {
                                    xtype: 'relationcommunalservicegrid',
                                    border: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Сведения о сроках',
                            padding: 5,
                            defaults: {
                                xtype: 'fieldset',
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
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
                                                    inputValue: false,
                                                    itemId: 'NextMonth'
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