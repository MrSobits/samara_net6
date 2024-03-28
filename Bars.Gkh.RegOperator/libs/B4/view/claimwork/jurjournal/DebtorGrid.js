Ext.define('B4.view.claimwork.jurjournal.DebtorGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.store.claimwork.JurJournalDebtor'
    ],

    cls: 'x-large-head',
    alias: 'widget.jjdebtorgrid',

    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.JurJournalDebtor');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        var id = record.get('Id'),
                            clwId = record.get('ClwId'),
                            ownerType = record.get('OwnerType'),
                            url = '';

                        if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Legal) {
                            url = Ext.String.format('claimwork/LegalClaimWork/{0}/{1}/lawsuit', clwId, id);
                        } else if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Individual) {
                            url = Ext.String.format('claimwork/IndividualClaimWork/{0}/{1}/lawsuit', clwId, id);
                        } else {
                            return;
                        }

                        Ext.History.add(url);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'OwnerType',
                    text: 'Тип абонента',
                    enumName: 'B4.enums.regop.PersonalAccountOwnerType',
                    filter: true
                },
                {
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'OwnerName',
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
                        hideTrigger: true
                    }
                },
                {
                    dataIndex: 'PenaltyDebtApproved',
                    text: 'Сумма признанной задолженности (пени)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
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
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Objection',
                    width: 70,
                    text: 'Поступило возражение',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
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
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Погашено до решения суда',
                    dataIndex: 'RepaidBeforeDecision',
                    enumName: 'B4.enums.claimwork.RepaymentType',
                    filter: true,
                    hidden: true
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Погашено до исполнительного производства',
                    dataIndex: 'RepaidBeforeExecutionProceedings',
                    enumName: 'B4.enums.claimwork.RepaymentType',
                    filter: true,
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
                                        'click': function () {
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