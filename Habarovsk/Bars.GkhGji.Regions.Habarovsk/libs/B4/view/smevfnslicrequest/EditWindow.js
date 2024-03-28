Ext.define('B4.view.smevfnslicrequest.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevfnslicrequest.FileInfoGrid',
        'B4.store.smev.SMEVFNSLicRequestFile',
        'B4.enums.TypeLicenseRequest',
        'B4.enums.TypeDocumentGji',
        'B4.enums.RequestState',
        'B4.enums.FNSLicRequestType',
        'B4.enums.PayerType',
        'B4.enums.FNSLicPersonType',
        'B4.store.manorglicense.License',
        'B4.enums.FNSLicDecisionType'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    height: 800,
    bodyPadding: 10,
    itemId: 'smevfnslicrequestEditWindow',
    title: 'Обмен данными с ФНС',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                                labelWidth: 150,
                                margin: '5 0 5 0',
                                align: 'stretch',
                                labelAlign: 'right'
                            },
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Отправка информации',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '0 0 5 0',
                                        labelWidth: 80,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'IdDoc',
                                            fieldLabel: 'ID документа',
                                            labelWidth: 130,
                                            allowBlank: true,
                                            itemId: 'dfIdDoc',
                                            disabled: false,
                                            flex: 1,
                                            readOnly: true
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '0 0 5 0',
                                        labelWidth: 80,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'button',
                                            text: 'Отправить',
                                            tooltip: 'Отправить',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'send'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Проверить ответ',
                                            tooltip: 'Проверить ответ',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'checkAns'
                                        }
                                        
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        margin: '0 0 5 0',
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            name: 'FNSLicRequestType',
                                            fieldLabel: 'Тип запроса',
                                            displayField: 'Display',
                                            itemId: 'dfFNSLicRequestType',
                                            flex: 1,
                                            store: B4.enums.FNSLicRequestType.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            editable: false
                                            //hidden: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DeleteIdDoc',
                                    fieldLabel: 'ID документа для исключения из ФНС',
                                    labelWidth: 130,
                                    allowBlank: true,
                                    itemId: 'dfDeleteIdDoc',
                                    disabled: false,
                                    flex: 1,
                                    editable: true,
                                    allowBlank: false,
                                    hidden: true
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        margin: '0 0 5 0',
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            name: 'FNSLicPersonType',
                                            fieldLabel: 'Вид лицензиата',
                                            displayField: 'Display',
                                            itemId: 'dfFNSLicPersonType',
                                            flex: 1,
                                            store: B4.enums.FNSLicPersonType.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            editable: false,
                                            hidden: true
                                            
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
                                    title: 'Информация о лицензиате',
                                    hidden: true,
                                    items: [  
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            itemId: 'dfULName',
                                            hidden: true,
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'NameUL',
                                                    fieldLabel: 'Наименование ЮЛ',
                                                    allowBlank: true,
                                                    itemId: 'dfNameUL',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            itemId: 'dfIPName',
                                            hidden: true,
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FirstName',
                                                    fieldLabel: 'Имя',
                                                    allowBlank: true,
                                                    itemId: 'dfName',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FamilyName',
                                                    fieldLabel: 'Фамилия',
                                                    allowBlank: true,
                                                    itemId: 'dfFamily',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OGRN',
                                                    itemId: 'dfOGRN',
                                                    fieldLabel: 'ОГРН',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                    //maxLength: 13,
                                                    //regex: /^(\d{13})$/,
                                                    //regexText: '13 цифр'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'INN',
                                                    itemId: 'dfINN',
                                                    fieldLabel: 'ИНН',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                    //maxLength: 10,
                                                    //regex: /^(\d{10})$/,
                                                    //regexText: '10 цифр'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                //{
                                //    xtype: 'fieldset',
                                //    itemId: 'fsIpParams',
                                //    defaults: {
                                //        labelWidth: 250,
                                //        anchor: '100%',
                                //        labelAlign: 'right'
                                //    },
                                //    title: 'Информация - ИП',
                                //    hidden: true,
                                //    items: [
                                //        {
                                //            xtype: 'container',
                                //            layout: 'hbox',
                                //            defaults: {
                                //                xtype: 'combobox',
                                //                //     margin: '10 0 5 0',
                                //                labelWidth: 80,
                                //                labelAlign: 'right',
                                //            },
                                //            items: [
                                //                {
                                //                    xtype: 'textfield',
                                //                    name: 'FirstName',
                                //                    fieldLabel: 'Имя',
                                //                    allowBlank: true,
                                //                    itemId: 'dfName',
                                //                    disabled: false,
                                //                    flex: 1,
                                //                    editable: true
                                //                },
                                //                {
                                //                    xtype: 'textfield',
                                //                    name: 'FamilyName',
                                //                    fieldLabel: 'Фамилия',
                                //                    allowBlank: true,
                                //                    itemId: 'dfFamily',
                                //                    disabled: false,
                                //                    flex: 1,
                                //                    editable: true
                                //                }
                                //            ]
                                //        },
                                //        {
                                //            xtype: 'container',
                                //            layout: 'hbox',
                                //            defaults: {
                                //                xtype: 'combobox',
                                //                //     margin: '10 0 5 0',
                                //                labelWidth: 80,
                                //                labelAlign: 'right',
                                //            },
                                //            items: [
                                //                {
                                //                    xtype: 'textfield',
                                //                    name: 'OGRN',
                                //                    itemId: 'dfOGRN2',
                                //                    fieldLabel: 'ОГРН',
                                //                    allowBlank: false,
                                //                    disabled: false,
                                //                    flex: 1,
                                //                    editable: true,
                                //                    maxLength: 15,
                                //                    regex: /^(\d{15})$/,
                                //                    regexText: '15 цифр'
                                //                },
                                //                {
                                //                    xtype: 'textfield',
                                //                    name: 'INN',
                                //                    itemId: 'dfINN2',
                                //                    fieldLabel: 'ИНН',
                                //                    allowBlank: false,
                                //                    disabled: false,
                                //                    flex: 1,
                                //                    editable: true,
                                //                    maxLength: 12,
                                //                    regex: /^(\d{12})$/,
                                //                    regexText: '12 цифр'
                                //                }
                                //            ]
                                //        }
                                //    ]
                                //},
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.manorglicense.License',
                                    name: 'ManOrgLicense',
                                    itemId: 'dfLicense',
                                    editable: true,
                                    labelWidth: 130,
                                    fieldLabel: 'Лицензия',
                                    textProperty: 'LicNumber',
                                    isGetOnlyIdProperty: true,
                                    hidden: true,
                                    columns: [
                                        { text: 'Контрагент', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Номер лицензии', dataIndex: 'LicNumber', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsLicParams',
                                    defaults: {
                                        labelWidth: 120,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    hidden: true,
                                    title: 'Лицензия',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                //{
                                                //    xtype: 'textfield',
                                                //    name: 'KindLic',
                                                //    itemId: 'dfKindLic',
                                                //    fieldLabel: 'Вид лицензии',
                                                //    allowBlank: false,
                                                //    disabled: false,
                                                //    flex: 1,
                                                //    editable: true,
                                                //    maxLength: 15
                                                //},
                                                {
                                                    xtype: 'combobox',
                                                    name: 'FNSLicDecisionType',
                                                    fieldLabel: 'Решение',
                                                    displayField: 'Display',
                                                    itemId: 'dfFNSLicDecisionType',
                                                    flex: 1,
                                                    store: B4.enums.FNSLicDecisionType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                    //hidden: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'NumLic',
                                                    itemId: 'dfNumLic',
                                                    fieldLabel: 'Номер лицензии',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 15
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'SerLic',
                                                    itemId: 'dfSerLic',
                                                    fieldLabel: 'Серия лицензии',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 15
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateLic',
                                                    itemId: 'dfDateLic',
                                                    fieldLabel: 'Дата лицензии',
                                                    margin: '3 0 0 0',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateStartLic',
                                                    itemId: 'dfDateStartLic',
                                                    fieldLabel: 'Дата начала лицензии',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateEndLic',
                                                    itemId: 'dfDateEndLic',
                                                    fieldLabel: 'Дата окончания лицензии',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsLicSvedParams',
                                    defaults: {
                                        labelWidth: 120,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    hidden: true,
                                    title: 'Сведения о лицензии',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'VDName',
                                                    itemId: 'dVDName',
                                                    fieldLabel: 'Наименование ВД',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 500
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Address',
                                                    itemId: 'dfAddress',
                                                    fieldLabel: 'Адрес',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 500
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'SLVDCode',
                                                    itemId: 'dfSLVDCode',
                                                    fieldLabel: 'Код СЛВД',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PrAction',
                                                    itemId: 'dfPrAction',
                                                    fieldLabel: 'Признак действия',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 10
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsDecLOParams',
                                    defaults: {
                                        labelWidth: 120,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    hidden: true,
                                    title: 'Сведения о решении лицензирующего органа',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DecisionKind',
                                                    itemId: 'dfDecisionKind',
                                                    fieldLabel: 'Вид решения',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 15
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DecisionNum',
                                                    itemId: 'dfDecisionNum',
                                                    fieldLabel: 'Номер решения',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 15
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DecisionDate',
                                                    itemId: 'dfDecisionDate',
                                                    fieldLabel: 'Дата решения',
                                                    margin: '3 0 0 0',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DecisionDateStart',
                                                    itemId: 'dfDecisionDateStart',
                                                    fieldLabel: 'Дата начала решения',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DecisionDateEnd',
                                                    itemId: 'dfDecisionDateEnd',
                                                    fieldLabel: 'Дата окончания решения',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsLOParams',
                                    defaults: {
                                        labelWidth: 120,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    hidden: true,
                                    title: 'Сведения о лицензирующем органе',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgFullName',
                                                    fieldLabel: 'Наименование ЛО',
                                                    allowBlank: true,
                                                    itemId: 'dfLicOrgFullName',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgShortName',
                                                    fieldLabel: 'Сокр. наим. ЛО',
                                                    allowBlank: true,
                                                    itemId: 'dfLicOrgShortName',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgOGRN',
                                                    itemId: 'dfLicOrgOGRN',
                                                    fieldLabel: 'ОГРН',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 13,
                                                    regex: /^(\d{13})$/,
                                                    regexText: '13 цифр'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgINN',
                                                    itemId: 'dfLicOrgINN',
                                                    fieldLabel: 'ИНН',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 10,
                                                    regex: /^(\d{10})$/,
                                                    regexText: '10 цифр'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgOKOGU',
                                                    itemId: 'dfLicOrgOKOGU',
                                                    fieldLabel: 'ОКОГУ',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 7,
                                                    regex: /^(\d{7})$/,
                                                    regexText: '7 цифр'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LicOrgRegion',
                                                    itemId: 'dfLicOrgRegion',
                                                    fieldLabel: 'Регион',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 3
                                                }
                                            ]
                                        }
                                    ]
                                }                                
                            ]
                        },
                        //{
                        //    layout: {
                        //        type: 'vbox',
                        //        align: 'stretch'
                        //    },
                        //    defaults: {
                        //        labelWidth: 150,
                        //        margin: '5 0 5 0',
                        //        align: 'stretch',
                        //        labelAlign: 'right'
                        //    },
                        //    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        //    title: 'Отправка информации',
                        //    border: false,
                        //    bodyPadding: 10,
                        //    items: [
                        //        {
                        //            xtype: 'container',
                        //            layout: 'hbox',
                        //            defaults: {
                        //                margin: '0 0 5 0',
                        //                labelWidth: 130,
                        //                labelAlign: 'right',
                        //            },
                        //            items: [
                        //                {
                        //                    xtype: 'textfield',
                        //                    name: 'IdDoc',
                        //                    fieldLabel: 'ID документа',
                        //                    labelWidth: 130,
                        //                    allowBlank: true,
                        //                    itemId: 'dfIdDoc',
                        //                    disabled: false,
                        //                    flex: 1,
                        //                    editable: true
                        //                }
                        //            ]
                        //        }
                                
                        //    ]
                        //},
                        {
                            xtype: 'smevfnslicrequestfileinfogrid',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Answer',
                            itemId: 'dfAnswer',
                            fieldLabel: 'Ответ',
                            allowBlank: true,
                            disabled: false,
                            labelWidth: 100,
                            labelAlign: 'right',
                            editable: false,
                            flex: 0.7
                        }                       
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});