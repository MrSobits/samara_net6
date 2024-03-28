Ext.define('B4.view.claimwork.IndividualGrid', {
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
        'B4.store.claimwork.IndividualClaimWork',
        'B4.ux.grid.filter.YesNo',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'B4.view.Control.GkhButtonPrint',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Реестр неплательщиков-физ.лиц',
    cls: 'x-large-head',
    alias: 'widget.individualclaimworkgrid',

    closable: true,
    enableColumnHide: false,
    store: 'claimwork.IndividualClaimWork',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 0.5,
                    text: 'Код'
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
                    text: 'Адрес прописки',
                    dataIndex: 'RegistrationAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адреса ЛС',
                    dataIndex: 'AccountsAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                    text: 'Сумма текущей задолженности по базовому тарифу',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrChargeDecisionTariffDebt',
                    flex: 1,
                    text: 'Сумма текущей задолженности по тарифу решения',
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
                                    iconCls: 'icon-page-refresh',
                                    text: 'Эталонный расчёт',
                                    textAlign: 'left',
                                    actionName: 'btnCalcDebtDate'
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
                                    itemId: 'cbShowPausedClw',
                                    boxLabel: 'Показать приостановленные',
                                    labelAlign: 'right',
                                    checked: false
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    fieldLabel: 'Дата с',
                                    labelAlign: 'right',
                                    width: 150,
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    labelAlign: 'right',
                                    fieldLabel: 'по',
                                    width: 130,
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    itemId: 'btupdateState',
                                    textAlign: 'left',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-page-refresh'
                                }
                            ]
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
            //plugins: [
            //    Ext.create('B4.ux.grid.plugin.HeaderFilters'),
            //    {
            //        ptype: 'filterbar',
            //        renderHidden: false,
            //        showShowHideButton: false,
            //        showClearAllButton: false
            //    }
            //],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var charge185fzexists = record.get('HasCharges185FZ');
                    var minshare = record.get("MinShare");
                    var underage = record.get("Underage");
                    if (underage) {
                        return 'back-coralblue';
                    }
                    if (minshare < 1) {
                        return 'back-coralyellow';
                    }
                    if (charge185fzexists) {
                        return 'back-coralred';
                    }
                    return;
                }
            }

        });

        me.callParent(arguments);
    }
});