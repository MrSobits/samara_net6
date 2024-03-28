Ext.define('B4.view.manorg.contract.OwnersEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.ownerseditwindow',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.view.realityobj.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.realityobj.Grid',
        'B4.enums.ManOrgContractOwnersFoundation',
        'B4.enums.ManOrgSetPaymentsOwnersFoundation',
        'B4.store.realityobj.ByManOrg',
        'B4.enums.ContractStopReasonEnum',
        'B4.view.manorg.contract.OwnersCommunalServiceGrid',
        'B4.view.manorg.contract.OwnersAdditionServiceGrid',
        'B4.view.manorg.contract.OwnersWorkServiceGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 853,
    minWidth: 853,
    maxWidth: 853,
    height: 660,
    minHeight: 375,
    bodyPadding: 5,
    itemId: 'manorgContractOwnersEditWindow',
    title: 'Форма редактирования договора управления (с собственниками)',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 160,
                labelAlign: 'right'
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
                    defaults: {
                        autoScroll: true
                    },
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
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Реквизиты',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'ContractFoundation',
                                            fieldLabel: 'Основание',
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            store: B4.enums.ManOrgContractOwnersFoundation.getStore()
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RealityObjectId',
                                            itemId: 'sfRealityObject',
                                            fieldLabel: 'Жилой дом',
                                            store: 'B4.store.realityobj.ByManOrg',
                                            textProperty: 'Address',
                                            editable: false,
                                            allowBlank: false,
                                            columns: [
                                                {
                                                    text: 'Муниципальное образование',
                                                    dataIndex: 'Municipality',
                                                    flex: 1,
                                                    filter: {
                                                        xtype: 'b4combobox',
                                                        operand: CondExpr.operands.eq,
                                                        storeAutoLoad: false,
                                                        hideLabel: true,
                                                        editable: false,
                                                        valueField: 'Name',
                                                        emptyItem: { Name: '-' },
                                                        url: '/Municipality/ListWithoutPaging'
                                                    }
                                                },
                                                {
                                                    text: 'Адрес',
                                                    dataIndex: 'Address',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
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
                                    title: 'Протокол',
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
                                            fieldLabel: 'Протокол'
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'OwnersSignedContractFile',
                                            fieldLabel: 'Реестр собственников, подписавших договор'
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
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                                allowBlank: false,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumber',
                                                    fieldLabel: 'Номер документа',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentDate',
                                                    fieldLabel: 'от',
                                                    labelWidth: 50,
                                                    maxWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                xtype: 'datefield',
                                                labelAlign: 'right',
                                                format: 'd.m.Y',
                                                flex: 1,
                                                labelWidth: 150
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
                                                    labelWidth: 160
                                                },
                                                {
                                                    name: 'EndDate',
                                                    fieldLabel: 'Дата окончания управления',
                                                    labelWidth: 170,
                                                    type: 'terminate'
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
                                            maxLength: 300,
                                            height: 40
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
                                            xtype: 'b4enumcombo',
                                            name: 'ContractStopReason',
                                            fieldLabel: 'Основание завершения обслуживания',
                                            enumName: 'B4.enums.ContractStopReasonEnum',
                                            includeEmpty: true,
                                            type: 'terminate',
                                            validator: function () {
                                                if (this.getValue() === B4.enums.ContractStopReasonEnum.is_not_filled && !this.allowBlank) {
                                                    return 'Это поле обязательно для заполнения';
                                                }

                                                return true;
                                            }
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TerminateReason',
                                            fieldLabel: 'Основание расторжения',
                                            maxLength: 300,
                                            minHeight: 22,
                                            height: 40,
                                            flex: 1,
                                            type: 'terminate'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'TerminationDate',
                                                    fieldLabel: 'Дата расторжения',
                                                    type: 'terminate'
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
                                minHeight: 200,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'contractownersadditionservicegrid',
                                    border: true,
                                    padding: '0 0 5 0'
                                },
                                {
                                    xtype: 'contractownerscommunalservicegrid',
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
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'InputMeteringDeviceValuesBeginDate',
                                                    fieldLabel: 'День месяца начала ввода показаний по ПУ (от 1 до 30)',
                                                    flex: 1,
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    padding: 5,
                                                    name: 'IsLastDayMeteringDeviceValuesBeginDate',
                                                    fieldLabel: 'Последний день месяца',
                                                    flex: 0.5
                                                }
                                            ]
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
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'InputMeteringDeviceValuesEndDate',
                                                    fieldLabel: 'День месяца окончания ввода показаний по ПУ (от 2 до 31)',
                                                    maxValue: 31,
                                                    minValue: 2,
                                                    flex: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    padding: 5,
                                                    name: 'IsLastDayMeteringDeviceValuesEndDate',
                                                    fieldLabel: 'Последний день месяца',
                                                    flex: 0.5
                                                }
                                            ]
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
                                                    itemId: 'ThisMonth',
                                                    inputValue: true
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
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'DrawingPaymentDocumentDate',
                                                    fieldLabel: 'День месяца (от 1 до 30)',
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    flex: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsLastDayDrawingPaymentDocument',
                                                    fieldLabel: 'Последний день месяца',
                                                    flex: 0.5
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'radiogroup',
                                            itemId: 'docDateMonth',
                                            name: 'ThisMonthPaymentDocDate',
                                            width: 500,
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
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'PaymentServicePeriodDate',
                                                    fieldLabel: 'День месяца (от 1 до 30)',
                                                    maxValue: 30,
                                                    minValue: 1,
                                                    flex: 1,
                                                    allowDecimals: false
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsLastDayPaymentServicePeriodDate',
                                                    fieldLabel: 'Последний день месяца',
                                                    flex: 0.5
                                                }
                                            ]
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
                                            padding: '0 0 5 0',
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
                                                    padding: '0 5 0 0',
                                                    format: 'd.m.Y'

                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'EndDatePaymentPeriod',
                                                    fieldLabel: 'Дата окончания периода',
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
                                        labelAlign: 'right'
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
                                            fieldLabel: 'Протокол',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'PaymentProtocolDescription',
                                            fieldLabel: 'Описание',
                                            height: 40,
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'contractownersworkservicegrid',
                                    border: true,
                                    minHeight: 200,
                                    flex: 1
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'RevocationReason',
                                    fieldLabel: 'Причина аннулирования',
                                    margin: '5px',
                                    labelWidth: 162,
                                    minHeight: 40,
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
                            columns: 2,
                            items: [
                                {
                                     xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Отправить договор в ГИС ЖКХ',
                                    itemId: 'btnSendDu'
                                }
                            ]
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