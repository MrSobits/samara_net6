Ext.define('B4.view.claimwork.pretension.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhDecimalField',
        'B4.model.claimwork.DocumentRegister',
        'B4.enums.ClaimWorkTypeBase',
        'B4.enums.DebtorType'
    ],

    cls: 'x-large-head',
    alias: 'widget.pretensiongrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.claimwork.DocumentRegister',
                autoLoad: false,
                listeners: {
                    beforeload: function(store, options) {
                        options.params = options.params || {};
                        Ext.apply(options.params, {
                            documentType: B4.enums.ClaimWorkDocumentType.Pretension
                        });
                    }
                }
            });

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    handler: function(view, rowIndex, colIndex, item, e, record) {
                        var id = record.get('Id'),
                            baseType = record.get('BaseType'),
                            debtorType = record.get('DebtorType'),
                            clwId = record.get('ClaimWorkId'),
                            controllerType = 'claimwork',
                            typeBase,
                            url;

                        if (baseType === B4.enums.ClaimWorkTypeBase.Debtor) {
                            switch (debtorType) {
                                case B4.enums.DebtorType.Individual:
                                    typeBase = 'IndividualClaimWork';
                                    break;
                                case B4.enums.DebtorType.Legal:
                                    typeBase = 'LegalClaimWork';
                                    break;
                                default:
                                    Ext.Msg.alert('Ошибка', 'Не удалось определить тип должника');
                                    return;
                            }
                        } else {
                            controllerType = 'claimworkbc';
                            typeBase = 'BuildContractClaimWork';
                        }

                        url = Ext.String.format('{0}/{1}/{2}/{3}/pretension', controllerType, typeBase, clwId, id);
                        Ext.History.add(url);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'BaseType',
                    text: 'Тип основания',
                    enumName: 'B4.enums.ClaimWorkTypeBase',
                    flex: 2,
                    filter: true
                },
                {
                    dataIndex: 'BaseInfo',
                    text: 'Основание',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата формирования',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ReviewDate',
                    text: 'Дата рассмотрения',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    dataIndex: 'PretensSum',
                    flex: 1,
                    decimalSeparator: ',',
                    text: 'Сумма претензии (основной долг) (руб.)',
                    allowNegative: false,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    dataIndex: 'PretensPenalty',
                    flex: 1,
                    decimalSeparator: ',',
                    text: 'Пени (руб.)',
                    allowNegative: false,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PretensPlanedDate',
                    itemId: 'cwcPaymentPlannedPeriod',
                    text: 'Планируемый срок оплаты',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: false,
                    sortable: false
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    var paymentPlannedPeriodDate = new Date(record.get('PretensPlanedDate')),
                        add5DaysFromToday = Ext.Date.add(new Date(), Ext.Date.DAY, +5),
                        today = new Date(),
                        result = '';

                    if (paymentPlannedPeriodDate <= add5DaysFromToday) {
                        result = 'back-strawyellow';
                    }
                    if (paymentPlannedPeriodDate <= today) {
                        result = 'back-pink';
                    }
                    return result;
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    action: 'export',
                                    text: 'Экспорт',
                                    textAlign: 'left'
                                }
                            ]
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
