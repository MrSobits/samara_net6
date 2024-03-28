Ext.define('B4.view.claimwork.LegalGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'Ext.ux.CheckColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.enums.DebtorState',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.claimwork.LegalClaimWork',
        'B4.enums.ContragentState',
        'B4.ux.grid.filter.YesNo',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'B4.view.Control.GkhButtonPrint',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Реестр неплательщиков-юр.лиц',
    cls: 'x-large-head',
    alias: 'widget.legalclaimworkgrid',

    closable: true,
    enableColumnHide: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.LegalClaimWork');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DebtorState',
                    filter: true,
                    text: 'Статус',
                    dataIndex: 'DebtorState',
                    width: 180
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 180,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    text: 'Абонент',
                    dataIndex: 'AccountOwnerName',
                    flex: 1.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Юридический адрес',
                    dataIndex: 'JuridicalAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'ИНН',
                    dataIndex: 'Inn',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'КПП',
                    dataIndex: 'Kpp',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentState',
                    flex: 1,
                    text: 'Статус',
                    renderer: function(val) {
                        return B4.enums.ContragentState.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ContragentState.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        operator: 'eq',
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    text: 'Количество ЛС',
                    dataIndex: 'AccountsNumber',
                    flex: 0.5,
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrChargeBaseTariffDebt',
                    flex: 1,
                    text: 'Cумма текущей задолженности по базовому тарифу',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrChargeDecisionTariffDebt',
                    flex: 1,
                    text: 'Cумма текущей задолженности по тарифу решения',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrChargeDebt',
                    flex: 1,
                    text: 'Общая сумма текущей задолженности',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrPenaltyDebt',
                    flex: 1,
                    text: 'Общая сумма текущей задолженности по пени',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsDebtPaid',
                    text: 'Задолженность погашена',
                    flex: 1,
                    maxWidth: 100,
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno', operator: 'eq' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DebtPaidDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Дата погашения',
                    filter: { xtype: 'datefield' }
                },
                {
                    dataIndex: 'UserName',
                    flex: 1,
                    text: 'Имя пользователя',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    xtype: 'button',
                                    text: 'Обновить реестр',
                                    textAlign: 'left',
                                    actionName: 'updateState',
                                    tooltip: 'Обновление статусов и задолженностей',
                                    iconCls: 'icon-page-refresh'
                                },
                                {
                                    xtype: 'button',
                                    actionName: 'CreateDoc',
                                    text: 'Сформировать',
                                    iconCls: 'icon-cog-go',
                                    menu: { items: [] }
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    actionName: 'Print'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'ShowDebt',
                                    itemId:'ShowDebtId',
                                    boxLabel: 'Наличие долга',
                                    checked: false
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});