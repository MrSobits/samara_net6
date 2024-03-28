Ext.define('B4.view.manorglicense.EditLicensePanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manOrgLicenseEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Лицензия',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.view.manorglicense.LicenseDocGrid',
        'B4.view.manorglicense.LicensePersonGrid',
        'B4.enums.TypeManOrgTerminationLicense',
        'B4.enums.TypeIdentityDocument',
        'B4.store.manorglicense.ListManOrg',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function() {
        var me = this;

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
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    action: 'Delete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Дома в управлении',
                                    iconCls: 'icon-accept',
                                    action: 'goToManOrgContracts'
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
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: '0 0 5 0',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            labelAlign: 'right',
                                            hideTrigger: true,
                                            labelWidth: 200,
                                            widht: 375,
                                            maskRe: /\d/,
                                            regex: /^\d{0,6}$/,
                                            regexText: 'Максимальная длина номера - 6 цифр',
                                            name: 'LicNumber',
                                            fieldLabel: 'Номер лицензии',
                                            allowBlank: true
                                        },
                                        {
                                            xtype: 'component',
                                            width: 120
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: '0 0 5 0',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 200,
                                            widht: 250,
                                            name: 'DateIssued',
                                            allowBlank: false,
                                            fieldLabel: 'Дата выдачи',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'component',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateRegister',
                                            labelWidth: 200,
                                            widht: 350,
                                            allowBlank: false,
                                            fieldLabel: 'Дата внесения в реестр лицензий',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'component',
                                            width: 102
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: '0 0 5 0',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            labelAlign: 'right',
                                            labelWidth: 200,
                                            name: 'HousingInspection',
                                            fieldLabel: 'Наименование лицензирующего органа',
                                            store: 'B4.store.HousingInspection',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'component',
                                            width: 102
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Приказ о предоставлении / отказе в выдаче лицензии',
                                    anchor: '100%',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: '0 0 5 0',
                                        xtype: 'container',
                                        layout: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DisposalNumber',
                                                    allowBlank: false,
                                                    labelWidth: 190,
                                                    width: 365,
                                                    fieldLabel: 'Номер'
                                                },
                                                {
                                                    xtype: 'component',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateDisposal',
                                                    labelWidth: 100,
                                                    width: 275,
                                                    allowBlank: false,
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Прекращение действия лицензии',
                                    anchor: '100%',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: '0 0 5 0',
                                        xtype: 'container',
                                        layout: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    fieldLabel: 'Основание',
                                                    store: B4.enums.TypeManOrgTerminationLicense.getStore(),
                                                    displayField: 'Display',
                                                    allowBlank: false,
                                                    flex: 1,
                                                    valueField: 'Value',
                                                    name: 'TypeTermination'
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OrganizationMadeDecisionTermination',
                                                    fieldLabel: 'Организация принявшая решение',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentTermination',
                                                    fieldLabel: 'Документ',
                                                    flex: 1

                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumberTermination',
                                                    fieldLabel: 'Номер',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentDateTermination',
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateTermination',
                                                    fieldLabel: 'Дата вступления в силу',
                                                    format: 'd.m.Y',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                falign: 'stretch',
                                                labelAlign: 'right',
                                                labelWidth: 150
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'TerminationFile',
                                                    fieldLabel: 'Файл документа',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'component',
                                                    width: 90
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Файлы лицензии',
                                    anchor: '100%',
                                    items: [
                                        {
                                            xtype: 'manorglicensedocgrid',
                                            flex: 1
                                        }
                                    ]
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
                            title: 'Информация о лицензиате',
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
                                },
                                {
                                    xtype: 'licensepersongrid',
                                    flex: 1
                                }

                            ]
                        }
                    ]
                }]
        });

        me.callParent(arguments);
    }
});