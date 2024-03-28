Ext.define('B4.view.claimwork.IndividualEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.individualclaimworkeditpanel',
    title: 'Общие сведения',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.store.Contragent',
        'B4.enums.DebtorState',
        'B4.view.claimwork.AccountDetailGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            flex: 9,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    name: 'AccountOwnerName',
                                    fieldLabel: 'Абонент'
                                },
                                {
                                    name: 'CurrChargeBaseTariffDebt',
                                    fieldLabel: 'Сумма текущей задолженности по базовому тарифу'
                                },
                                {
                                    name: 'CurrChargeDecisionTariffDebt',
                                    fieldLabel: 'Сумма текущей задолженности по тарифу решения'
                                },
                                {
                                    name: 'CurrChargeDebt',
                                    fieldLabel: 'Общая сумма текущей задолженности'
                                },
                                {
                                    name: 'CurrPenaltyDebt',
                                    fieldLabel: 'Общая сумма текущей задолженности по пени'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 10,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 220,
                                labelAlign: 'right',
                                readOnly: true,
                                padding: '0 0 5 0'
                            },
                            items: [
                                {
                                    name: 'RegistrationAddress',
                                    fieldLabel: 'Адрес по прописке'
                                },
                                {
                                    name: 'OrigChargeBaseTariffDebt',
                                    fieldLabel: 'Сумма просроченной задолженности по базовому тарифу'
                                },
                                {
                                    name: 'OrigChargeDecisionTariffDebt',
                                    fieldLabel: 'Сумма просроченной задолженности по тарифу решения'
                                },
                                {
                                    name: 'OrigChargeDebt',
                                    fieldLabel: 'Общая сумма просроченной задолженности'
                                },
                                {
                                    name: 'OrigPenaltyDebt',
                                    fieldLabel: 'Общая сумма просроченной задолженности по пени'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'DebtorState',
                    enumName: 'B4.enums.DebtorState',
                    fieldLabel: 'Статус задолженности',
                    itemId: 'enDebtorState',
                    readOnly: true,
                    labelAlign: 'right',
                    labelWidth: 200
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        flex: 1
                    },
					items: [
						{
							xtype: 'datefield',
							name: 'PIRCreateDate',
							format: 'd.m.Y',
							fieldLabel: 'Дата создания ПИР'
						},
                        {
                            xtype: 'textfield',
                            name: 'SubContractNum',
                            fieldLabel: 'Номер договора с подрядчиком'
                        },
                        {
                            xtype: 'datefield',
                            name: 'SubContractDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата договора с подрядчиком'
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
						labelWidth: 200,
						labelAlign: 'right',
						flex: 1
					},
					items: [
						{
							xtype: 'b4selectfield',
							store: 'B4.store.Contragent',
							textProperty: 'ShortName',
							name: 'SubContragent',
							fieldLabel: 'Подрядчик',
							itemId: 'sfContragent',
							disabled: false,
							editable: false,
							allowBlank: true,
							columns: [
								{
									header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
								{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
								{ header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
								{ header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
							]
						},
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
                        labelWidth: 200,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'IsDebtPaid',
                            fieldLabel: 'Задолженность погашена'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DebtPaidDate',
                            format: 'd.m.Y',
                            fieldLabel: 'Дата погашения'
                        }
                    ]
                },
                {
                    xtype: 'claimworkaccountdetailgrid',
                    margin: '10 0 0 0',
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
                                    xtype: 'button',
                                    text: 'Обновить данные',
                                    textAlign: 'left',
                                    actionName: 'updState',
                                    tooltip: 'Обновление сумм и статусов',
                                    iconCls: 'icon-page-refresh'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                {
                                    xtype: 'button',
                                   // iconCls: 'icon-cross',
                                 //   iconCls: 'icon-build',
                                  //  iconCls: 'icon-decline',
                                  //  iconCls: 'icon-basket-add',
                                   // iconCls: 'icon-collapse-all',
                                    iconCls: 'icon-stop',
                                    text: 'Приостановить',
                                    textAlign: 'left',
                                    itemId:'btnStop',
                                    actionName: 'pauseState'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-basket-add',
                                    text: 'Возобновить',
                                    textAlign: 'left',
                                    itemId: 'btnStart',
                                    actionName: 'resumeState'
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