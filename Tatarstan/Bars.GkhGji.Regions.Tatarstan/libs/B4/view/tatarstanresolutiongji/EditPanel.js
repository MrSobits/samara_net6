Ext.define('B4.view.tatarstanresolutiongji.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    alias: 'widget.tatarstanresolutiongjieditpanel',
    title: 'Постановление',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.store.DocumentGji',
        'B4.store.dict.Citizenship',
        'B4.store.dict.Municipality',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Delete',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.TypeTerminationBasement',
        'B4.enums.TypeDocumentGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeExecutantProtocol',
        'B4.enums.CitizenshipType',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.store.dict.Inspector',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.form.TreeSelectField',
        'B4.view.resolution.DisputeGrid',
        'B4.view.resolution.DefinitionGrid',
        'B4.view.resolution.PayFineGrid',
        'B4.view.tatarstanprotocolgji.AnnexGrid',
        'B4.view.tatarstanresolutiongji.WitnessGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        border: false,
                        labelWidth: 160,
                        xtype: 'panel',
                        layout: 'hbox',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '10px 15px 5px 15px',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    width: 295
                                }
                            ]
                        },
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '0 15px 20px 15px',
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 80,
                                    width: 200,
                                    readOnly: true
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true,
                                },
                                {
                                    name: 'LiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    name: 'tatarstanResolutionGjiTabPanel',
                    flex: 1,
                    border: false,
                    defaults: {
                        autoScroll: true
                    },
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                anchor: '100%',
                                                editable: false,
                                                labelWidth: 160,
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'BasisDocumentName',
                                                            fieldLabel: 'Документ-основание',
                                                            readOnly: true
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'DeliveryDate',
                                                            fieldLabel: 'Дата вручения',
                                                            format: 'd.m.Y'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                anchor: '100%',
                                                editable: false,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            name: 'OffenderWas',
                                                            fieldLabel: 'Нарушитель явился на рассмотрение',
                                                            displayField: 'Display',
                                                            store: B4.enums.YesNoNotSet.getStore(),
                                                            valueField: 'Value'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Кем вынесено',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                anchor: '100%',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            name: 'TypeInitiativeOrg',
                                                            fieldLabel: 'Кем вынесено',
                                                            displayField: 'Display',
                                                            store: B4.enums.TypeInitiativeOrgGji.getStore(),
                                                            valueField: 'Value'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'SectorNumber',
                                                            fieldLabel: 'Номер участка'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }, {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                anchor: '100%',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'treeselectfield',
                                                            name: 'FineMunicipality',
                                                            fieldLabel: 'МО получателя штрафа',
                                                            titleWindow: 'Выбор муниципального образования',
                                                            store: 'B4.store.dict.MunicipalitySelectTree',
                                                            editable: false,
                                                            allowBlank: true
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            store: 'B4.store.dict.Inspector',
                                                            name: 'Official',
                                                            fieldLabel: 'Должностное лицо',
                                                            editable: false,
                                                            textProperty: 'Fio',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'Fio',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            store: 'B4.store.dict.Municipality',
                                                            name: 'Municipality',
                                                            fieldLabel: 'Местонахождение',
                                                            editable: false,
                                                            textProperty: 'Name',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'Name',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                                {
                                                                    text: 'Код',
                                                                    dataIndex: 'Code',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
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
                                    defaults: {
                                        xtype: 'container',
                                        anchor: '100%',
                                        flex: 1,
                                        padding: '0 0 7 0'
                                    },
                                    title: 'Санкция',
                                    items: [
                                        {
                                            layout: 'hbox',
                                            defaults: {
                                                editable: false,
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4combobox',
                                                            name: 'Sanction',
                                                            fieldLabel: 'Вид санкции',
                                                            fields: ['Id', 'Name', 'Code'],
                                                            url: '/SanctionGji/List'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            name: 'Paided',
                                                            fieldLabel: 'Штраф оплачен',
                                                            displayField: 'Display',
                                                            store: B4.enums.YesNoNotSet.getStore(),
                                                            valueField: 'Value'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            layout: 'hbox',
                                            defaults: {
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'PenaltyAmount',
                                                            fieldLabel: 'Сумма штрафа'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'DateTransferSsp',
                                                            fieldLabel: 'Дата передачи в ССП'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            layout: 'hbox',
                                            defaults: {
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    name: 'TerminationBasement',
                                                    fieldLabel: 'Основание прекращения',
                                                    displayField: 'Display',
                                                    store: B4.enums.TypeTerminationBasement.getStore(),
                                                    valueField: 'Value'
                                                },
                                            ]
                                        },
                                        {
                                            layout: 'hbox',
                                            defaults: {
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'TerminationDocumentNum',
                                                    fieldLabel: 'Номер документа',
                                                },
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Executant',
                                    editable: false,
                                    fieldLabel: 'Тип исполнителя',
                                    displayField: 'Display',
                                    store: B4.enums.TypeExecutantProtocol.getStore(),
                                    valueField: 'Value',
                                    labelWidth: 170
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'ContragentInfoFieldSet',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Протокол составлен в отношении',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160,
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            store: 'B4.store.Contragent',
                                                            name: 'Contragent',
                                                            fieldLabel: 'Контрагент',
                                                            editable: false,
                                                            readOnly: false,
                                                            textProperty: 'Name',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'ShortName',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                },
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
                                                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                                                    }
                                                                },
                                                                {
                                                                    text: 'ИНН',
                                                                    dataIndex: 'Inn',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                },
                                                                {
                                                                    text: 'КПП',
                                                                    dataIndex: 'Kpp',
                                                                    flex: 1,
                                                                    filter: { xtype: 'textfield' }
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            readOnly: true,
                                                            name: 'Ogrn',
                                                            fieldLabel: 'ОГРН'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160,
                                                        readOnly: true
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Inn',
                                                            fieldLabel: 'ИНН'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        readOnly: true,
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Kpp',
                                                            fieldLabel: 'КПП'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160,
                                                        readOnly: true
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'SettlementAccount',
                                                            fieldLabel: 'Расчетный счет'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        readOnly: true,
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'BankName',
                                                            fieldLabel: 'Банк'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 120,
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CorrAccount',
                                                    fieldLabel: 'Корр. счет',
                                                    labelWidth: 160,
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Bik',
                                                    fieldLabel: 'БИК'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Okpo',
                                                    fieldLabel: 'ОКПО'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Okonh',
                                                    fieldLabel: 'ОКОНХ'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Okved',
                                                    fieldLabel: 'ОКВЭД'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'PersonalInfoFieldSet',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Протокол составлен в отношении',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'SurName',
                                                            fieldLabel: 'Фамилия'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            name: 'CitizenshipType',
                                                            fieldLabel: 'Гражданство',
                                                            displayField: 'Display',
                                                            flex: 2,
                                                            store: B4.enums.CitizenshipType.getStore(),
                                                            valueField: 'Value'
                                                        },
                                                        {
                                                            xtype: 'b4selectfield',
                                                            store: 'B4.store.dict.Citizenship',
                                                            name: 'Citizenship',
                                                            fieldLabel: 'Код страны',
                                                            editable: false,
                                                            flex: 3,
                                                            labelWidth: 80,
                                                            textProperty: 'Name',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'Name',
                                                                    flex: 1
                                                                },
                                                                {
                                                                    text: 'Код ОКСМ',
                                                                    dataIndex: 'OksmCode',
                                                                    flex: 1
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Name',
                                                            fieldLabel: 'Имя'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            store: 'B4.store.dict.IdentityDocumentType',
                                                            name: 'IdentityDocumentType',
                                                            fieldLabel: 'Тип документа',
                                                            editable: false,
                                                            textProperty: 'Name',
                                                            columns: [
                                                                {
                                                                    text: 'Наименование',
                                                                    dataIndex: 'Name',
                                                                    flex: 1
                                                                },
                                                                {
                                                                    text: 'Код',
                                                                    dataIndex: 'Code',
                                                                    flex: 1
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Patronymic',
                                                            fieldLabel: 'Отчество'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'SerialAndNumberDocument',
                                                            fieldLabel: 'Серия и номер документа'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'BirthDate',
                                                            fieldLabel: 'Дата рождения'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'IssueDate',
                                                            fieldLabel: 'Дата выдачи'
                                                        },
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'BirthPlace',
                                                            fieldLabel: 'Место рождения'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'IssuingAuthority',
                                                            fieldLabel: 'Кем выдан'
                                                        }
                                                    ]
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Address',
                                                            fieldLabel: 'Фактический адрес проживания'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Company',
                                                            fieldLabel: 'Место работы, должность, адрес'
                                                        }
                                                    ]
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'MaritalStatus',
                                                            fieldLabel: 'Семейное положение'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RegistrationAddress',
                                                            fieldLabel: 'Адрес регистрации'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhintfield',
                                                            name: 'DependentCount',
                                                            fieldLabel: 'Количество иждивенцев'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'Salary',
                                                            fieldLabel: 'Размер зарплаты (пенсии, стипендии) в руб.'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'ResponsibilityPunishment',
                                            fieldLabel: 'Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались',
                                            margin: '5 20 0 0',
                                            labelWidth: 500
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Законный представитель',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160,
                                                border: false
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'DelegateFio',
                                                            fieldLabel: 'ФИО'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ProcurationNumber',
                                                            fieldLabel: 'Доверенность номер',
                                                            labelWidth: 160
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'ProcurationDate',
                                                            fieldLabel: 'Дата',
                                                            format: 'd.m.Y',
                                                            labelWidth: 60,
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'DelegateCompany',
                                                            fieldLabel: 'Место работы, должность'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'checkbox',
                                                            name: 'DelegateResponsibilityPunishment',
                                                            fieldLabel: 'Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались',
                                                            labelWidth: 500
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Обстоятельства правонарушения',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textarea',
                                                            name: 'ImprovingFact',
                                                            fieldLabel: 'Смягчающие вину обстоятельства',
                                                            maxLength: 255
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textarea',
                                                            name: 'DisimprovingFact',
                                                            fieldLabel: 'Отягчающие вину обстоятельства',
                                                            maxLength: 255
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'right'
                                    },
                                    title: 'Направлении копии постановления',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 7 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RulinFio',
                                                            fieldLabel: 'ФИО'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    border: false,
                                                    layout: 'hbox',
                                                    padding: '0 0 7 0',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        flex: 1,
                                                        labelWidth: 160
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'RulingDate',
                                                            fieldLabel: 'Дата',
                                                            format: 'd.m.Y',
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            border: false,
                                            layout: 'hbox',
                                            padding: '0 0 7 0',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 160
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RulingNumber',
                                                    fieldLabel: 'Номер'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        { xtype: 'resolutionDisputeGrid', flex: 1 },
                        { xtype: 'resolutionDefinitionGrid', flex: 1 },
                        { xtype: 'resolutionPayFineGrid', flex: 1 },
                        { xtype: 'tatarstanprotocolgjiannexgrid', flex: 1, itemId: 'resolutionAnnexGrid' },
                        { xtype: 'tatarstanresolutiongjieyewitnessgrid', flex: 1 }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'b4deletebutton',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'btnState',
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