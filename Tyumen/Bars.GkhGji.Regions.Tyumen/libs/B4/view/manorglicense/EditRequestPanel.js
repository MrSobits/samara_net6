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
    title: 'Обращение за выдачей лицензии',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.manorglicense.ListManOrg',
        'B4.view.manorglicense.PersonGrid',
        'B4.view.manorglicense.ProvDocGrid',
        'B4.view.manorglicense.RequestAnnexEditWindow',
        'B4.view.manorglicense.RequestAnnexGrid',
        'B4.view.manorglicense.RequestInspectionGrid',
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
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать проверку',
                                    iconCls: 'icon-accept',
                                    action: 'createInspection'
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
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Contragent',
                                    fieldLabel: 'Управляющая организация',
                                    store: 'B4.store.manorglicense.ListManOrg',
                                    editable: false,
                                    allowBlank: false,
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
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
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
                                            allowBlank: false,
                                            fieldLabel: 'Дата обращения',
                                            format: 'd.m.Y',
                                            flex:1
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            hideTrigger: true,
                                            minValue: 0,
                                            name: 'RegisterNum',
                                            fieldLabel: 'Регистрационный номер',
                                            allowBlank: true,
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ConfirmationOfDuty',
                                    fieldLabel: 'Документ, подтверждающий уплату гос. пошлины',
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'ReasonOffers',
                                    fieldLabel: 'Основание предложения',
                                    height:50,
                                    maxLength: 10000
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Файл'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'ReasonRefusal',
                                    fieldLabel: 'Причина отказа',
                                    height: 50,
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'manorglicensepersongrid',
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
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Информация о заявителе',
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