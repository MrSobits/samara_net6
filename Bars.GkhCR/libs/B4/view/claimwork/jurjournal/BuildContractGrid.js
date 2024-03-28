Ext.define('B4.view.claimwork.jurjournal.BuildContractGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.claimwork.JurJournalBuildContract',
        
        'B4.enums.LawsuitResultConsideration',
        'B4.enums.LawsuitDocumentType'
    ],

    cls: 'x-large-head',
    alias: 'widget.jjbuildcontractgrid',

    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.JurJournalBuildContract');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        var id = record.get('Id'),
                            clwId = record.get('ClwId'),
                            url = Ext.String.format('claimwork/BuildContractClaimWork/{0}/{1}/lawsuit', clwId, id);
                        Ext.History.add(url);
                    }
                },
                {
                    dataIndex: 'ContractNum',
                    text: 'Номер договора',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ContractDate',
                    text: 'Дата договора',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Builder',
                    text: 'ФИО/Наименование ЮЛ',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'LawsuitDocNumber',
                    text: 'Номер заявления',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LawsuitDocDate',
                    text: 'Дата заявления',
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
                    dataIndex: 'WhoConsidered',
                    text: 'Кем рассмотрено',
                    enumName: 'B4.enums.LawsuitConsiderationType',
                    filter: true
                },
                {
                    dataIndex: 'JurSectorNumber',
                    text: 'Наименование суда',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOfAdoption',
                    text: 'Дата принятия заявления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOfReview',
                    text: 'Дата рассмотрения заявления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ResultConsideration',
                    text: 'Результат рассмотрения',
                    enumName: 'B4.enums.LawsuitResultConsideration',
                    filter: true
                },
                {
                    dataIndex: 'DebtSumApproved',
                    text: 'Сумма признанной задолженности (основной долг)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        flex: 1,
                        hideTrigger: true
                    }
                },
                {
                    dataIndex: 'PenaltyDebtApproved',
                    text: 'Сумма признанной задолженности (пени)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        flex: 1,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Тип документа',
                    dataIndex: 'LawsuitDocType',
                    enumName: 'B4.enums.LawsuitDocumentType',
                    filter: true
                },
                {
                    dataIndex: 'NumberConsideration',
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateConsideration',
                    text: 'Дата документа',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'CbDebtSum',
                    text: 'Погашено до исполнит. производства (основной долг)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'CbPenaltyDebtSum',
                    text: 'Погашено до исполнит. производства (пени)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PretensionDocDate',
                    text: 'Дата формирования претензии',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    hidden: true
                },
                {
                    dataIndex: 'PretensionDebtSum',
                    text: 'Сумма претензии (основной долг)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    hidden: true
                },
                {
                    dataIndex: 'PretensionPenaltyDebtSum',
                    text: 'Сумма претензии (пени)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    hidden: true
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
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'export'
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