Ext.define('B4.view.protocolgji.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolgjiRequisitePanel',
    
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.enums.TypeExecutantProtocolMvd',
        'B4.enums.TypeRepresentativePresence',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.enums.TypeAddress',
        'B4.enums.PlaceOffense',
        'B4.store.dict.JurInstitution'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            bodyStyle: Gkh.bodyStyle,
            title: 'Реквизиты',
            border: false,
            bodyPadding: 5,
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'protocolBaseName',
                            itemId: 'protocolBaseNameTextField',
                            fieldLabel: 'Документ-основание',
                            readOnly: true,
                            flex: 1
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ToCourt',
                            fieldLabel: 'Документы переданы в суд',
                            itemId: 'cbToCourt',
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'protocolInspectors',
                            itemId: 'trigfInspector',
                            fieldLabel: 'Инспекторы',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateToCourt',
                            fieldLabel: 'Дата направления материала в суд',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            flex: 0.7,
                            labelWidth: 220
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
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            textProperty: 'ShortName',
                            name: 'Contragent',
                            fieldLabel: 'Юридическое лицо',
                            itemId: 'sfContragent',
                            disabled: true,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    itemId: 'tfPhysPerson',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonPosition',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPosition',
                                    maxLength: 500,
                                    labelWidth: 130
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'BirthDay',
                                    fieldLabel: 'Дата рождения',
                                    itemId: 'dfBirthDay',
                                    format: 'd.m.Y',
                                    labelWidth: 160,
                                    width: 330
                                },
                            ],
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                flex: 0.4
                            },
                            items: [
                                {
                                    //тип адреса
                                    xtype: 'b4enumcombo',
                                    name: 'TypeAddress',
                                    fieldLabel: 'Тип адреса',
                                    itemId: 'ecTypeAddress',
                                    width: 450,
                                    minWidth: 450,
                                    enumName: B4.enums.TypeAddress
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'AddressPlace',
                                    fieldLabel: 'Место адреса',
                                    itemId: 'tfAddressPlace',
                                    width: 450,
                                    minWidth: 450,
                                    flex: 1
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 270,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    //правонарушение
                                    xtype: 'b4enumcombo',
                                    name: 'PlaceOffense',
                                    fieldLabel: 'Тип места совершения правонарушения',
                                    itemId: 'ecPlaceOffense',
                                    width: 550,
                                    minWidth: 550,
                                    enumName: B4.enums.PlaceOffense,
                                },
                                {
                                    //Адрес места правонаружения
                                    xtype: 'b4fiasselectaddress',
                                    flex: 1.5,
                                    labelAlign: 'right',
                                    name: 'FiasPlaceAddress',
                                    itemId: 'protocolFiasPlaceAddressField',
                                    fieldLabel: 'Адрес места правонарушения',
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    }
                                },
                            ]

                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                 {
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                     name: 'JudSector',
                                     itemId: 'sfJudSector',
                                    fieldLabel: 'Судебный участок',
                                    store: 'B4.store.dict.JurInstitution',
                                    readOnly: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    flex: 1
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
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
                            xtype: 'textfield',
                            fieldLabel: 'Дата и место рождения',
                            name: 'PersonBirthDatePlace',
                            itemId: 'tfDatePlaceOfBirth',
                            maxLength: 250,
                            disabled: true
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
                                labelWidth: 160,
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
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'surveySubjectRequirements',
                    itemId: 'trigfSurveySubjectRequirements',
                    fieldLabel: 'Перечень требований'
                },
                {
                    xtype: 'fieldset',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    title: 'Уведомление о времени и месте составления протокола',
                    items: [
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Вручено через канцелярию',
                            name: 'NotifDeliveredThroughOffice',
                            itemId: 'cbNotifDeliveredThroughOffice'
                        },
                        {
                            xtype: 'datefield',
                            name: 'FormatDate',
                            itemId: 'dfNotifDeliveryDate',
                            fieldLabel: 'Дата вручения (регистрации) уведомления',
                            labelWidth: 275,
                            disabled: true
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'nfNotifNum',
                            name: 'NotifNumber',
                            fieldLabel: 'Номер регистрации',
                            disabled: true,
                            hideTrigger: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Рассмотрение',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateOfProceedings',
                                    fieldLabel: 'Дата и время расмотрения дела:',
                                    format: 'd.m.Y',
                                    labelWidth: 160,
                                    width: 330
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'HourOfProceedings',
                                    margin: '0 0 0 10',
                                    fieldLabel: '',
                                    labelWidth: 25,
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
                                    name: 'MinuteOfProceedings',
                                    width: 45,
                                    maxValue: 59,
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ProceedingCopyNum',
                                    fieldLabel: 'Количество экземпляров',
                                    hideTrigger: true,
                                    flex: 1,
                                    labelWidth: 160,
                                    minValue: 0
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    flex: 1,
                                    name: 'ProceedingsPlace',
                                    labelWidth: 180,
                                    fieldLabel: 'Место рассмотрения дела',
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbFamilrefusal',
                                    name: 'Familrefusal',
                                    flex: 0.5,
                                    labelWidth: 180,
                                    fieldLabel: 'Отказ от ознакомления',
                                    allowBlank: true,
                                    flex: 1
                                    //editable: true
                                }
                            ]
                        }
               
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Remarks',
                    fieldLabel: 'Замечания со стороны нарушителя',
                    maxLength: 1000
                }
            ],
            viewConfig: {

               
            },
        });

        me.callParent(arguments);
    }
});