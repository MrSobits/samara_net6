Ext.define('B4.view.claimwork.buildcontract.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.buildcontracteditpanel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    
    title: 'Общие сведения',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save',
        'B4.view.claimwork.buildcontract.ViolGrid',
        'B4.enums.BuildContractCreationType',
        'B4.ux.button.AcceptMenuButton'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Подрядчик',
                            name: 'Builder',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'ИНН',
                            name: 'Inn',
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '№ договора',
                                    readOnly: true,
                                    name: 'DocumentNum'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDateFrom',
                                    readOnly: true,
                                    fieldLabel: 'Дата договора',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateEndWork',
                                    readOnly: true,
                                    fieldLabel: 'Срок выполнения работ',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'CountDaysDelay',
                                    fieldLabel: 'Количество дней просрочки',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    readOnly: true,
                                    minValue: 0,
                                    negativeText: 'Значение не может быть отрицательным'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Статус нарушения',
                                    readOnly: true,
                                    name: 'StateName'
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    name: 'CreationType',
                                    fieldLabel: 'Способ формирования',
                                    displayField: 'Display',
                                    readOnly: true,
                                    store: B4.enums.BuildContractCreationType.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsDebtPaid',
                                    fieldLabel: 'Нарушения устранены'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DebtPaidDate',
                                    fieldLabel: 'Дата устранения'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'buildcontractviolgrid',
                    flex: 1
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
                                },
                                {
                                    text: 'Договор',
                                    action: 'goContract',
                                    iconCls: 'icon-page-forward'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    textAlign: 'left',
                                    actionName: 'updState',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh'
                                },
                                {
                                    xtype: 'acceptmenubutton'
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