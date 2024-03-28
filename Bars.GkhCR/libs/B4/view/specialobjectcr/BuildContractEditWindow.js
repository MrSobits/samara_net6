Ext.define('B4.view.specialobjectcr.BuildContractEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 800,
    height: 740,
    alias: 'widget.specialobjectcrbuildcontracteditwindow',
    title: 'Договор подряда',
    autoScroll: true,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.store.Contragent',
        'B4.store.dict.Inspector',
        'B4.store.specialobjectcr.BuildContractBuilder',
        'B4.store.dict.TerminationReason',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeContractBuild',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.YesNo',
        'B4.enums.BuildContractState'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Договор подряда',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                            {
                                xtype: 'b4enumcombo',
                                enumName: 'B4.enums.BuildContractState',
                                name: 'BuildContractState',
                                fieldLabel: 'Состояние договора'
                            },
                            {
                                xtype: 'textfield',
                                name: 'DocumentName',
                                fieldLabel: 'Документ',
                                allowBlank: false,
                                maxLength: 300
                            },
                            {
                                xtype: 'container',
                                padding: '0 0 5 0',
                                defaults: {
                                    labelWidth: 190,
                                    flex: 1,
                                    labelAlign: 'right'
                                },
                                layout: {
                                    type: 'hbox'
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'DocumentNum',
                                        fieldLabel: 'Номер',
                                        maxLength: 50
                                    },
                                    {
                                        xtype: 'datefield',
                                        name: 'DocumentDateFrom',
                                        fieldLabel: 'от',
                                        format: 'd.m.Y'
                                    }
                                ]
                            },
                            {
                                xtype: 'b4filefield',
                                editable: false,
                                name: 'DocumentFile',
                                fieldLabel: 'Файл'
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'Contragent',
                                fieldLabel: 'Заказчик',
                                store: 'B4.store.Contragent',
                                editable: false,
                                allowBlank: false,
                                columns: [
                                    { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                    {
                                        text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
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
                                    { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                ]
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'Builder',
                                fieldLabel: 'Подрядная организация',
                                store: 'B4.store.specialobjectcr.BuildContractBuilder',
                                textProperty: 'ContragentName',
                                columns: [
                                    { text: 'Наименование', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
                                ],
                                editable: false,
                                allowBlank: false
                            },
                            {
                                xtype: 'container',
                                padding: '0 0 5 0',
                                layout: {
                                    type: 'hbox'
                                },
                                defaults: {
                                    labelWidth: 190,
                                    flex: 1,
                                    labelAlign: 'right'
                                },
                                items: [
                                    {
                                        xtype: 'combobox',
                                        editable: false,
                                        fieldLabel: 'Тип договора',
                                        store: B4.enums.TypeContractBuild.getStore(),
                                        displayField: 'Display',
                                        valueField: 'Value',
                                        name: 'TypeContractBuild',
                                        listConfig: {
                                            listeners: {
                                                refresh: function(picker) {
                                                    var cb = picker.pickerField;
                                                    cb.fireEvent('refresh', cb);
                                                }
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'gkhdecimalfield',
                                        name: 'Sum',
                                        fieldLabel: 'Сумма договора (руб.)'
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox'
                                },
                                defaults: {
                                    labelWidth: 190,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'BudgetMo',
                                        fieldLabel: 'Бюджет МО',
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'BudgetSubject',
                                        fieldLabel: 'Бюджет субъекта',
                                        maxLength: 300
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                padding: '5 0 5 0',
                                layout: {
                                    type: 'hbox'
                                },
                                defaults: {
                                    labelWidth: 190,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'OwnerMeans',
                                        fieldLabel: 'Средства собственников',
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'FundMeans',
                                        fieldLabel: 'Средства фонда',
                                        maxLength: 300
                                    }
                                ]
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'Inspector',
                                fieldLabel: 'Инспектор',

                                store: 'B4.store.dict.Inspector',
                                textProperty: 'Fio',
                                editable: false,
                                columns: [
                                    { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
                                ]
                            },
                            {
                                xtype: 'container',
                                layout: {
                                    type: 'anchor'
                                },
                                items: [
                                    {
                                        xtype: 'container',
                                        padding: '0 0 5 0',
                                        anchor: '100%',
                                        layout: {
                                            type: 'hbox'
                                        },
                                        defaults: {
                                            labelWidth: 190,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'datefield',
                                                name: 'DateStartWork',
                                                fieldLabel: 'Дата начала работ',
                                                format: 'd.m.Y'
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'DateEndWork',
                                                fieldLabel: 'Дата окончания работ',
                                                format: 'd.m.Y'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        padding: '0 0 5 0',
                                        anchor: '100%',
                                        layout: {
                                            type: 'hbox'
                                        },
                                        defaults: {
                                            labelWidth: 190,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'datefield',
                                                name: 'DateInGjiRegister',
                                                fieldLabel: 'Договор внесен в реестр ГЖИ',
                                                format: 'd.m.Y'
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'GuaranteePeriod',
                                                fieldLabel: 'Гарантийный срок (лет)',
                                                maskRe: /\d/i
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        padding: '0 0 5 0',
                                        anchor: '100%',
                                        layout: {
                                            type: 'hbox'
                                        },
                                        defaults: {
                                            labelWidth: 190,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'datefield',
                                                name: 'DateAcceptOnReg',
                                                fieldLabel: 'Принято на регистрацию, но еще не зарегистрировано',
                                                format: 'd.m.Y',
                                                labelWidth: 190,
                                                labelAlign: 'right',
                                                anchor: '50%'
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'DateCancelReg',
                                                fieldLabel: 'Отклонено от регистрации',
                                                format: 'd.m.Y'
                                            }
                                        ]
                                    },
                                ]
                            },
                            {
                                xtype: 'textarea',
                                name: 'Description',
                                fieldLabel: 'Описание',
                                maxLength: 500,
                                minHeight: 32,
                                flex: 1
                            },
                            {
                                xtype: 'combobox',
                                editable: false,
                                fieldLabel: 'Выводить документ на портал',
                                name: 'UsedInExport',
                                store: B4.enums.YesNo.getStore(),
                                displayField: 'Display',
                                valueField: 'Value'
                            },
                            {
                                xtype: 'textfield',
                                name: 'UrlResultTrading',
                                fieldLabel: 'Ссылка на результаты проведения торгов',
                                maxLength: 250,
                            },
                            {
                                xtype: 'contracttypeworkspecialcrgrid',
                                height: 200
                            }
                            ]
                        },
                        {
                            name: 'ResultQual',
                            layout: 'anchor',
                            title: 'Результат квалификационного отбора',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            defaults: {
                                labelWidth: 230,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ProtocolName',
                                    fieldLabel: 'Протокол квалификационного отбора',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelWidth: 230,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ProtocolNum',
                                            fieldLabel: 'Номер протокола',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ProtocolDateFrom',
                                            fieldLabel: 'от',
                                            format: 'd.m.Y',
                                            labelWidth: 50,
                                            maxWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'ProtocolFile',
                                    editable: false,
                                    fieldLabel: 'Файл'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    enumName: 'B4.enums.YesNo',
                                    name: 'IsLawProvided',
                                    fieldLabel: 'Проведение отбора предусмотрено законодательством'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'WebSite',
                                    manualDisabled: true,
                                    allowBlank: false,
                                    fieldLabel: 'Адрес сайта с информацией об отборе'
                                }
                            ]
                        },
                        {
                            title: 'Расторжение договора',
                            name: 'TerminationContractTab',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            hidden: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                allowBlank: true,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'TerminationDate',
                                            fieldLabel: 'Дата расторжения',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TerminationDocumentNumber',
                                            fieldLabel: 'Номер документа'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'TerminationDocumentFile',
                                    fieldLabel: 'Документ-основание',
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'TerminationDictReason',
                                    fieldLabel: 'Причина расторжения',
                                    store: 'B4.store.dict.TerminationReason',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'TerminationReason',
                                    fieldLabel: 'Основание расторжения',
                                    maxLength: 250
                                },
                            ]
                        },
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
                            items: [
                                { xtype: 'b4savebutton' },
                                { xtype: 'gkhbuttonprint' }
                            ]
                        },
                        { xtype: 'tbfill' },
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