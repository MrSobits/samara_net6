﻿Ext.define('B4.view.contragent.EditPanel', {
    extend: 'Ext.form.Panel', /*переопределение для Татарстана*/

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    alias: 'widget.contragentEditPanel',
    title: 'Общие сведения',

    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.dict.OrganizationForm',
        'B4.store.dict.ContragentRole',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.store.contragent.ContragentExceptChildren',
        'B4.enums.ContragentState',
        'B4.enums.TypeEntrepreneurship',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'ShortName',
                            fieldLabel: 'Краткое наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'OrganizationForm',
                            fieldLabel: 'Организационно-правовая форма',
                            store: 'B4.store.dict.OrganizationForm',
                            allowBlank: false,
                            editable: false,
                            itemId: 'sfCtrgOrgForm'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Parent',
                            fieldLabel: 'Головная организация',
                            store: 'B4.store.contragent.ContragentExceptChildren',
                            columns: [
                                {
                                    xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальный район', flex: 1, filter: {
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
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false,
                            itemId: 'sfParent'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'MainRole',
                            fieldLabel: 'Основная роль',
                            store: 'B4.store.dict.ContragentRole',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Включен в СОПР',
                            name: 'IncludeInSopr',
                            labelWidth: 250,
                            labelAlign: 'right'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Сведения по поставщикам',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ProviderCode',
                                    flex: 1,
                                    fieldLabel: 'Код поставщика'
                                },
                                {
                                    xtype: 'button',
                                    action: 'GenerateProviderCode',
                                    text: 'Получить код',
                                    margin: '0 0 0 10'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты',
                    items: [
                         {
                             xtype: 'container',
                             padding: '0 0 5 0',
                             layout: 'hbox',
                             defaults: {
                                 xtype: 'textfield',
                                 labelAlign: 'right',
                                 labelWidth: 250,
                                 flex: 1
                             },
                             items: [
                                {
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    itemId: 'tfCtrgInn',
                                    maxLength: 20
                                },
                                {
                                    name: 'Kpp',
                                    fieldLabel: 'КПП',
                                    itemId: 'tfCtrgKpp',
                                    maxLength: 20
                                }
                             ]
                         },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'FiasJuridicalAddress',
                            itemId: 'contragentFiasJuridicalAddressField',
                            fieldLabel: 'Юридический адрес',
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
                        {
                            xtype: 'container',
                            anchor: '100%',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    flex: 1,
                                    labelAlign: 'right',
                                    name: 'FiasFactAddress',
                                    itemId: 'contragentFiasFactAddressField',
                                    fieldLabel: 'Фактический адрес',
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
                                {
                                    xtype: 'button',
                                    name: 'CopyButtonFactAddress',
                                    itemId: 'btnCopyButtonFactAddress',
                                    text: 'Заполнить',
                                    width: 70,
                                    margin: '0 0 0 10'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    flex: 1,
                                    labelAlign: 'right',
                                    name: 'FiasMailingAddress',
                                    itemId: 'contragentFiasMailingAddressField',
                                    fieldLabel: 'Почтовый адрес',
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
                                {
                                    xtype: 'button',
                                    width: 70,
                                    name: 'CopyButtonMailingAddress',
                                    itemId: 'btnCopyButtonMailingAddress',
                                    text: 'Заполнить',
                                    margin: '0 0 0 10'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            flex: 1,
                            name: 'AddressOutsideSubject',
                            itemId: 'tfcontragentOutsideAddress',
                            fieldLabel: 'Адрес за пределами субъекта',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%'
                    },
                    title: 'Сведения о регистрации',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250,
                                width: '50%'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Ogrn',
                                    labelAlign: 'right',
                                    fieldLabel: 'ОГРН',
                                    itemId: 'tfCtrgOgrn',
                                    maxLength: 250
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateRegistration',
                                    fieldLabel: 'Дата регистрации',
                                    labelAlign: 'right',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            name: 'OgrnRegistration',
                            fieldLabel: 'Орган, принявший решение о регистрации',
                            maxLength: 300
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%'
                    },
                    title: 'Свидетельство о постановке на учет в налоговом органе',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 125
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'TaxRegistrationSeries',
                                    labelAlign: 'right',
                                    fieldLabel: 'Серия',
                                    itemId: 'tfTaxRegistrationSeries',
                                    width: '30%',
                                    labelWidth: 250
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TaxRegistrationNumber',
                                    labelAlign: 'right',
                                    fieldLabel: 'Номер',
                                    itemId: 'tfTaxRegistrationNumber',
                                    width: '40%'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TaxRegistrationDate',
                                    itemId: 'tfTaxRegistrationDate',
                                    fieldLabel: 'Дата',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    width: '30%'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            name: 'TaxRegistrationIssuedBy',
                            itemId: 'tfTaxRegistrationIssuedBy',
                            fieldLabel: 'Кем выдан',
                            maxLength: 300
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%'
                    },
                    title: 'Дополнительные сведения',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 250,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            labelAlign: 'right',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            maxValue: 2000000000,
                                            name: 'Okpo',
                                            fieldLabel: 'ОКПО'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            labelAlign: 'right',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            minValue: 0,
                                            name: 'Oktmo',
                                            fieldLabel: 'OKTMO'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Phone',
                                            labelAlign: 'right',
                                            fieldLabel: 'Телефон',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Fax',
                                            labelAlign: 'right',
                                            fieldLabel: 'Факс',
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'textfield',
                                            labelAlign: 'right',
                                            name: 'Email',
                                            fieldLabel: 'E-mail',
                                            maxLength: 200,
                                            regex: /^([\w\-\'\-]+)(\.[\w\'\-]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/
                                        },
                                        {
                                            xtype: 'combobox', 
                                            editable: false,
                                            fieldLabel: 'Получать оповещения на E-mail',
                                            labelAlign: 'right',
                                            store: B4.enums.YesNo.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'ReceiveNotifications'
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 250,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    itemId: 'cbIsSite',
                                                    name: 'IsSite',
                                                    fieldLabel: 'Официальный сайт',
                                                    listeners: {
                                                        change: {
                                                            fn: function (field) {
                                                                var tfOfficialWebsite = field.up('container').down('#tfOfficialWebsite');
                                                                tfOfficialWebsite.setDisabled(!this.checked);
                                                                tfOfficialWebsite.reset();
                                                                tfOfficialWebsite.allowBlank = !this.checked;
                                                            }
                                                        }
                                                    },
                                                    flex: 0.1
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OfficialWebsite',
                                                    itemId: 'tfOfficialWebsite',
                                                    listeners: {
                                                        render: {
                                                            fn: function (field) {
                                                                var cbIsSite = field.up('container').down('#cbIsSite');
                                                                this.setDisabled(!cbIsSite.checked);
                                                                if (!cbIsSite.checked)
                                                                    this.reset();
                                                                Ext.create('Ext.tip.ToolTip', {
                                                                    target: this.getEl().getAttribute("id"),
                                                                    trackMouse: true,
                                                                    width: 200,
                                                                    html: 'При наличии официального сайта вводится адрес сайта'
                                                                });
                                                            }
                                                        }
                                                    },
                                                    padding: '0 0 0 280',
                                                    width: '100%',
                                                    maxLength: 250,
                                                    regex: /^(\S{2,256})(\.{1})([A-Za-zА-Яа-я0-9]{2,13})$/
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FrguOrgNumber',
                                            fieldLabel: 'Номер организации в ФРГУ',
                                            labelAlign: 'right',
                                            regex: /^[0-9]*$/,
                                            maxLength: 36
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 250,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Okved',
                                            fieldLabel: 'ОКВЭД',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'combobox', editable: false,
                                            fieldLabel: 'Тип предпринимательства',
                                            store: B4.enums.TypeEntrepreneurship.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'TypeEntrepreneurship'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PhoneDispatchService',
                                            fieldLabel: 'Телефон диспетчерской службы',
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'SubscriberBox',
                                            fieldLabel: 'Абонентский ящик',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TweeterAccount',
                                            fieldLabel: 'Твитер аккаунт',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FrguRegNumber',
                                            fieldLabel: 'Реестровый номер функции в ФРГУ',
                                            labelAlign: 'right',
                                            regex: /^[0-9]*$/,
                                            maxLength: 36
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FrguServiceNumber',
                                            fieldLabel: 'Номер услуги в ФРГУ',
                                            labelAlign: 'right',
                                            regex: /^[0-9]*$/,
                                            maxLength: 36
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            labelAlign: 'right',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 2000
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%'
                    },
                    title: 'Прекращение деятельности',
                    items: [
                        {
                            xtype: 'container',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Статус',
                                    store: B4.enums.ContragentState.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'ContragentState',
                                    listeners: {
                                        change: {
                                            fn: function (field, newValue) {
                                                var dfDateTermination = field.up('panel').down('#dfDateTermination');
                                                if (newValue != 10) {
                                                    dfDateTermination.allowBlank = false;
                                                } else {
                                                    dfDateTermination.allowBlank = true;
                                                }
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateTermination',
                                    fieldLabel: 'Дата прекращения деятельности',
                                    format: 'd.m.Y',
                                    itemId: 'dfDateTermination'
                                }
                            ]
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});