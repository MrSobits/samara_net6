Ext.define('B4.view.appealcits.AdmonitionEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.admonitioneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    //minHeight: 510,
    //height: 710,
    bodyPadding: 5,
    itemId: 'admonitioneditwindow',
    title: 'Форма просмотра предостережения',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.enums.KindKND',
        'B4.store.appealcits.AppCitAdmonVoilation',
        'B4.view.appealcits.AdmonVoilationGrid',
        'B4.store.appealcits.AppCitAdmonAppeal',
        'B4.store.appealcits.AppCitAdmonAnnex',
        'B4.view.appealcits.AdmonAnnexGrid',
        'B4.view.appealcits.AppCitAdmonAppealGrid',
        'B4.store.dict.InspectionReasonERKNM',
        'B4.view.Control.GkhButtonPrint',
        'B4.store.dict.Inspector',
        'B4.enums.IdentifierType',
        'B4.enums.RiskCategory',
        'B4.store.dict.SurveySubject',
        'B4.enums.PayerType',
        'B4.store.dict.PhysicalPersonDocType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0 0 5 0',
                        labelWidth: 150,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'PayerType',
                            fieldLabel: 'Вид исполнителя',
                            labelWidth: 170,
                            displayField: 'Display',
                            itemId: 'dfPayerType',
                            flex: 1,
                            store: B4.enums.PayerType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'ERKNMID',
                            itemId: 'dfERKNMID',
                            fieldLabel: 'Номер в ЕРКНМ',
                            allowBlank: true,
                            flex: 1,
                            readOnly: true,
                            maxLength: 20
                        },
                        {
                            xtype: 'button',
                            text: 'Разместить в ЕРКНМ',
                            name: 'calculateButton',
                            tooltip: 'Разместить в ЕРКНМ',
                            action: 'ERKNMRequest',
                            iconCls: 'icon-accept',
                            itemId: 'calculateButton'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsUrParams',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты исполнителя - Юр.лицо',
                    hidden: true,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                /*margin: '10 0 5 0',*/
                                labelWidth: 160,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    flex: 1,
                                    name: 'Contragent',
                                    itemId: 'contragent',
                                    fieldLabel: 'Контрагент',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    allowBlank: false
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsFizParams',
                    defaults: {
                        labelWidth: 160,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты исполнителя - физ.лицо',
                    hidden: true,
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'PhysicalPersonDocType',
                            fieldLabel: 'Вид документа',
                            store: 'B4.store.dict.FLDocType',
                            allowBlank: false,
                            editable: false,
                            flex: 1,
                            itemId: 'dfPhysicalPersonDocType',
                            columns: [
                                { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                /*margin: '10 0 5 0',*/
                                labelWidth: 160,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FIO',
                                    itemId: 'dfFIO',
                                    fieldLabel: 'ФИО',
                                    allowBlank: false,
                                    disabled: false,
                                    flex: 1,
                                    editable: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentSerial',
                                    itemId: 'dfDocumentSerial',
                                    fieldLabel: 'Серия документа',
                                    labelWidth: 120,
                                    allowBlank: false,
                                    disabled: false,
                                    flex: 0.5,
                                    editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumberFiz',
                                    itemId: 'dfDocumentNumberFiz',
                                    labelWidth: 120,
                                    fieldLabel: 'Номер документа',
                                    allowBlank: false,
                                    disabled: false,
                                    flex: 0.5,
                                    editable: true,
                                    maxLength: 20
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                padding: '5 0 5 0',
                                labelWidth: 160,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FizINN',
                                    itemId: 'dfFizINN',
                                    fieldLabel: 'ИНН',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FizAddress',
                                    labelWidth: 120,
                                    itemId: 'tfFizAddress',
                                    fieldLabel: 'Адрес',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: true
                                },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            fieldLabel: 'Документ',
                            maxLength: 300
                        },
                        {
                            xtype: 'b4enumcombo',
                            enumName: 'B4.enums.KindKND',
                            labelWidth: 150,
                            name: 'KindKND',
                            fieldLabel: 'Вид контроля/надзора',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            enumName: 'B4.enums.RiskCategory',
                            labelWidth: 170,
                            name: 'RiskCategory',
                            fieldLabel: 'Категория риска'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'SurveySubject',
                            fieldLabel: 'Предмет ПМ',
                            store: 'B4.store.dict.SurveySubject',
                            allowBlank: true,
                            editable: false,
                            labelWidth: 150,
                            flex: 1,
                            itemId: 'sfSurveySubject',
                            columns: [
                                { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            allowBlank: false,
                            format: 'd.m.Y',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'PerfomanceDate',
                            fieldLabel: 'Срок исполнения',
                            allowBlank: false,
                            labelWidth: 170
                        },
                        {
                            name: 'PerfomanceFactDate',
                            fieldLabel: 'Дата факт. исполнения',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.Inspector',
                            name: 'Inspector',
                            flex: 1,
                            editable: true,
                            fieldLabel: 'Вынес предостережение',
                            textProperty: 'Fio',
                            allowBlank: false,
                            isGetOnlyIdProperty: true,
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.Inspector',
                            name: 'Executor',
                            editable: true,
                            fieldLabel: 'Инспектор',
                            flex: 1,
                            labelWidth: 150,
                            textProperty: 'Fio',
                            allowBlank: false,
                            isGetOnlyIdProperty: true,
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.InspectionReasonERKNM',
                    padding: '5 0 5 0',
                    textProperty: 'Name',
                    itemId: 'sfInspectionReasonERKNM',
                    editable: false,
                    columns: [
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 0.5,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'InspectionReasonERKNM',
                    fieldLabel: 'Основание ПМ',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.RealityObject',
                    padding: '0 0 5 0',
                    textProperty: 'Address',
                    editable: false,
                    columns: [
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
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'RealityObject',
                    fieldLabel: 'Жилой дом',
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'AnswerFile',
                    fieldLabel: 'Файл ответа',
                    editable: true

                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Нарушенные требования',
                            flex: 1,
                            height: 50,
                            maxHeight: 50,
                          //  width: 350,
                            name: 'Violations'
                        },
                        {
                            xtype: 'textarea',
                            labelWidth: 80,
                            fieldLabel: 'Предлагаю',
                            flex: 1,
                            height: 50,
                            maxHeight: 50,
                          //  width: 350,
                            name: 'Measures'
                        },
                    ]
                },
               
                {
                    xtype: 'tabpanel',

                    layout: { type: 'vbox', align: 'stretch' },
                    autoScroll: true,
                    border: false,
                    flex: 1,
                    items: [

                        {
                            xtype: 'admonVoilationGrid',
                            flex: 1
                        },
                        {
                            xtype: 'appCitAdmonAppealGrid',
                            flex: 1
                        },
                        {
                            xtype: 'admonAnnexGrid',
                            flex: 1
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
                             xtype: 'buttongroup',
                             items: [
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