Ext.define('B4.view.resolution.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.resolutionRequisitePanel',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.TypeRepresentativePresence',
        'B4.enums.YesNoNotSet',
        'B4.enums.ResolutionPaymentStatus',
        'B4.enums.TypeTerminationBasement',
        'B4.form.TreeSelectField',
        'B4.store.dict.JurInstitution',
        'B4.enums.OSPDecisionType',
        'B4.store.dict.PhysicalPersonDocType'
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
                            //labelWidth: 130,
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'GisUin',
                            itemId: 'tfGisUin',
                            fieldLabel: 'УИН',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'AbandonReason',
                            itemId: 'tfAbandonReason',
                            fieldLabel: 'Причина аннулирования'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right'

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
                            value: 30,
                            width: 230

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
                            padding: '5 0 5 0',
                            xtype: 'textarea',
                            labelAlign: 'right',
                            name: 'Established',
                            fieldLabel: 'Состав АП',
                            labelWidth: 130,
                            flex: 1,
                            itemId: 'taEstablished',
                            maxLength: 2500
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
           
                        //{
                        //    padding: '5 0 5 0',
                        //    xtype: 'textarea',
                        //    labelAlign: 'right',
                        //    name: 'Description',
                        //    fieldLabel: 'Состав АП',
                        //    labelWidth: 130,
                        //    flex: 1,
                        //    itemId: 'taDescription',
                        //    maxLength: 30000
                        //}
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsJudicalOffice',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    title: 'Судебное решение',
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
                                     xtype: 'textfield',
                                     name: 'DecisionNumber',
                                     itemId: 'tfDecisionNumber',
                                     fieldLabel: 'Номер решения судебного участка',
                                     allowBlank: false,
                                     flex: 1
                                 },
                                {
                                    xtype: 'datefield',
                                    name: 'DecisionDate',
                                    labelWidth: 200,
                                    itemId: 'dfDecisionDate',
                                    width: 300,
                                    allowBlank: false,
                                    fieldLabel: 'Дата решения',
                                    format: 'd.m.Y'
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
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                    name: 'JudicalOffice',
                                    itemId: 'sfJudicalOffice',
                                    fieldLabel: 'Судебный участок',
                                    store: 'B4.store.dict.JurInstitution',
                                    readOnly: false,
                                    flex: 1,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DecisionEntryDate',
                                    labelWidth: 200,
                                    width: 300,
                                    allowBlank: true,
                                    fieldLabel: 'Дата вступления в законную силу',
                                    format: 'd.m.Y'
                                }

                            ]
                        },
                          {
                              xtype: 'textfield',
                              name: 'Violation',
                              fieldLabel: 'Нарушение',
                              allowBlank: true,
                              flex: 1
                          },
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
                                    name: 'PayStatus',
                                    fieldLabel: 'Штраф оплачен',
                                    displayField: 'Display',
                                    store: B4.enums.ResolutionPaymentStatus.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbPaided'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbPayded50Percent',
                                    name: 'Payded50Percent',
                                    fieldLabel: '50%',
                                    allowBlank: true,
                                    flex: 0.3
                                    //editable: true
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbWrittenOff',
                                    name: 'WrittenOff',
                                    fieldLabel: 'Списан',
                                    allowBlank: true,
                                    flex: 0.3
                                    //editable: true
                                }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
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
                            xtype: 'textarea',
                            margin: '3px, 50px',
                            labelWidth: 80,
                            name: 'WrittenOffComment',
                            hidden:true,
                            fieldLabel: 'Причина списания',
                            itemId: 'taWrittenOffComment',
                            maxLength: 500
                        },
                        {
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    padding: '5 0 5 0',
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
                                    xtype: 'container'
                                }
                            ]
                        },
                       {
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumSsp',
                                    fieldLabel: 'Номер документа переданного в ССП',
                                    itemId: 'tfDocumentNumSsp',
                                    maxLength: 300,
                                    flex: 1,
                                    labelAlign: 'right',
                                    labelWidth: 130
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'OSPDecisionType',
                                    fieldLabel: 'Решение ССП',
                                    labelAlign: 'right',
                                    itemId: 'cbOSPDecisionType',
                                    store: B4.enums.OSPDecisionType.getStore(),
                                    valueField: 'Value',
                                    labelWidth: 130,
                                    flex: 1,
                                    displayField: 'Display',
                                    editable: false
                                },
                            ]
                        },
{
                            defaults: {
                                padding: '5 0 5 0',
                                allowBlank: true,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.dict.JurInstitution',
                                    textProperty: 'ShortName',
                                    name: 'OSP',
                                    flex: 1,
                                    fieldLabel: 'Отдел ССП',
                                    labelWidth: 130,
                                    itemId: 'sfSSP',
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
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ExecuteSSPNumber',
                                    fieldLabel: 'Номер ИП',
                                    labelWidth: 130,
                                    flex: 1,
                                    itemId: 'tfExecuteSSPNumber',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            defaults: {
                                padding: '5 0 5 0',
                                allowBlank: true,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                   {
                                       xtype: 'datefield',
                                       name: 'DateExecuteSSP',
                                       fieldLabel: 'Дата начала ИП',
                                       itemId: 'dfDateExecuteSSP'
                                   },
                                   {
                                       xtype: 'datefield',
                                       name: 'DateOSPListArrive',
                                       fieldLabel: 'Дата вручения ИД',
                                       itemId: 'dfDateOSPListArrive'
                                   },
                                   {
                                       xtype: 'datefield',
                                       name: 'DateEndExecuteSSP',
                                       fieldLabel: 'Дата окончания ИП',
                                       itemId: 'dfDateEndExecuteSSP'
                                   }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
                            xtype: 'textfield',
                            name: 'Comment',
                            fieldLabel: 'Комментарий',
                            labelWidth: 130,
                            flex: 1,
                            itemId: 'tfComment',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Документ выдан',
                    name: 'fsReciever',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
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
                                    xtype: 'datefield',
                                    name: 'DateWriteOut',
                                    fieldLabel: 'Дата выписки из ЕГРЮЛ',
                                    itemId: 'dfDateWriteOut'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.Contragent',
                                    textProperty: 'ShortName',
                                    name: 'Contragent',
                                    fieldLabel: 'Контрагент',
                                    itemId: 'sfContragent',
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
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    itemId: 'tfPhysPerson',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Surname',
                                    fieldLabel: 'Фамилия',
                                    itemId: 'tfSurname',
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FirstName',
                                    fieldLabel: 'Имя',
                                    itemId: 'tfFirstName',
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Patronymic',
                                    fieldLabel: 'Отчество',
                                    itemId: 'tfPatronymic',
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Position',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPosition',
                                    maxLength: 255
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'PhysicalPersonDocType',
                                    fieldLabel: 'Вид документа ФЛ',
                                    store: 'B4.store.dict.PhysicalPersonDocType',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'dfPhysicalPersonDocType',
                                    allowBlank: true,
                                    columns: [
                                        { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentSerial',
                                    itemId: 'dfPhysicalPersonDocumentSerial',
                                    fieldLabel: 'Серия документа ФЛ',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentNumber',
                                    itemId: 'dfPhysicalPersonDocumentNumber',
                                    fieldLabel: 'Номер документа ФЛ',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'dfPhysicalPersonIsNotRF',
                                    name: 'PhysicalPersonIsNotRF',
                                    fieldLabel: 'Не является гражданином РФ',
                                    allowBlank: true,
                                    flex: 1
                                    //editable: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'PersonBirthDate',
                                    fieldLabel: 'Дата рождения',
                                    disabled: true,
                                    itemId: 'dfPersonBirthDate'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PersonBirthPlace',
                                    fieldLabel: 'Место рождения',
                                    itemId: 'tfPersonBirthPlace',
                                    maxLength: 500,
                                    disabled: true,
                                    flex: 1,
                                    labelWidth: 100
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес регистрации места жительства',
                            name: 'PersonRegistrationAddress',
                            itemId: 'tfRegistrationAddress',
                            maxLength: 250,
                            disabled: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес фактического места жительства',
                            name: 'PersonFactAddress',
                            itemId: 'tfFactAddress',
                            maxLength: 250,
                            disabled: true
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypePresence',
                                    fieldLabel: 'В присутствии/отсутствии',
                                    itemId: 'ecTypePresence',
                                    width: 450,
                                    minWidth: 450,
                                    enumName: B4.enums.TypeRepresentativePresence
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Representative',
                                    fieldLabel: 'Представитель',
                                    itemId: 'tfRepresentative',
                                    maxLength: 500,
                                    disabled: true,
                                    flex: 1,
                                    labelWidth: 100
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'ReasonTypeRequisites',
                            itemId: 'taReasonTypeRequisites',
                            maxLength: 1000,
                            disabled: true,
                            fieldLabel: 'Вид и реквизиты основания'
                        },
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            fieldLabel: 'Реквизиты физ. лица',
                            itemId: 'taPhysPersonInfo',
                            maxLength: 500
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});