Ext.define('B4.view.import.chesimport.payments.AssignedGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.chesimportpaymentsassignedgrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.ImportPaymentType',
        'B4.enums.ChesImportPaymentsState',
        'B4.enums.YesNo',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar',
        'B4.ux.button.Update',
        'B4.ux.grid.filter.YesNo',
        'B4.store.import.chesimport.payments.Assigned'
    ],

    
    columnLines: true,
    title: 'Сопоставленные оплаты',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.payments.Assigned'),
            numberFilter = {
                xtype: 'numberfield',
                allowDecimals: true,
                hideTrigger: true,
                minValue: Number.NEGATIVE_INFINITY,
                operand: CondExpr.operands.eq
            };

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    text: 'ID оплаты',
                    dataIndex: 'RegistryNum',
                    flex: 0.8,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС БАРС',
                    dataIndex: 'LsNum',
                    flex: 0.8,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата операции',
                    dataIndex: 'PaymentDate',
                    format: 'd.m.Y',
                    width: 120,
                    filter: { xtype: 'datefield' }
                },
                {
                    text: 'Сумма по базовому тарифу',
                    dataIndex: 'TariffPayment',
                    flex: 1,
                    filter: numberFilter
                },
                {
                    text: 'Сумма по тарифу решения',
                    dataIndex: 'TariffDecisionPayment',
                    flex: 1,
                    filter: numberFilter
                },
                {
                    text: 'Сумма по пени',
                    dataIndex: 'PenaltyPayment',
                    flex: 1,
                    filter: numberFilter
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ImportPaymentType',
                    filter: true,
                    text: 'Тип оплаты',
                    dataIndex: 'PaymentType',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата документа/реестра',
                    dataIndex: 'RegistryDate',
                    format: 'd.m.Y',
                    width: 120,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата выгрузки оплаты',
                    dataIndex: 'ReportDate',
                    format: 'd.m.Y',
                    width: 120,
                    filter: { xtype: 'datefield' }
                },
                {
                    text: 'Версия файла',
                    dataIndex: 'Version',
                    width: 85,
                    filter: numberFilter
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ChesImportPaymentsState',
                    filter: true,
                    text: 'Статус оплаты',
                    dataIndex: 'State',
                    width: 150
                },
                {
                    width: 90,
                    dataIndex: 'IsImported',
                    text: 'Импортирован',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: {
                        xtype: 'b4dgridfilteryesno',
                        operator: 'eq'
                    }
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
                                    xtype: 'b4updatebutton',
                                    handler: function (button) {
                                        button.up('grid').getStore().load();
                                    }
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить оплаты',
                                    iconCls: 'icon-application-go',
                                    action: 'import'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
