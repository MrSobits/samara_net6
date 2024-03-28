Ext.define('B4.view.transferrf.RequestEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.ContractRfByManOrg',
        'B4.store.dict.ProgramCr',
        'B4.store.contragent.Bank',
        'B4.store.ManagingOrganization',
        'B4.store.objectcr.PersonalAccount',
        'B4.view.contractrf.Grid',

        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramRequest'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 780,
    height: 550,
    bodyPadding: 3,
    itemId: 'requestTransferRfEditWindow',
    title: 'Заявка на перечисление средств',
    closeAction: 'hide',
    trackResetOnLoad: true,
    maximized: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    bodyPadding: 3,
                    height: 75,
                    layout: 'form',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNum',
                                            itemId: 'tfDocumentNum',
                                            fieldLabel: 'Номер заявки',
                                            maxLength: 50,
                                            labelWidth: 190,
                                            anchor: '100%',
                                            labelAlign: 'right',
                                            readOnly: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'DateFrom',
                                            itemId: 'dfDateFrom',
                                            fieldLabel: 'Дата заявки',
                                            labelWidth: 120,
                                            anchor: '100%',
                                            labelAlign: 'right',
                                            allowBlank: false
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    labelWidth: 190,
                                    labelAlign: 'right',
                                    xtype: 'b4selectfield',
                                    name: 'ManagingOrganization',
                                    itemId: 'sfManagingOrganization',
                                    textProperty: 'ContragentName',
                                    fieldLabel: 'Управляющая организация',

                                    windowContainerSelector: '#requestTransferRfEditWindow',

                                    store: 'B4.store.ManagingOrganization',
                                    allowBlank: false,
                                    editable: false,
                                    flex: 1,
                                    columns: [
                                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                        { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    width: 90,
                                    name: 'ContragentEditButton',
                                    itemId: 'btnEditContragentRf',
                                    text: 'Контрагент',
                                    margin: '0 0 0 10',
                                    iconCls: 'icon-pencil-go'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    bodyPadding: 3,
                    flex: 1,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Реквизиты',
                            layout: 'anchor',
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        readOnly: true,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ContragentInn',
                                            itemId: 'tfInn',
                                            fieldLabel: 'ИНН получателя',
                                            labelWidth: 190
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ContragentKpp',
                                            itemId: 'tfKpp',
                                            fieldLabel: 'КПП получателя',
                                            labelWidth: 120
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        readOnly: true,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ContragentPhone',
                                            itemId: 'tfPhone',
                                            fieldLabel: 'Контактный телефон',
                                            labelWidth: 190
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'SettlementAccount',
                                            itemId: 'tfSettlementAccount',
                                            fieldLabel: 'Расчетный счет',
                                            labelWidth: 120
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    itemId: 'sfContragentBank',
                                    name: 'ContragentBank',
                                    fieldLabel: 'Наименование банка',
                                    windowContainerSelector: '#requestTransferRfEditWindow',
                                    store: 'B4.store.contragent.Bank',
                                    disabled: true,
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        readOnly: true,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'CorrAccount',
                                            itemId: 'tfCorrAccount',
                                            fieldLabel: 'Кор. счет',
                                            labelWidth: 190
                                        },
                                        {
                                            name: 'Bik',
                                            itemId: 'tfBik',
                                            fieldLabel: 'БИК',
                                            labelWidth: 120
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ContractRf',
                                    itemId: 'sfContractRf',
                                    textProperty: 'DocumentNum',
                                    fieldLabel: 'Договор заключенный с ГИСУ',
                                    windowContainerSelector: '#requestTransferRfEditWindow',
                                    store: 'B4.store.ContractRfByManOrg',
                                    allowBlank: false,
                                    editable: false,
                                    disabled: true,
                                    columns: [
                                        { text: 'Номер', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Дата',
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'DocumentDate',
                                            flex: 1,
                                            filter: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                operand: CondExpr.operands.eq
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ProgramCr',
                                    itemId: 'sfProgramCr',
                                    textProperty: 'Name',
                                    fieldLabel: 'Программа капремонта',
                                    windowContainerSelector: '#requestTransferRfEditWindow',
                                    store: 'B4.store.dict.ProgramCr',
                                    allowBlank: false,
                                    editable: false,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            text: 'Наименование',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'TypeProgramCr',
                                            flex: 1,
                                            text: 'Тип',
                                            renderer: function (val) { return B4.enums.TypeProgramCr.displayRenderer(val); },
                                            filter: {
                                                xtype: 'b4combobox',
                                                items: B4.enums.TypeProgramCr.getItemsWithEmpty([null, '-']),
                                                editable: false,
                                                operand: CondExpr.operands.eq,
                                                valueField: 'Value',
                                                displayField: 'Display'
                                            }
                                        }]
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    editable: false,
                                    fieldLabel: 'Тип программы',
                                    store: B4.enums.TypeProgramRequest.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'TypeProgramRequest',
                                    itemId: 'cbxTypeProgramRequest'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Perfomer',
                                    itemId: 'tfPerformer',
                                    fieldLabel: 'Исполнитель',
                                    maxLength: 300,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    itemId: 'ffFile',
                                    fieldLabel: 'Файл'
                                }
                            ]
                        },
                        {
                            xtype: 'transferfundsrfgrid'
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
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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