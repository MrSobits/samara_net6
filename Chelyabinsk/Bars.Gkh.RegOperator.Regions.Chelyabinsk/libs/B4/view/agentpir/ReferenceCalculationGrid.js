Ext.define('B4.view.agentpir.ReferenceCalculationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.agentpirreferencecalculationgrid',

    requires: [

        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.field.AreaShareField',
        'B4.store.DebtorReferenceCalculation'
    ],

    title: 'Расчет задолженности',
    closable: false,
    enableColumnHide: true,
    store: 'DebtorReferenceCalculation',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    name: 'AccountNumber',
                    dataIndex: 'AccountNumber',
                    text: 'Лицевой счет',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Name',
                    dataIndex: 'Name',
                    text: 'Период',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'AreaShare',
                    dataIndex: 'AreaShare',
                    text: 'Доля собственности',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'RoomArea',
                    dataIndex: 'RoomArea',
                    text: 'Площадь </br>помещения',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'BaseTariff',
                    dataIndex: 'BaseTariff',
                    text: 'Тариф',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TariffCharged',
                    dataIndex: 'TariffCharged',
                    text: 'Начислено </br>за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TarifPayment',
                    dataIndex: 'TarifPayment',
                    text: 'Оплачено </br>за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PaymentDate',
                    dataIndex: 'PaymentDate',
                    text: 'Дата платежа',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TarifDebt',
                    dataIndex: 'TarifDebt',
                    text: 'Задолженность </br>за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TarifDebtPay',
                    dataIndex: 'TarifDebtPay',
                    text: 'Задолженность </br>за период </br>с учетом </br>погашений',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Description',
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Penalties',
                    dataIndex: 'Penalties',
                    text: 'Пени',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PenaltyPayment',
                    dataIndex: 'PenaltyPayment',
                    text: 'Оплата пени',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PenaltyPaymentDate',
                    dataIndex: 'PenaltyPaymentDate',
                    text: 'Дата оплаты пени',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    name: 'AccrualPenalties',
                    dataIndex: 'AccrualPenalties',
                    text: 'Протокол </br>начисления </br>пени',
                    flex: 0.5
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

                                { xtype: 'b4updatebutton' },

                            ],
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});

