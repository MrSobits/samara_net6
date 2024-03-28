Ext.define('B4.view.claimwork.actviolidentification.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.claimwork.ActViolIdentification',
        'B4.enums.ClaimWorkTypeBase',
        'B4.enums.FactOfSigning',
        'B4.enums.DebtorType'
    ],

    cls: 'x-large-head',
    alias: 'widget.actviolidentifgrid',

    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.ActViolIdentification');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        var id = record.get('Id'),
                            claimWork = record.get('ClaimWork'),
                            clwId = claimWork.Id || 0,
                            debtorType = claimWork.DebtorType || 0,
                            controllerType = 'claimwork',
                            typeBase,
                            url;

                        if (record.get('ClaimWorkTypeBase') === B4.enums.ClaimWorkTypeBase.Debtor) {
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

                        url = Ext.String.format('{0}/{1}/{2}/{3}/actviolidentification', controllerType, typeBase, clwId, id);
                        Ext.History.add(url);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ClaimWorkTypeBase',
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
                    dataIndex: 'FormDate',
                    text: 'Дата составления',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'FactOfSigning',
                    text: 'Факт подписания',
                    enumName: 'B4.enums.FactOfSigning',
                    flex: 1,
                    filter: true
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
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