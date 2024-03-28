Ext.define('B4.view.protocolgji.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolgjiRequisitePanel',
    
    requires: [
        'B4.store.Contragent',
        'B4.store.dict.Citizenship',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.form.FiasSelectAddress',
        'B4.view.Control.GkhTriggerField',
        'B4.enums.CitizenshipType'
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
                labelWidth: 150,
                labelAlign: 'right'
            },
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'protocolBaseName',
                            itemId: 'protocolBaseNameTextField',
                            fieldLabel: 'Документ-основание',
                            readOnly: true,
                            labelWidth: 130,
                            flex: 1
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ToCourt',
                            fieldLabel: 'Документы переданы в суд',
                            itemId: 'cbToCourt',
                            labelWidth: 170,
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'protocolInspectors',
                            itemId: 'trigfInspector',
                            fieldLabel: 'Инспектор',
                            allowBlank: false,
                            labelWidth: 130,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateToCourt',
                            fieldLabel: 'Дата передачи документов',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            labelWidth: 170,
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    labelWidth: 130,
                    itemId: 'taDescriptionProtocol',
                    maxLength: 2000
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 120
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
                                        labelWidth: 130
                                    },
                                    itemId: 'fsReceiverInfo',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Surname',
                                            fieldLabel: 'Фамилия',
                                            itemId: 'surname'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            fieldLabel: 'Имя',
                                            itemId: 'name'
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
                                            name: 'FactAddress',
                                            fieldLabel: 'Фактический адрес проживания',
                                            itemId: 'factAddress'
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
                                        labelWidth: 130
                                    },
                                    itemId: 'fsReceiverReq',
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
                            labelAlign: 'right',
                            itemId: 'physicalPersonInfo'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    shrinkWrap: true,
                    title: 'Уведомление о времени и месте составления протокола',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Вручено через канцелярию',
                                    name: 'NotifDeliveredThroughOffice',
                                    itemId: 'cbNotifDeliveredThroughOffice',
                                    listeners: {
                                        'change': function(component, value) {
                                            var fieldset = component.up('fieldset'),
                                                formatDateField = fieldset.down('datefield[name=FormatDate]'),
                                                notifNumberField = fieldset.down('textfield[name=NotifNumber]');

                                            formatDateField.setDisabled(!value);
                                            notifNumberField.setDisabled(!value);
                                        }
                                    }
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
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'DocumentPlace',
                                    fieldLabel: 'Место составления протокола',
                                    labelWidth: 160,
                                    labelAlign: 'right',
                                    flatIsVisible: false,
                                    allowBlank: false,
                                    padding: '0 5 0 0',
                                    flex: 1,
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
                                }
                            ]
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
                            xtype: 'textfield',
                            name: 'ProceedingsPlace',
                            fieldLabel: 'Место рассмотрения дела',
                            maxLength: 1000
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Remarks',
                    fieldLabel: 'Замечания со стороны нарушителя',
                    maxLength: 1000
                }
            ]
        });

        me.callParent(arguments);
    }
});