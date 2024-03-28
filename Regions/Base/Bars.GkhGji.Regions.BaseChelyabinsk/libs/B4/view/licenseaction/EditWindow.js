Ext.define('B4.view.licenseaction.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Contragent',
        'B4.view.licenseaction.FileInfoGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'licenseactionEditWindow',
    title: 'Запрос реестр лицензий',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [{
                xtype: 'tabpanel',
                border: false,
                flex: 1,
                defaults: {
                    border: false
                },
                items: [{
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 100,
                            margin: '5 0 5 0',
                            align: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Форма запроса',
                        border: false,
                        bodyPadding: 10,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0 0 5 0',
                                labelWidth: 110,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'LicenseActionType',
                                    fieldLabel: 'Тип запроса',
                                    displayField: 'Display',
                                    itemId: 'dfLicenseActionType',
                                    flex: 1,
                                    store: B4.enums.LicenseActionType.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                    //hidden: true
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Заявитель',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantType',
                                            fieldLabel: 'Тип',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantType'
                                        },
                                        //{
                                        //    xtype: 'textfield',
                                        //    name: 'ApplicantSnils',
                                        //    fieldLabel: 'СНИЛС',
                                        //    allowBlank: false,
                                        //    flex: 1,
                                        //    disabled: false,
                                        //    editable: true,
                                        //    itemId: 'dfApplicantSnils'
                                        //},
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantInn',
                                            fieldLabel: 'ИНН',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantInn'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantEmail',
                                            fieldLabel: 'E-mail',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantEmail'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantPhone',
                                            fieldLabel: 'Телефон',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantPhone'
                                        },
                                        ,
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantOkved',
                                            fieldLabel: 'ОКВЭД',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantOkved'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantLastName',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFamilyName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantFirstName',
                                            fieldLabel: 'Имя',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFirstName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantMiddleName',
                                            fieldLabel: 'Отчество',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfPatronymic'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ApplicantAgreement',
                                            fieldLabel: 'Отметка о соглашении',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantAgreement'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TypeAnswer',
                                            fieldLabel: 'Форма ответа',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfTypeAnswer'
                                        }
                                    ]
                                }
                                ]
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            textProperty: 'ShortName',
                            name: 'Contragent',
                            anchor: '100%',
                            fieldLabel: 'Контрагент',
                            labelWidth: 110,
                            itemId: 'sfContragent',
                            editable: false,
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
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Информация',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentType',
                                            fieldLabel: 'Тип документа',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentType'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentName',
                                            fieldLabel: 'Наименование',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentName'
                                        },
                                        ,
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentIssuer',
                                            fieldLabel: 'Кем выдан',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentIssuer'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentSeries',
                                            fieldLabel: 'Серия',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentSeries'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentNumber'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            fieldLabel: 'Дата выдачи',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentDate'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'SurnameFl',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfSurnameFl'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameFl',
                                            fieldLabel: 'Имя',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfNameFl'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'MiddleNameFl',
                                            fieldLabel: 'Отчество',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfMiddleNameFl'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Address',
                                            fieldLabel: 'Почтовый адрес',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfAddress'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Position',
                                            fieldLabel: 'Должность',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfPosition'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Лицензия',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'LicenseDate',
                                            fieldLabel: 'Дата лицензии',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfLicenseDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LicenseNumber',
                                            fieldLabel: 'Номер лицензии',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfLicenseNumber'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            flex: 1,
                            defaults: {
                                border: false
                            },
                            items: [{
                                xtype: 'licenseactionfileinfogrid',
                                flex: 1
                            }]
                        }]
                    },
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 170,
                            falign: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Ответ',
                        border: false,
                        bodyPadding: 10,
                        items: [
                            {
                                xtype: 'textarea',
                                name: 'DeclineReason',
                                fieldLabel: 'Причина отлонения',
                                maxLength: 4000
                            },
                            //{
                            //    xtype: 'fieldset',
                            //    defaults: {
                            //        labelWidth: 250,
                            //        anchor: '100%',
                            //        labelAlign: 'right'
                            //    },
                            //    title: 'Сведения о дисквалификации',
                            //    items: [
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                //     margin: '10 0 5 0',
                            //                labelWidth: 120,
                            //                labelAlign: 'right',
                            //            },
                            //        items: [
                            //            {
                            //                xtype: 'datefield',
                            //                name: 'FormDate',
                            //                fieldLabel: 'Дата формирования',
                            //                format: 'd.m.Y',
                            //                flex: 1,
                            //                allowBlank: true,
                            //                readOnly: true
                            //            },
                            //            {
                            //                xtype: 'datefield',
                            //                name: 'EndDisqDate',
                            //                fieldLabel: 'Дата окончания',
                            //                format: 'd.m.Y',
                            //                flex: 1,
                            //                allowBlank: true,
                            //                readOnly: true
                            //            },
                            //            {
                            //                xtype: 'textfield',
                            //                name: 'RegNumber',
                            //                fieldLabel: 'Номер записи',
                            //                allowBlank: true,
                            //                flex: 1,
                            //                disabled: false,
                            //                editable: false,
                            //                readOnly: true
                            //            }
                            //            ]
                            //        },
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                margin: '10 0 5 0',
                            //                labelWidth: 100,
                            //                labelAlign: 'right'
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'label',
                            //                    name: 'label',
                            //                    text: 'Срок дисквалификации:',
                            //                    labelWidth: 120
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'DisqDays',
                            //                    fieldLabel: 'дней',
                            //                    allowBlank: true,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    flex: 1,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'DisqMonths',
                            //                    fieldLabel: 'месяцев',
                            //                    allowBlank: true,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    flex: 1,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'DisqYears',
                            //                    fieldLabel: 'лет',
                            //                    allowBlank: true,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    flex: 1,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        },
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                margin: '5 0 5 0',
                            //                labelWidth: 120,
                            //                labelAlign: 'right',
                            //                flex: 1
                            //            },
                            //            items: [{
                            //                xtype: 'textfield',
                            //                name: 'Article',
                            //                fieldLabel: 'Статья КоАП',
                            //                allowBlank: true,
                            //                disabled: false,
                            //                flex: 1,
                            //                editable: false,
                            //                readOnly: true
                            //            }]
                            //        },
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                margin: '5 0 5 0',
                            //                labelWidth: 120,
                            //                labelAlign: 'right',
                            //                flex: 1
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'LawName',
                            //                    fieldLabel: 'Наименование суда',
                            //                    allowBlank: true,
                            //                    //   flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        },
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                margin: '5 0 5 0',
                            //                labelWidth: 120,
                            //                labelAlign: 'right',
                            //                flex: 1
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'datefield',
                            //                    name: 'LawDate',
                            //                    fieldLabel: 'Дата постановления',
                            //                    format: 'd.m.Y',
                            //                    allowBlank: true,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'CaseNumber',
                            //                    fieldLabel: 'Номер Дела',
                            //                    allowBlank: true,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        }
                            //    ]
                            //}
                        ]
                    }
                ]
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
                                xtype: 'b4savebutton'
                            }
                        ]
                    },
                    {
                        xtype: 'tbfill'
                    },
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
                            xtype: 'b4closebutton'
                        }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});