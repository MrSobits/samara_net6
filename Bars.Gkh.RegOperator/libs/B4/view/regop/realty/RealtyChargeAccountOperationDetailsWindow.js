Ext.define('B4.view.regop.realty.RealtyChargeAccountOperationDetailsWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.rchaopdetailswin',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.Panel',
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'Ext.data.Store',
        'B4.base.Proxy'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    title: 'Детализация по счету',

    width: 700,
    height: 500,

    layout: 'fit',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'ChargeTotal' },
                    { name: 'Dept' },
                    { name: 'RoomNum' },
                    { name: 'PeriodId' },
                    { name: 'AccountId' },
                    { name: 'Penalty' },
                    { name: 'PenaltyPayment' },
                    { name: 'Period' },
                    { name: 'PersonalAccountNum' },
                    { name: 'Recalc' },
                    { name: 'SaldoIn' },
                    { name: 'SaldoOut' },
                    { name: 'TotalPayment' },
                    { name: 'ChargedByBaseTariff' },
                    { name: 'TariffPayment' },
                    { name: 'ChargeByDecision' },
                    { name: 'TariffDecisionPayment' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'PersonalAccountPeriodSummary'
                },
                autoLoad: false
            });

        Ext.apply(me, {
            items: {
                store: store,
                xtype: 'b4grid',
                name: 'accountsummarygrid',
                border: 0,
                columns: [
                    {
                        xtype: 'b4editcolumn',
                        scope: me
                    },
                    {
                        text: 'Номер ЛС',
                        dataIndex: 'PersonalAccountNum',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        text: '№ помещения',
                        dataIndex: 'RoomNum',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        text: 'Вх. сальдо',
                        dataIndex: 'SaldoIn',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Начислено на базовому тарифу',
                        dataIndex: 'ChargedByBaseTariff',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Оплачено по базовому тарифу',
                        dataIndex: 'TariffPayment',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Начислено по тарифу решения',
                        dataIndex: 'ChargeByDecision',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Оплачено по тарифу решения',
                        dataIndex: 'TariffDecisionPayment',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Пени начислено',
                        dataIndex: 'Penalty',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Пени оплачено',
                        dataIndex: 'PenaltyPayment',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    },
                    {
                        text: 'Исх сальдо',
                        dataIndex: 'SaldoOut',
                        xtype: 'numbercolumn',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            hideTrigger: true
                        }
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'b4updatebutton',
                                listeners: {
                                    click: function(btn) {
                                        btn.up('grid').getStore().load();
                                    }
                                }
                            },
                            {
                                xtype: 'tbfill'
                            },
                            {
                                xtype: 'b4closebutton',
                                listeners: {
                                    click: function (btn) {
                                        btn.up('rchaopdetailswin').close();
                                    }
                                }
                            }
                        ]

                    },
                    {
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: store,
                        dock: 'bottom'
                    }
                ]
            }
        });
        me.callParent(arguments);
    }
});