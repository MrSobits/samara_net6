Ext.define('B4.view.realityobj.ExistingSolutionsPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.existingsolutionspanel',

    requires: [
        'B4.ux.button.Update',
        'B4.view.realityobj.MonthlyFeeHistoryGrid',
        'B4.enums.AccountOwnerDecisionType',
        'B4.enums.MkdManagementDecisionType',
        'B4.enums.CrFundFormationDecisionType'
    ],

    title: 'Действующие решения',
    closable: true,
    enableColumnHide: true,
    itemId: 'realityobjExistingSolutionsPanel',
    layout: 'fit',


    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    autoScroll: true,
                    bodyStyle: Gkh.bodyStyle,
                    bodyPadding: 5,
                    border: false,
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Способ управления домом',
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '0 0 6 0',
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 200,
                                        xtype: 'datefield',
                                        labelAlign: 'right',
                                        readOnly: true
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Дата начала управления',
                                            name: 'ManageStart'
                                        },
                                        {
                                            fieldLabel: 'Дата окончания управления',
                                            name: 'ManageEnd'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Принятое решение',
                                    name: 'ManageDecision'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'УО',
                                    name: 'ManageUo'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Способ формирования фонда',
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '0 0 6 0',
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 200,
                                        xtype: 'datefield',
                                        labelAlign: 'right',
                                        readOnly: true
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Дата ввода в действие решение',
                                            name: 'CrFundStart'
                                        },
                                        {
                                            fieldLabel: 'Дата окончания действия',
                                            name: 'CrFundEnd'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Принятое решение',
                                    name: 'CrFundDecision'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Владелец счета',
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '0 0 6 0',
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 200,
                                        xtype: 'datefield',
                                        labelAlign: 'right',
                                        readOnly: true
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Дата ввода в действие решение',
                                            name: 'OwnerStart'
                                        },
                                        {
                                            fieldLabel: 'Дата окончания действия',
                                            name: 'OwnerEnd'
                                        }
                                    ]
                                },
                                //{
                                //    xtype: 'textfield',
                                //    fieldLabel: 'Принятое решение',
                                //    name: 'OwnerDecision'
                                //},
                                //{
                                //    xtype: 'textfield',
                                //    fieldLabel: 'Тип контрагента',
                                //    name: 'OwnerContragentType'
                                //},
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Наименование контрагента',
                                    name: 'OwnerContragentName'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Уполномоченное лицо',
                            labelWidth: 200,
                            labelAlign: 'right',
                            name: 'AuthorizedPerson',
                            readOnly: true,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Банк (кредитная организация)',
                            labelWidth: 200,
                            labelAlign: 'right',
                            name: 'CreditOrg',
                            readOnly: true,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Рассчетный счет',
                            labelWidth: 200,
                            labelAlign: 'right',
                            name: 'AccountNumber',
                            readOnly: true,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Минимальный размер фонда КР',
                            name: 'MinFundAmount',
                            labelWidth: 200,
                            labelAlign: 'right',
                            readOnly: true,
                            anchor: '100%'
                        },
                        {
                            xtype: 'monthlyfeehistorygrid',
                            height: 400
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
                                    xtype: 'b4updatebutton'
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