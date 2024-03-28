Ext.define('B4.view.manorglicense.EditLicenseGisPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manOrgLicenseGisEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Постановления',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.enums.TypeManOrgTerminationLicense',
        'B4.store.manorglicense.LicenseGis',
        'B4.store.manorglicense.LicenseResolutionGis'
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
                                    xtype: 'component',
                                    width: 10
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
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
                             xtype: 'textfield',
                             name: 'Contragent',
                             labelAlign: 'right',
                             labelWidth: 200,
                             fieldLabel: 'Контрагент',
                             flex: 1
                         },
                        {
                            xtype: 'gkhintfield',
                            labelAlign: 'right',
                            hideTrigger: true,
                            labelWidth: 200,
                            widht: 375,
                            minValue: 0,
                            name: 'LicNum',
                            fieldLabel: 'Номер лицензии',
                            allowBlank: true
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
                             xtype: 'textfield',
                             name: 'ROAddress',
                             allowBlank: false,
                             labelWidth: 190,
                             width: 600,
                             fieldLabel: 'Адрес МКД в управлении'
                         },
                        {
                            xtype: 'component',
                            flex: 1
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
                //{
                //    xtype: 'fieldset',
                //    title: 'Прекращение действия лицензии',
                //    anchor: '100%',
                //    layout: {
                //        type: 'vbox',
                //        align: 'stretch'
                //    },
                //    defaults: {
                //        padding: '0 0 5 0',
                //        xtype: 'container',
                //        layout: 'hbox'
                //    },
                //    items: [
                //        {
                //            xtype: 'container',
                //            layout: 'hbox',
                //            defaults: {
                //                falign: 'stretch',
                //                labelAlign: 'right'
                //            },
                //            items: [
                                
                //                {
                //                    xtype: 'datefield',
                //                    labelWidth: 200,
                //                    width: 375,
                //                    name: 'DateTermination',
                //                    fieldLabel: 'Дата',
                //                    format: 'd.m.Y'
                //                },
                //                {
                //                    xtype: 'component',
                //                    width: 90
                //                }
                //            ]
                //        }
                //    ]
                //}
                
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                       {
                           xtype: 'manorglicenseresolutiongisgrid',
                           flex: 1
                       }
                    ]
                }          
            ]
        });

        me.callParent(arguments);
    }
});