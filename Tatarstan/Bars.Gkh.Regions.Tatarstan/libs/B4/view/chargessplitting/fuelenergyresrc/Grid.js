Ext.define('B4.view.chargessplitting.fuelenergyresrc.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhButtonPrint',

        'B4.view.chargessplitting.fuelenergyresrc.FilterPanel'
    ],

    title: 'Договоры ТЭР',
    alias: 'widget.fuelenergyresourcecontractgrid',

    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.chargessplitting.fuelenergyresrc.FuelEnergyOrgContractDetail'),
            editIcon = B4.Url.content('content/img/icons/pencil.png'),
            editTooltip = 'Редактировать';

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me,
                    renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                        var me = this,
                            prevRec = store.getAt(rowIndex - 1),
                            column = me.columns[colIndex];

                        if (column) {
                            column.icon = (prevRec && record.get('PeriodSummId') === prevRec.get('PeriodSummId')) ? '' : editIcon;
                            column.tooltip = (prevRec && record.get('PeriodSummId') === prevRec.get('PeriodSummId')) ? '' : editTooltip;
                        }

                        return value;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 2,
                    renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                        var prevRec = store.getAt(rowIndex - 1);

                        return (prevRec && record.get('PeriodSummId') === prevRec.get('PeriodSummId')) ? '' : value;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublicServiceOrg',
                    text: 'Ресурсоснабжающая организация',
                    flex: 2,
                    renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                        var prevRec = store.getAt(rowIndex - 1);

                        return (prevRec && record.get('PeriodSummId') === prevRec.get('PeriodSummId')) ? '' : value;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    text: 'Услуга',
                    flex: 1
                },
                {
                    text: 'Ресурсоснабжающая организация',
                    name: 'PubServOrgGroup',
                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Charged',
                            text: 'Начислено за месяц'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Paid',
                            text: 'Оплачено за месяц'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Debt',
                            text: 'Задолженность на конец месяца'
                        }
                    ]
                },
                {
                    text: 'ТЭР',
                    name: 'FuelEnergyreSourceGroup',
                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'GasEnergyPercents',
                            text: 'Процент ГАЗ'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ElectricityEnergyPercents',
                            text: 'Процент Э/Э'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PlanPayGas',
                            text: 'Планируемая оплата ГАЗ'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PlanPayElectricity',
                            text: 'Планируемая оплата Э/Э'
                        }
                    ]
                }
            ],

            viewConfig: {
                loadMask: true,
                getRowClass: function (record, rowIndex, rowParams, store) {
                    var cls = '',
                        nextRec = store.getAt(rowIndex + 1);

                    if (nextRec && record.get('PeriodSummId') !== nextRec.get('PeriodSummId')) {
                        cls = 'underlined-grid-row';
                    }

                    return cls;
                }
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
                                                importId: 'Bars.Gkh.Regions.Tatarstan.Import.FuelEnergyOrgContractImport',
                                                iconCls: 'icon-page-white-text',
                                                possibleFileExtensions: 'csv'
                                            }
                                        ]
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать сведения',
                                    action: 'Actualize'
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
                            xtype: 'fuelenergyresourcecontractfilterpanel'
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