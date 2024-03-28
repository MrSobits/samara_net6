Ext.define('B4.view.resolpros.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'resolProsEditPanel',
    title: 'Постановление прокуратуры',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.store.Contragent',
        'B4.store.dict.ProsecutorOffice',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.enums.TypeRepresentativePresence',
        'B4.view.resolpros.AnnexGrid',
        'B4.view.resolpros.ArticleLawGrid',
        'B4.view.resolpros.RealityObjectGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.resolpros.DefinitionGrid',
        'B4.enums.TypeDocumentGji'
    ],

    initComponent: function() {
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
                        labelWidth: 170,
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
                                    allowBlank: false,
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    readOnly: true,
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'UIN',
                                    fieldLabel: 'УИН',
                                    labelWidth: 50,
                                    width: 350,
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
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
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
                    itemId: 'resolprosTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [

                                {
                                    xtype: 'b4selectfield',
                                    store:   'B4.store.dict.ProsecutorOffice',
                                    name: 'ProsecutorOffice',
                                    labelWidth: 280,
                                    fieldLabel: 'Орган прокуратуры, вынесший постановление',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } } ],
                                    itemId: 'sfMunicipalityResolPros'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        disabled: false,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'IssuedByFio',
                                            fieldLabel: 'ФИО прокурора',
                                            itemId: 'tfIssuedByFio',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'IssuedByPosition',
                                            fieldLabel: 'Должность',
                                            itemId: 'tfIssuedByPosition',
                                            maxLength: 500,
                                            labelWidth: 80
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'IssuedByRank',
                                            fieldLabel: 'Звание',
                                            itemId: 'tfIssuedByRank',
                                            maxLength: 500,
                                            labelWidth: 80
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '10 0 5 0',
                                    border: false,
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 130,
                                            name: 'DateSupply',
                                            fieldLabel: 'Дата поступления',
                                            format: 'd.m.Y',
                                            itemId: 'dfDateSupplyResolPros',
                                            maxWidth: 400
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ActCheck',
                                            itemId: 'actCheckSelectField',
                                            fieldLabel: 'Акт проверки',
                                            labelWidth: 150,
                                            isGetOnlyIdProperty: false,
                                            editable: false,
                                            textProperty: 'DocumentNumber',
                                            store: 'B4.store.DocumentGji',
                                            columns: [
                                                { xtype: 'datecolumn', dataIndex: 'DocumentDate', text: 'Дата', format: 'd.m.Y', width: 100 },
                                                { text: 'Номер', dataIndex: 'DocumentNumber', flex: 1 },
                                                {
                                                    text: 'Тип документа',
                                                    dataIndex: 'TypeDocumentGji',
                                                    flex: 1,
                                                    renderer: function(val) { return B4.enums.TypeDocumentGji.displayRenderer(val); }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right'
                                    },
                                    title: 'Постановление вынесено в отношении',
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
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'ShortName',
                                            name: 'Contragent',
                                            fieldLabel: 'Контрагент',
                                            itemId: 'sfContragent',
                                            disabled: true,
                                            editable: false,
                                            columns: [
                                                {
                                                    header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'Физическое лицо',
                                                    itemId: 'tfPhysPerson',
                                                    maxLength: 300,
                                                    labelWidth: 120
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PhysicalPersonInfo',
                                                    fieldLabel: 'Реквизиты физ. лица',
                                                    itemId: 'taPhysPersonInfo',
                                                    maxLength: 500,
                                                    labelWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '10 0 0 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
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
                                                    name: 'PhysicalPersonPosition',
                                                    fieldLabel: 'Должность',
                                                    itemId: 'tfdPhysicalPersonPosition',
                                                    maxLength: 255
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '5 0 0 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
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
                                                    itemId: 'tfPhysicalPersonDocumentSerial',
                                                    fieldLabel: 'Серия документа ФЛ',
                                                    labelWidth: 160,
                                                    allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPersonDocumentNumber',
                                                    itemId: 'tfPhysicalPersonDocumentNumber',
                                                    fieldLabel: 'Номер документа ФЛ',
                                                    allowBlank: true,
                                                    labelWidth: 160,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    itemId: 'dfPhysicalPersonIsNotRF',
                                                    name: 'PhysicalPersonIsNotRF',
                                                    labelWidth: 180,
                                                    fieldLabel: 'Не является гражданином РФ',
                                                    allowBlank: true,
                                                    flex: 1
                                                    //editable: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '5 0 0 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
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
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: 'hbox',
                                    title: 'Дата и время рассмотрения',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 120,
                                            name: 'DateResolPros',
                                            labelAlign: 'right',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y',
                                            itemId: 'dfDateResolPros',
                                            maxWidth: 400
                                        },
                                        {
                                            xtype: 'label',
                                            text: ' в ',
                                            margin: '5'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'FormatHour',
                                            margin: '0 0 0 10',
                                            fieldLabel: '',
                                            width: 45,
                                            maxValue: 23,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'label',
                                            text: ':',
                                            margin: '5'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'FormatMinute',
                                            width: 45,
                                            maxValue: 59,
                                            minValue: 0
                                        },
                                    ]
                                }
                            ]
                        },
                        { xtype: 'resolprosArticleLawGrid', flex: 1 },
                        { xtype: 'resolprosRealityObjectGrid', flex: 1 },
                        { xtype: 'resolprosDefinitionGrid', flex: 1 },
                        { xtype: 'resolprosAnnexGrid', flex: 1 }
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
                                //ToDo ГЖИ после перехода на правила необходимо удалить
                                /*
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Сформировать',
                                    itemId: 'btnCreateDocument',
                                    menu: [
                                        {
                                            text: 'Постановление',
                                            textAlign: 'left',
                                            itemId: 'btnCreateResolProsToResolution',
                                            actionName: 'createResolProsToResolution'
                                        }
                                    ]
                                }*/
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                }
                                /*, В постановлении прокуратуры неможет быть кнопки Удалить потмоу что оудаление произходит из реестра
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                }*/
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});