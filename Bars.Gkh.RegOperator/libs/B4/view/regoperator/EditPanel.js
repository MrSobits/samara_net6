Ext.define('B4.view.regoperator.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.form.SelectField'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'regoperatorEditPanel',
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
                    xtype: 'b4selectfield',
                    store: 'B4.store.Contragent',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    name: 'Contragent',
                    fieldLabel: 'Контрагент',
                    editable: false,
                    allowBlank: false,
                    labelWidth: 170,
                    labelAlign: 'right'
                },
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
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelAlign: 'right',
                                labelWidth: 170,
                                readOnly:true,
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
                               }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'FactAddress',
                            fieldLabel: 'Фактический адрес'
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
                                   name: 'Phone',
                                   fieldLabel: 'Телефон'
                               },
                               {
                                   name: 'Email',
                                   fieldLabel: 'E-mail'
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