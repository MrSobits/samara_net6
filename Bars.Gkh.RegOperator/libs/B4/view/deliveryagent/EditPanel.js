Ext.define('B4.view.deliveryagent.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.deliveryagenteditpanel',
    title: 'Общие сведения',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.dict.OrganizationForm',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save'
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
                        anchor: '100%',
                        readOnly: true
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
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
                            editable: false
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
                                 readOnly: true,
                                 flex: 1
                             },
                             items: [
                                {
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    maxLength: 20
                                },
                                {
                                    name: 'Kpp',
                                    fieldLabel: 'КПП',
                                    maxLength: 20
                                }
                             ]
                         },
                         {
                             xtype: 'textfield',
                             name: 'JuridicalAddress',
                             fieldLabel: 'Юридический адрес',
                             readOnly: true
                         },
                        {
                            xtype: 'textfield',
                            name: 'FactAddress',
                            fieldLabel: 'Фактический адрес',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            name: 'MailingAddress',
                            fieldLabel: 'Почтовый адрес'
                        }
                    ]
                },
            {
                xtype: 'fieldset',
                defaults: {
                    labelWidth: 250,
                    anchor: '50%',
                    readOnly: true
                },
                layout:{
                    type: 'hbox',
                    align:'stretch'
                 },
                title: 'Сведения о регистрации',
                items: [
                            {
                                xtype: 'textfield',
                                name: 'Ogrn',
                                labelAlign: 'right',
                                fieldLabel: 'ОГРН',
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
                }
            ],
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
                                            contragentId = record.get('Contragent') ? record.get('Contragent') : 0;

                                        if (contragentId) {
                                            Ext.History.add(Ext.String.format('contragentedit/{0}/', contragentId));
                                        }
                                    }
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