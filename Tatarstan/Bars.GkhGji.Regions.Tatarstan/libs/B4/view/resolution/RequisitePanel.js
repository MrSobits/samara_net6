Ext.define('B4.view.resolution.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.resolutionRequisitePanel',
    itemId: 'resolutionRequisitePanel',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.store.GisGmpPatternDict',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.store.dict.Citizenship',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeTerminationBasement',
        'B4.enums.CitizenshipType',
        'B4.form.TreeSelectField'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            title: 'Реквизиты',
            bodyPadding: 5,
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'resolutionBaseName',
                            itemId: 'tfBaseName',
                            fieldLabel: 'Документ-основание',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'DeliveryDate',
                            fieldLabel: 'Дата вручения',
                            format: 'd.m.Y',
                            itemId: 'dfDeliveryDate',
                            labelWidth: 130,
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'OffenderWas',
                            fieldLabel: 'Нарушитель явился на рассмотрение',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            itemId: 'offenderWas',
                            editable: false,
                            value: 30

                        },
                        {
                            xtype: 'component'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    title: 'ГИС ГМП',
                    items: [
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.GisGmpPatternDict',
                                    textProperty: 'PatternName',
                                    name: 'PatternDict',
                                    fieldLabel: 'Шаблон ГИС ГМП',
                                    itemId: 'patternDict',
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'PatternName',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        },
                                        {
                                            text: 'Код',
                                            dataIndex: 'PatternCode',
                                            flex: 1,
                                            filter: {
                                                xtype: 'numberfield',
                                                minValue: 0,
                                                hideTrigger: true
                                            }
                                        }
                                    ],
                                    listeners: {
                                        beforeload: function (field, options) {
                                            options.params.relevanceFilter = true;
                                        }
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GisUin',
                                    itemId: 'tfGisUin',
                                    fieldLabel: 'УИН',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'AbandonReason',
                                    itemId: 'tfAbandonReason',
                                    fieldLabel: 'Причина аннулирования'
                                },
                                {
                                    padding: '5 0 5 0',
                                    xtype: 'textfield',
                                    name: 'ChangeReason',
                                    itemId: 'changeReason',
                                    fieldLabel: 'Причина изменения'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    title: 'Кем вынесено',
                    items: [
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'TypeInitiativeOrg',
                                    fieldLabel: 'Кем вынесено',
                                    displayField: 'Display',
                                    store: B4.enums.TypeInitiativeOrgGji.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbTypeInitOrg',
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'SectorNumber',
                                    fieldLabel: 'Номер участка',
                                    itemId: 'tfSectorNumber',
                                    maxLength: 250
                                }
                            ]
                        },
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'treeselectfield',
                                    name: 'FineMunicipality',
                                    itemId: 'tsfFineMunicipality',
                                    fieldLabel: 'МО получателя штрафа',
                                    titleWindow: 'Выбор муниципального образования',
                                    store: 'B4.store.dict.MunicipalitySelectTree',
                                    editable: false
                                },
                                {
                                    xtype: 'component'
                                }

                            ]
                        },
                        {
                            defaults: {
                                xtype: 'b4selectfield',
                                editable: false,
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    name: 'Official',
                                    fieldLabel: 'Должностное лицо',
                                    allowBlank: true,
                                    itemId: 'sfOfficial',
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: 'B4.store.dict.Inspector',
                                            dock: 'bottom'
                                        }
                                    ]
                                },
                                {
                                    store: 'B4.store.dict.Municipality',
                                    textProperty: 'Name',
                                    name: 'Municipality',
                                    fieldLabel: 'Местонахождение',
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1 },
                                        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1 }
                                    ],
                                    itemId: 'sfMunicipality'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%',
                        flex: 1
                    },
                    title: 'Санкция',
                    items: [
                        {
                            defaults: {
                                allowBlank: false,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Sanction',
                                    fieldLabel: 'Вид санкции',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/SanctionGji/List',
                                    itemId: 'cbSanction'
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Paided',
                                    fieldLabel: 'Штраф оплачен',
                                    displayField: 'Display',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbPaided'
                                }
                            ]
                        },
                        {
                            padding: '10 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyAmount',
                                    fieldLabel: 'Сумма штрафа',
                                    itemId: 'nfPenaltyAmount'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateTransferSsp',
                                    fieldLabel: 'Дата передачи в ССП',
                                    itemId: 'dfDateTransferSsp'
                                }
                            ]
                        },
                        {
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'SanctionsDuration',
                                    fieldLabel: 'Срок накладываемых санкций',
                                    itemId: 'tfSanctionsDuration',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container'
                                }
                            ]
                        },
                        {
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'TypeTerminationBasement',
                                    fieldLabel: 'Основание прекращения',
                                    itemId: 'cbTermination',
                                    store: B4.enums.TypeTerminationBasement.getStore(),
                                    valueField: 'Value',
                                    displayField: 'Display',
                                    editable: false
                                },
                                {
                                    xtype: 'component'
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumSsp',
                                    fieldLabel: 'Номер документа',
                                    itemId: 'tfDocumentNumSsp',
                                    maxLength: 300,
                                    flex: 0.5,
                                    labelAlign: 'right',
                                    labelWidth: 130
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Документ выдан',
                    name: 'fsReciever',
                    items: [
                        {
                            xtype: 'b4combobox',
                            itemId: 'cbExecutant',
                            name: 'Executant',
                            allowBlank: false,
                            editable: false,
                            fieldLabel: 'Тип исполнителя',
                            fields: ['Id', 'Name', 'Code'],
                            url: '/ExecutantDocGji/List',
                            queryMode: 'local',
                            triggerAction: 'all'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                padding: '0 0 5 0',
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateWriteOut',
                                    fieldLabel: 'Дата выписки из ЕГРЮЛ',
                                    itemId: 'dfDateWriteOut',
                                    labelWidth: 130,
                                },
                                {
                                    padding: '5 0 5 0',
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    allowBlank: true,
                                    store: 'B4.store.Contragent',
                                    textProperty: 'ShortName',
                                    name: 'Contragent',
                                    fieldLabel: 'Контрагент',
                                    itemId: 'sfContragent',
                                    labelWidth: 130,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    border: false,
                                    layout: {
                                        padding: '0 0 5 0',
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: true,
                                        labelWidth: 130
                                    },
                                    itemId: 'fsRecieverInfo',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'SurName',
                                            fieldLabel: 'Фамилия',
                                            itemId: 'surName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            fieldLabel: 'Имя',
                                            itemId: 'firstName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Patronymic',
                                            fieldLabel: 'Отчество',
                                            itemId: 'patronymic'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'BirthDate',
                                            fieldLabel: 'Дата рождения',
                                            itemId: 'birthDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'BirthPlace',
                                            fieldLabel: 'Место рождения',
                                            itemId: 'birthPlace'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Address',
                                            fieldLabel: 'Фактический адрес проживания',
                                            itemId: 'address'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    border: false,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: true,
                                        labelWidth: 130
                                    },
                                    itemId: 'fsRecieverReq',
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'CitizenshipType',
                                            width: 270,
                                            fieldLabel: 'Гражданство',
                                            labelAlign: 'right',
                                            enumName: 'B4.enums.CitizenshipType',
                                            itemId: 'citizenshipType'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Citizenship',
                                            name: 'Citizenship',
                                            fieldLabel: 'Код страны',
                                            hidden: true,
                                            textProperty: 'OksmCode',
                                            columns: [
                                                {
                                                    text: 'Наименование Страны',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    text: 'Код ОКСМ',
                                                    dataIndex: 'OksmCode',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield', hideTrigger: true, operand: 'eq' }
                                                }
                                            ],
                                            itemId: 'citizenship'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'SerialAndNumber',
                                            fieldLabel: 'Серия и номер паспорта',
                                            itemId: 'serialAndNumber',
                                            maxLength: 10,
                                            minLength: 10,
                                            maskRe: /[0-9]/
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'IssueDate',
                                            fieldLabel: 'Дата выдачи',
                                            itemId: 'issueDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'IssuingAuthority',
                                            fieldLabel: 'Кем выдан',
                                            itemId: 'issuingAuthority'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Snils',
                                            fieldLabel: 'СНИЛС',
                                            itemId: 'tfSnils',
                                            maxLength: 14
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Company',
                                            fieldLabel: 'Место работы, должность',
                                            itemId: 'company'
                                        }
                                        
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            fieldLabel: 'Комментарий',
                            itemId: 'physicalPersonInfo',
                            allowblank: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 130,
                        labelAlign: 'right'
                    },
                    title: 'Постановление вынесено в отношении',
                    name: 'fsPerson',
                    items: [
                        {
                            xtype: 'combobox',
                            padding: '0 0 5 0',
                            itemId: 'fsExecutant',
                            name: 'TypeExecutant',
                            editable: false,
                            fieldLabel: 'Тип исполнителя',
                            displayField: 'Display',
                            store: B4.enums.TypeExecutantProtocolMvd.getStore(),
                            valueField: 'Value'
                        },
                        {
                            xtype: 'container',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    border: false,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        padding: '0 0 5 0',
                                        flex: 1,
                                        readOnly: true,
                                        allowBlank: true,
                                        labelWidth: 130
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdSurName',
                                            fieldLabel: 'Фамилия',
                                            itemId: 'protocolMvdSurName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdName',
                                            fieldLabel: 'Имя',
                                            itemId: 'protocolMvdName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdPatronymic',
                                            fieldLabel: 'Отчество',
                                            itemId: 'protocolMvdPatronymic'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ProtocolMvdBirthDate',
                                            fieldLabel: 'Дата рождения',
                                            itemId: 'protocolMvdBirthDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdBirthPlace',
                                            fieldLabel: 'Место рождения',
                                            itemId: 'protocolMvdBirthPlace'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'ProtocolMvdPhysicalPersonInfo',
                                            fieldLabel: 'Фактический адрес проживания',
                                            itemId: 'taPhysPersonInfo'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    border: false,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        padding: '0 0 5 0',
                                        flex: 1,
                                        readOnly: true,
                                        allowBlank: true,
                                        labelWidth: 130
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'ProtocolMvdCitizenshipType',
                                            width: 270,
                                            fieldLabel: 'Гражданство',
                                            labelAlign: 'right',
                                            enumName: 'B4.enums.CitizenshipType',
                                            itemId: 'protocolMvdCitizenshipType'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Citizenship',
                                            name: 'ProtocolMvdCitizenship',
                                            fieldLabel: 'Код страны',
                                            hidden: true,
                                            textProperty: 'OksmCode',
                                            columns: [
                                                {
                                                    text: 'Наименование Страны',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    text: 'Код ОКСМ',
                                                    dataIndex: 'OksmCode',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield', hideTrigger: true, operand: 'eq' }
                                                }
                                            ],
                                            itemId: 'protocolMvdCitizenship'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdSerialAndNumber',
                                            fieldLabel: 'Серия и номер паспорта',
                                            itemId: 'protocolMvdSerialAndNumber'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ProtocolMvdIssueDate',
                                            fieldLabel: 'Дата выдачи',
                                            itemId: 'protocolMvdIssueDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdIssuingAuthority',
                                            fieldLabel: 'Кем выдан',
                                            itemId: 'protocolMvdIssuingAuthority'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolMvdCompany',
                                            fieldLabel: 'Место работы, должность',
                                            itemId: 'protocolMvdCompany'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'ProtocolMvdCommentary',
                            fieldLabel: 'Комментарий',
                            itemId: 'protocolMvdCommentary',
                            allowBlank: true,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Направление копии постановления',
                    name: 'fsCopy',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1,
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'RulinFio',
                                    fieldLabel: 'ФИО',
                                    itemId: 'rulinFio',
                                    maxLength: 150
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'RulingDate',
                                    fieldLabel: 'Дата',
                                    width: 400,
                                    itemId: 'rulingDate'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                flex: 0.5,
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'RulingNumber',
                                    fieldLabel: 'Номер',
                                    itemId: 'rulingNumber',
                                    maxLength: 50,
                                    maskRe: /[0-9]/i,
                                    width: 200
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