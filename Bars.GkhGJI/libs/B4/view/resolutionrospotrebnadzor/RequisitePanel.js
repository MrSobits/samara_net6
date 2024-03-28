Ext.define('B4.view.resolutionrospotrebnadzor.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.resolutionRospotrebnadzorRequisitePanel',

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
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeTerminationBasement',
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
                    xtype: 'fieldset',
                    border: 0,
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentReason',
                                    itemId: 'tfDocumentReason',
                                    fieldLabel: 'Документ-основание',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DeliveryDate',
                                    itemId: 'dfDeliveryDate',
                                    fieldLabel: 'Дата вручения',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
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
                                    name: 'RevocationReason',
                                    itemId: 'tfRevocationReason',
                                    fieldLabel: 'Причина аннулирования',
                                    maxlength: 255
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
                        padding: '0 0 5 0'
                    },
                    title: 'Кем вынесено',
                    items: [
                        {
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'TypeInitiativeOrg',
                                    itemId: 'tfTypeInitiativeOrg',
                                    items: B4.enums.TypeInitiativeOrgGji.getItems(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    fieldLabel: 'Кем вынесено',
                                    readOnly: true,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'LocationMunicipality',
                                    itemId: 'sfLocationMunicipality',
                                    store: 'B4.store.dict.Municipality',
                                    textProperty: 'Name',
                                    fieldLabel: 'Местонахождение',
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1 },
                                        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1 }
                                    ]
                                }
                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 150,
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
                                    editable: false,
                                    allowBlank: true
                                },
                                {
                                    xtype: 'component'
                                }

                            ]
                        },
                        {
                            defaults: {
                                editable: false,
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Official',
                                    itemId: 'sfOfficial',
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    fieldLabel: 'Должностное лицо',
                                    allowBlank: false,
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
                                    xtype: 'container'
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
                        flex: 1,
                        padding: '0 0 5 0'
                    },
                    title: 'Санкция',
                    items: [
                        {
                            defaults: {
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Sanction',
                                    itemId: 'cbSanction',
                                    fieldLabel: 'Вид санкции',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/SanctionGji/List',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Paided',
                                    itemId: 'cbPaided',
                                    fieldLabel: 'Штраф оплачен',
                                    displayField: 'Display',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    valueField: 'Value'
                                }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyAmount',
                                    itemId: 'nfPenaltyAmount',
                                    fieldLabel: 'Сумма штрафа'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TransferToSspDate',
                                    itemId: 'dfTransferToSspDate',
                                    fieldLabel: 'Дата передачи в ССП'
                                }
                            ]
                        },
                        {
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'ExpireReason',
                                    itemId: 'cbExpireReason',
                                    padding: '5 0 5 0',
                                    fieldLabel: 'Основание прекращения',
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
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'SspDocumentNum',
                                    itemId: 'tfSspDocumentNum',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 255,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right',
                        anchor: '100%',
                        labelWidth: 150
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
                                    name: 'Executant',
                                    itemId: 'cbExecutant',
                                    allowBlank: false,
                                    editable: false,
                                    fieldLabel: 'Тип исполнителя',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/ExecutantDocGji/List',
                                    queryMode: 'local',
                                    triggerAction: 'all'
                                },
                                {
                                    xtype: 'container'
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
                                    allowBlank: false,
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
                                    itemId: 'tfPhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    maxLength: 255
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            itemId: 'taPhysPersonInfo',
                            fieldLabel: 'Реквизиты физ. лица',
                            maxLength: 255
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});