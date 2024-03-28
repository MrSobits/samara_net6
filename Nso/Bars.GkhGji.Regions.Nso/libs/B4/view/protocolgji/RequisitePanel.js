Ext.define('B4.view.protocolgji.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolgjiRequisitePanel',
    
    requires: [
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.enums.TypeRepresentativePresence'
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
                            fieldLabel: 'Дата передачи документов',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            flex: 0.7
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
                                disabled: true,
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
                                    name: 'PersonPosition',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPosition',
                                    maxLength: 500,
                                    labelWidth: 130
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