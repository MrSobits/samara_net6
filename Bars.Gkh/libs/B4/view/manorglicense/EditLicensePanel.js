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
        'B4.view.manorglicense.LicenseExtensionGrid',
        'B4.enums.TypeManOrgTerminationLicense',
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
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить в ЕРУЛ',
                                    name: 'calculate2Button',
                                    tooltip: 'Обновить сведения в ЕРУЛ',
                                    action: 'ERULUpdateRequest',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculate2Button'
                                },
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
                            name: 'Contragent',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.manorglicense.ListManOrg',
                            readOnly: true,
                            flex:1
                        },
                        //{
                        //    xtype: 'gkhintfield',
                        //    labelAlign: 'right',
                        //    hideTrigger: true,
                        //    labelWidth: 200,
                        //    widht: 375,
                        //    minValue: 0,
                        //    name: 'LicNum',
                        //    fieldLabel: 'Номер лицензии',
                        //    allowBlank: true
                        //},
                        {
                            xtype: 'textfield',
                            name: 'LicNumber',
                            allowBlank: false,
                            labelWidth: 200,
                            width: 375,
                            fieldLabel: 'Номер лицензии'
                        },
                        {
                            xtype: 'component',
                            width:120
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
                            xtype: 'datefield',
                            labelWidth: 200,
                            widht: 250,
                            name: 'DateValidity',
                            allowBlank: true,
                            fieldLabel: 'Срок действия',
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
                    xtype: 'fieldset',
                    title: 'ЕРУЛ',
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
                                    name: 'ERULNumber',
                                    allowBlank: true,
                                    labelWidth: 190,
                                    width: 365,
                                    fieldLabel: 'Номер'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ERULDate',
                                    labelWidth: 100,
                                    width: 275,
                                    allowBlank: true,
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'component',
                                    width: 90
                                },
                                {
                                    xtype: 'button',
                                    text: 'Запросить номер лицензии в ЕРУЛ',
                                    name: 'calculateButton',
                                    tooltip: 'Запросить номер лицензии в ЕРУЛ',
                                    action: 'ERULRequest',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculateButton'
                                },
                            ]
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
                                    width:90
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
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    labelWidth: 190,
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
                                    xtype: 'datefield',
                                    labelWidth: 200,
                                    width: 375,
                                    name: 'DateTermination',
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
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                       {
                           xtype: 'manorglicensedocgrid',
                           flex: 1
                        },
                        {
                            xtype: 'manorglicenseextensiongrid',
                            flex: 1
                        }
                    ]
                }            
            ]
        });

        me.callParent(arguments);
    }
});