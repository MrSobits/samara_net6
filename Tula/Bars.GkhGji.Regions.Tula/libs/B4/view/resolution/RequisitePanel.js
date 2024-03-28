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

        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.YesNo'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            title: 'Реквизиты',
            bodyPadding: 5,
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
                            labelWidth: 130
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
                                    xtype: 'b4combobox',
                                    name: 'BecameLegal',
                                    fieldLabel: 'Вступило в законную силу',
                                    store: B4.enums.YesNo.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Display',
                                    labelWidth: 200,
                                    width: 295,
                                    labelAlign: 'right'
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
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
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
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelWidth: 230,
                                labelAlign: 'right',
                                xtype: 'textfield',
                                disabled: true
                            },
                            margin: '5 0 0 0',
                            title: 'Реквизиты физ. лица',
                            items: [
                                {
                                    name: 'PhysPersonAddress',
                                    fieldLabel: 'Адрес (место жительства, телефон)',
                                    itemId: 'tfPhysPersonAddress'
                                },
                                {
                                    name: 'PhysPersonJob',
                                    fieldLabel: 'Место работы',
                                    itemId: 'tfPhysPersonJob'
                                },
                                {
                                    name: 'PhysPersonPosition',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPhysPersonPosition'
                                },
                                {
                                    name: 'PhysPersonBirthdayAndPlace',
                                    fieldLabel: 'Дата, место рождения',
                                    itemId: 'tfPhysPersonBirthdayAndPlace'
                                },
                                {
                                    name: 'PhysPersonDocument',
                                    fieldLabel: 'Документ, удостоверяющий личность',
                                    itemId: 'tfPhysPersonDocument'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    disabled: false,
                                    defaults: {
                                        labelAlign: 'right',
                                        xtype: 'textfield',
                                        flex: 1,
                                        disabled: true
                                    },
                                    items: [
                                        {
                                            name: 'PhysPersonSalary',
                                            fieldLabel: 'Заработная плата',
                                            itemId: 'tfPhysPersonSalary',
                                            labelWidth: 230
                                        },
                                        {
                                            name: 'PhysPersonMaritalStatus',
                                            fieldLabel: 'Семейное положение, кол-во иждивенцев',
                                            itemId: 'tfPhysPersonMaritalStatus',
                                            labelWidth: 260
                                        }
                                    ]
                                }
                            ]
                        }                        
                    ]                    
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    itemId: 'taDescriptionResolution',
                    fieldLabel: 'Состав административного правонарушения',
                    labelAlign: 'right',
                    labelWidth: 180,                    
                    readOnly: true
                }
            ]
        });

        me.callParent(arguments);
    }
});