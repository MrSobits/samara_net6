Ext.define('B4.view.chargessplitting.contrpersumm.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.view.Control.GkhButtonPrint',

        'B4.view.chargessplitting.contrpersumm.FilterPanel'
    ],

    title: 'Договоры ресурсоснабжения (УО)',
    alias: 'widget.contractperiodsummarygrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.chargessplitting.contrpersumm.ContractPeriodSumm');

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UoState',
                    text: 'Статус УО',
                    sortable: false,
                    renderer: function (val) {
                        return val['Name'];
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RsoState',
                    text: 'Статус РСО',
                    sortable: false,
                    renderer: function (val) {
                        return val['Name'];
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManagingOrganization',
                    text: 'Управляющая организация'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublicServiceOrg',
                    text: 'Ресурсоснабжающая организация'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    text: 'Услуга'
                },
                {
                    text: 'Управляющая организация',
                    name: 'ManOrgGroup',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'StartDebt',
                            text: 'Входящее сальдо (Долг на начало месяца)'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedResidents',
                            text: 'Начислено жителям'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RecalcPrevPeriod',
                            text: 'Сумма перерасчета начисления за предыдущий период'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChangeSum',
                            text: 'Сумма изменений (перекидки)'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'NoDeliverySum',
                            text: 'Сумма учтеной  недопоставки'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PaidResidents',
                            text: 'Оплачено жителями'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'EndDebt',
                            text: 'Исходящее сальдо (Долг на конец месяца)'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedToPay',
                            text: 'Начислено к оплате'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TransferredPubServOrg',
                            text: 'Перечислено РСО'
                        }
                    ]
                },
                {
                    text: 'Ресурсоснабжающая организация',
                    name: 'PubServOrgGroup',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ChargedManOrg',
                            text: 'Начислено УО'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PaidManOrg',
                            text: 'Оплачено УО'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'SaldoOut',
                            text: 'Исходящее сальдо'
                        }
                    ]
                }
            ],

            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    name:'buttons',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отчетные периоды',
                                    action: 'GoToPeriods'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Операции',
                                    iconCls: 'icon-cog-go',
                                    action: 'Operations',
                                    menu: {
                                        items: [
                                            {
                                                text: 'Импорт',
                                                action: 'Import',
                                                importId: 'Bars.Gkh.Regions.Tatarstan.Import.PubServContractImport',
                                                iconCls: 'icon-page-white-text',
                                                possibleFileExtensions: 'csv'
                                            },
                                            {
                                                text: 'Экспорт',
                                                action: 'Export',
                                                iconCls: 'icon-page-white-put'
                                            }
                                        ]
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    name: 'filters',
                    items: [
                        {
                            xtype: 'contrpersummfilterpanel'
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
        });

        me.callParent(arguments);
    }
});