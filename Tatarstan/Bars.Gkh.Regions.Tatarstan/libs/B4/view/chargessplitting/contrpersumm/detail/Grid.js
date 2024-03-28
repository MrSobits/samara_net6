Ext.define('B4.view.chargessplitting.contrpersumm.detail.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],

    alias: 'widget.contractperiodsummarydetailgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.chargessplitting.contrpersumm.ContractPeriodSummDetail');

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес'
                },
                {
                    text: 'УО',
                    name: 'ManOrgGroup',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'StartDebt',
                            text: 'Входящее сальдо (Долг на начало месяца)',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedResidents',
                            text: 'Начислено жителям',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RecalcPrevPeriod',
                            text: 'Сумма перерасчета начисления за предыдущий период',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChangeSum',
                            text: 'Сумма изменений (перекидки)',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'NoDeliverySum',
                            text: 'Сумма учтеной  недопоставки',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PaidResidents',
                            text: 'Оплачено жителями',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'EndDebt',
                            text: 'Исходящее сальдо (Долг на конец месяца)',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedToPay',
                            text: 'Начислено к оплате',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TransferredPubServOrg',
                            text: 'Перечислено РСО',
                            editor: { xtype: 'gkhdecimalfield' }
                        }
                    ]
                },
                {
                    text: 'РСО',
                    name: 'PubServOrgGroup',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedManOrg',
                            text: 'Начислено УО',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PaidManOrg',
                            text: 'Оплачено УО',
                            editor: { xtype: 'gkhdecimalfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'SaldoOut',
                            text: 'Исходящее сальдо',
                            editor: { xtype: 'gkhdecimalfield' }
                        }
                    ]
                }
            ],
            plugins:[
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        'beforeedit': function (editor, eventArgs, eventOpts) {
                            return eventArgs.column.allowEdit;
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});