Ext.define('B4.view.paymentagent.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    itemId: 'paymentAgentEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right',
                        xtype: 'textfield',
                        readOnly: true
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            name: 'CtrName',
                            fieldLabel: 'Наименование'
                        },
                        {
                            name: 'CtrShortName',
                            fieldLabel: 'Краткое наименование'
                        },
                        {
                            name: 'CtrOrgFormName',
                            fieldLabel: 'Организационно-правовая форма'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right',
                        xtype: 'textfield',
                        readOnly: true
                    },
                    title: 'Реквизиты',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right',
                                xtype: 'textfield',
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'CtrInn',
                                   fieldLabel: 'ИНН'
                               },
                                {
                                    name: 'CtrKpp',
                                    fieldLabel: 'КПП'
                                }
                            ]
                        },
                        {
                            name: 'CtrJurAdress',
                            fieldLabel: 'Юридический адрес'
                        },
                        {
                            name: 'CtrFactAdress',
                            fieldLabel: 'Фактический адрес'
                        },
                        {
                            name: 'CtrMailAddress',
                            fieldLabel: 'Почтовый адрес'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right',
                        xtype: 'textfield',
                        readOnly: true,
                        flex: 1
                    },
                    title: 'Сведения о регистрации',
                    items: [
                        {
                            name: 'CtrOgrn',
                            fieldLabel: 'ОГРН'
                        },
                        {
                            name: 'CtrDateReg',
                            fieldLabel: 'Дата регистрации'
                        }
                    ]
                },
                {
                    labelWidth: 190,
                    anchor: '100%',
                    labelAlign: 'right',
                    xtype: 'textfield',
                    allowBlank: false,
                    flex: 0.5,
                    name: 'Code',
                    fieldLabel: 'Идентификатор'
                },
                {
                    labelWidth: 190,
                    anchor: '100%',
                    labelAlign: 'right',
                    xtype: 'textfield',
                    name: 'PenaltyContractId',
                    fieldLabel: 'Id договора загрузки пени',
                    maxLength: 3
                },
                {
                    labelWidth: 190,
                    anchor: '100%',
                    labelAlign: 'right',
                    xtype: 'textfield',
                    name: 'SumContractId',
                    fieldLabel: 'Id договора загрузки суммы',
                    maxLength: 3
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
                                            contragentId = record.get('Contragent') ? record.get('Contragent').Id : 0;

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