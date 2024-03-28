Ext.define('B4.view.manorglicense.LicenseGisEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.licensegiseditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align : 'stretch',
        pack  : 'start'
    },
    width: 900,
    minWidth: 800,
    height: 600,
    resizable: true,
    bodyPadding: 3,
    title: 'Панель управления лицензией',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.view.manorglicense.LicensePrescriptionGrid',
        'B4.view.manorglicense.LicenseNotificationGisGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                padding: '0 5 5 0',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Contragent',
                                    labelAlign: 'right',
                                    fieldLabel: 'Контрагент',
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Inn',
                                    labelAlign: 'right',
                                    fieldLabel: 'ИНН',
                                    flex: 0.5
                                },
                                {
                                    xtype: 'gkhintfield',
                                    labelAlign: 'right',
                                    hideTrigger: true,
                                    minValue: 0,
                                    name: 'LicNum',
                                    flex: 0.5,
                                    fieldLabel: 'Номер лицензии',
                                    allowBlank: true
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
                                padding: '0 5 5 0',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'MkdCount',
                                    allowBlank: true,
                                    flex: 1,
                                    fieldLabel: 'Количество МКД в управлении'
                                },
                                {
                                    xtype: 'component',
                                    flex: 0.5
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MKDArea',
                                    allowBlank: true,
                                    flex: 1,
                                    fieldLabel: 'Площадь МКД в управлении'
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
                                padding: '0 5 5 0',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'StateName',
                                    allowBlank: true,
                                    flex: 0.5,
                                    fieldLabel: 'Статус'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateIssued',
                                    allowBlank: false,
                                    fieldLabel: 'Дата выдачи', 
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateRegister',
                                    allowBlank: false,
                                    labelWidth:200,
                                    fieldLabel: 'Дата внесения в реестр лицензий',
                                    format: 'd.m.Y'
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
                                padding: '0 5 5 0',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ROAddress',
                                    allowBlank: false,
                                    flex: 1,
                                    fieldLabel: 'Адрес МКД в управлении'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ManStartDate',
                                    allowBlank: false,
                                    flex: 0.5,
                                    fieldLabel: 'Дата начала',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ManEndDate',
                                    allowBlank: true,
                                    flex: 0.5,
                                    fieldLabel: 'Дата окончания',
                                    format: 'd.m.Y'
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
                                            flex: 1,
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
                                            flex: 1,
                                            allowBlank: false,
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y'
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
                                    xtype: 'licenseprescriptiongrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'manorglicensnotificationgisgrid',
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
                        { xtype: 'b4closebutton' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});