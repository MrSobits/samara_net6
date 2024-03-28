Ext.define('B4.view.gasequipmentorg.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.gasequipmentorgEditPanel',

    requires: [
        'B4.store.Contragent',
        'B4.store.contragent.Contact',
        'B4.ux.button.Save',
        'B4.form.SelectField'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 800,
    width: 800,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    title: 'Общие сведения',
    trackResetOnLoad: true,
    frame: true,

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
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            action: 'GoToContragent',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Перейти к контрагенту',
                                    iconCls: 'icon-arrow-out',
                                    panel: me,
                                    handler: function () {
                                        var me = this,
                                            form = me.panel.getForm(),
                                            record = form.getRecord(),
                                            contragentId = record.get('ContragentId') ? record.get('ContragentId') : 0;

                                        if (contragentId) {
                                            Ext.History.add(Ext.String.format('contragentedit/{0}/', contragentId));
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [                
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        readOnly: true,
                        anchor: '100%'
                    },
                    title: 'Общие сведения',
                    items: [                        
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование полное'
                        },                        
                        {
                            xtype: 'textfield',
                            name: 'Contragent',
                            fieldLabel: 'Наименование краткое'
                        },
                        {
                            xtype: 'textfield',
                            name: 'JuridicalAddress',
                            fieldLabel: 'Юридический адрес'
                        },
                        {
                            xtype: 'b4selectfield',
                            readOnly: false,
                            allowBlank:false,
                            store: 'B4.store.contragent.Contact',
                            name: 'Contact',
                            textProperty: 'FullName',
                            idProperty: 'Id',
                            fieldLabel: 'ФИО директора',
                            columns: [
                                {
                                    text: 'ФИО',
                                    dataIndex: 'FullName',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    text: 'Должность',
                                    dataIndex: 'Position',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    text: 'Дата начала',
                                    dataIndex: 'DateStartWork',
                                    format: 'd.m.Y',
                                    flex: 1,
                                    filter: {
                                        xtype: 'datefield'
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    text: 'Дата окончания',
                                    dataIndex: 'DateEndWork',
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield'
                                    }
                                }]
                            },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 170,
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'Inn',
                                   fieldLabel: 'ИНН'
                               },
                               {
                                   name: 'Kpp',
                                   fieldLabel: 'КПП'
                               },
                               {
                                   name: 'Ogrn',
                                   fieldLabel: 'ОГРН'
                               },
                               {
                                   name: 'Phone',
                                   fieldLabel: 'Телефон'
                               }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 170,
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                               {
                                    xtype: 'datefield',
                                    name: 'DateRegistration',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата начала деятельности'
                               },
                               {
                                    xtype: 'datefield',
                                    name: 'DateTermination',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата прекращения деятельности'
                               },
                            ]
                        },                                       
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});