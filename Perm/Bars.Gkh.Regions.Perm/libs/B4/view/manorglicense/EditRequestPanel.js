Ext.define('B4.view.manorglicense.EditRequestPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manOrgLicenseRequestEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Заявление на выдачу лицензии',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhDecimalField',
        'B4.store.manorglicense.ListManOrg',
        'B4.view.manorglicense.PersonGrid',
        'B4.view.manorglicense.ProvDocGrid',
        'B4.view.manorglicense.RequestAnnexEditWindow',
        'B4.view.manorglicense.RequestAnnexGrid',
        'B4.view.manorglicense.RequestInspectionGrid',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.LicenseRequestType',
        'B4.enums.TypeIdentityDocument'
    ],

    initComponent: function() {
        var me = this,
            itemsStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                fields: ['Text', 'ExtraParams'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'ManOrgLicenseRequest',
                    listAction: 'GetListRules'
                }
            });

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать проверку',
                                    iconCls: 'icon-accept',
                                    itemsStore: itemsStore,
                                    name: 'createInspection',
                                    menu: []
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
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
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
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 170,
                                        falign: 'stretch',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateRequest',
                                            fieldLabel: 'Дата заявления',
                                            format: 'd.m.Y',
                                            flex:1
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            hideTrigger: true,
                                            minValue: 0,
                                            name: 'RegisterNum',
                                            fieldLabel: 'Регистрационный номер',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            name: 'Type',
                                            allowBlank: false,
                                            fieldLabel: 'Тип заявления',
                                            flex: 1,
                                            items: B4.enums.LicenseRequestType.getItems(),
                                            readOnly: true,
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    name: 'TaxInfo',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 170,
                                        falign: 'stretch',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ConfirmationOfDuty',
                                            fieldLabel: 'Документ, подтверждающий уплату гос. пошлины',
                                            maxLength: 1000,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'TaxSum',
                                            fieldLabel: 'Сумма пошлины',
                                            margin: '5 0 0 5'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Файл заявления',
                                    margin: '5 0 0 0'
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'ApplicantInfo',
                                    hidden: true,
                                    defaults: {
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    title: 'Заявитель',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Applicant',
                                                    fieldLabel: 'Заявитель',
                                                    labelWidth: 190
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'LicenseInfo',
                                    hidden: true,
                                    defaults: {
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    title: 'Информация о лицензии',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200,
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicenseNum',
                                                    fieldLabel: 'Номер лицензии',
                                                    labelWidth: 190
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicenseState',
                                                    fieldLabel: 'Статус лицензии'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateIssued',
                                                    fieldLabel: 'Дата выдачи/регистрации',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'ProvDocFile',
                                                    fieldLabel: 'Файл лицензии',
                                                    onTrigger1Click: function () {
                                                    },
                                                    onTrigger3Click: function () {
                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'RevokeLicenseInfo',
                                    hidden: true,
                                    defaults: {
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    title: 'Информация о лицензии',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200,
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RevokeNumberLicense',
                                                    fieldLabel: 'Номер лицензии',
                                                    labelWidth: 190
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RevokeStateLicense',
                                                    fieldLabel: 'Статус лицензии'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                //{
                                //    xtype: 'b4selectfield',
                                //    name: 'LicenseRegistrationReason',
                                //    fieldLabel: 'Причины переоформления лицензии',
                                //    store: 'B4.store.dict.LicenseRegistrationReason',
                                //    editable: false,
                                //    hidden: true,
                                //    disabled: true,
                                //    columns: [
                                //        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                //    ]
                                //},
                                //{
                                //    xtype: 'fieldset',
                                //    name: 'RegisterLicenseReviewInfo',
                                //    hidden: true,
                                //    title: 'Рассмотрение заявления',
                                //    items: [
                                //        {
                                //            xtype: 'container',
                                //            padding: '0 0 5 0',
                                //            layout: {
                                //                type: 'hbox',
                                //                align: 'stretch'
                                //            },
                                //            defaults: {
                                //                labelAlign: 'right',
                                //                padding: '5 5',
                                //                labelWidth: 200,
                                //                flex: 1
                                //            },
                                //            items: [
                                //                {
                                //                    xtype: 'datefield',
                                //                    name: 'SendResultDate',
                                //                    fieldLabel: 'Дата отправки результата',
                                //                    format: 'd.m.Y'
                                //                },
                                //                {
                                //                    xtype: 'b4enumcombo',
                                //                    fieldLabel: 'Способ отправки',
                                //                    name: 'SendMethod',
                                //                    enumName: 'B4.enums.SendMethod'
                                //                }
                                //            ]
                                //        }
                                //    ]
                                //},
                                {
                                    xtype: 'fieldset',
                                    name: 'ReviewInfo',
                                    hidden: true,
                                    title: 'Рассмотрение заявления',
                                    items: [
                                        {
                                            xtype: 'container',
                                            name: 'Notice',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'NoticeAcceptanceDate',
                                                    fieldLabel: 'Дата уведомления о принятии документов к рассмотрению',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'NoticeViolationDate',
                                                    fieldLabel: 'Дата уведомления об устранении нарушений',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'ReviewDate',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ReviewDate',
                                                    fieldLabel: 'Дата рассмотрения документов',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'NoticeReturnDate',
                                                    fieldLabel: 'Дата уведомления о возврате документов',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'SendResultDate',
                                                    fieldLabel: 'Дата отправки результата',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'b4enumcombo',
                                                    fieldLabel: 'Способ отправки',
                                                    name: 'SendMethod',
                                                    enumName: 'B4.enums.SendMethod'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'ReviewInfoLk',
                                    hidden: true,
                                    title: 'Рассмотрение заявления ЛК',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ReviewDateLk',
                                                    fieldLabel: 'Дата рассмотрения документов ЛК',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'PreparationOfferDate',
                                                    fieldLabel: 'Дата подготовки мотивированного предложения',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'OrderInfo',
                                    hidden: true,
                                    title: 'Приказ о предоставлении / отказе в выдаче лицензии',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'OrderDate',
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OrderNumber',
                                                    fieldLabel: 'Номер'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                padding: '5 5',
                                                labelWidth: 200,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'OrderFile',
                                                    fieldLabel: 'Файл заявления'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'LicenseRejectReason',
                                    fieldLabel: 'Причина отказа',
                                    store: 'B4.store.dict.LicenseRejectReason',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Note',
                                    fieldLabel: 'Примечание',
                                    height: 50,
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'manorglicensepersongrid',
                                    name: 'PersonGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 200,
                                falign: 'stretch',
                                labelAlign: 'right'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Информация о заявителе',
                            name: 'ApplicantTab',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        falign: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            items: [
                                                {
                                                    xtype: 'button',
                                                    iconCls: 'icon-accept',
                                                    action: 'goToContragent',
                                                    text: 'Редактировать'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'component',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentName',
                                    fieldLabel: 'Полное наименование'
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentShortName',
                                    fieldLabel: 'Сокращенное наименование'
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentOrgForm',
                                    fieldLabel: 'Организационно-правовая форма'
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentJurAddress',
                                    fieldLabel: 'Юридический адрес'
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentFactAddress',
                                    fieldLabel: 'Фактический адрес'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        falign: 'stretch',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            readOnly: true,
                                            name: 'ContragentOgrn',
                                            fieldLabel: 'ОГРН'
                                        },
                                        {
                                            xtype: 'textfield',
                                            readOnly: true,
                                            name: 'ContragentInn',
                                            fieldLabel: 'ИНН'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    name: 'ContragentRegistration',
                                    fieldLabel: 'Орган, принявший решение о регистрации'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        falign: 'stretch',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            readOnly: true,
                                            name: 'ContragentPhone',
                                            fieldLabel: 'Телефон'
                                        },
                                        {
                                            xtype: 'textfield',
                                            readOnly: true,
                                            name: 'ContragentEmail',
                                            fieldLabel: 'E-mail'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    title: 'Свидетельство о постановке на учет в налоговом органе',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200,
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'TaxRegistrationSeries',
                                                    fieldLabel: 'Серия',
                                                    labelWidth: 190
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'TaxRegistrationNumber',
                                                    fieldLabel: 'Номер'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'TaxRegistrationDate',
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            labelAlign: 'right',
                                            name: 'TaxRegistrationIssuedBy',
                                            fieldLabel: 'Кем выдан',
                                            maxLength: 300,
                                            readOnly: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    title: 'Документ, удостоверяющий личность',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    name: 'TypeIdentityDocument',
                                                    fieldLabel: 'Тип документа',
                                                    displayField: 'Display',
                                                    store: B4.enums.TypeIdentityDocument.getStore(),
                                                    valueField: 'Value'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'IdIssuedDate',
                                                    fieldLabel: 'Дата выдачи',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: 'hbox',
                                            defaults:
                                            {
                                                xtype: 'textfield',
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 10
                                            },
                                            items: [
                                                {
                                                    name: 'IdSerial',
                                                    fieldLabel: 'Серия'
                                                },
                                                {
                                                    name: 'IdNumber',
                                                    fieldLabel: 'Номер'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'IdIssuedBy',
                                            fieldLabel: 'Кем выдан',
                                            maxLength: 2000
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'manorglicenseprovdocgrid',
                            flex: 1
                        },
                        {
                            xtype: 'manorglicenserequestannexgrid',
                            flex: 1
                        },
                        {
                            xtype: 'manorglicenserequestinspgrid',
                            flex: 1
                        }
                    ]
                }

            ]
        });

        me.callParent(arguments);
    }
});