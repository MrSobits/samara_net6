Ext.define('B4.view.regop.personal_account.DebtorGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'Ext.ux.CheckColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.regop.personal_account.Debtor',
        'B4.store.StateByType',
        'B4.store.regop.owner.PersonalAccountOwner',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.store.regop.ChargePeriod',
        'B4.ux.grid.filter.YesNo',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'Ext.ux.grid.FilterBar',
        'B4.enums.ClaimWork.CourtType',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Реестр должников',
    cls: 'x-large-head',
    alias: 'widget.debtorgrid',

    closable: true,
    enableColumnHide: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.Debtor'),
            claimWorkStore = Ext.create('Ext.data.Store', {
                fields: ['Display', 'Value'],
                data: [
                    { "Display": 'Не ведется', "Value": false },
                    { "Display": 'Ведется', "Value": true }
                ]
            });

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    width: 160,
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlement'
                    }
                },
                { text: 'Адрес', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус ЛС',
                    width: 150,
                    renderer: function(val) {
                        return val && val.Name ? val.Name : '';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    scope: this
                },
                { text: 'Номер ЛС', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } },
                { text: 'Абонент', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } },
                {
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.regop.PersonalAccountOwnerType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operator: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) { return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerArea',
                    flex: 1,
                    text: 'Площадь в собственности',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtBaseTariffSum',
                    flex: 1,
                    text: 'Сумма задолженности по базовому тарифу',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtDecisionTariffSum',
                    flex: 1,
                    text: 'Сумма задолженности по тарифу решения',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtSum',
                    flex: 1,
                    text: 'Сумма задолженности',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyDebt',
                    flex: 1,
                    text: 'Сумма задолженности по пени',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExpirationDaysCount',
                    flex: 1,
                    text: 'Количество дней просрочки оплаты',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExpirationMonthCount',
                    flex: 1,
                    text: 'Количество месяцев просрочки оплаты',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Тип учреждения',
                    dataIndex: 'CourtType',
                    enumName: 'B4.enums.ClaimWork.CourtType',
                    flex: 1,
                    filter: true
                },
                {
                    text: 'Краткое наименование учреждения',
                    dataIndex: 'JurInstitution',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasClaimWork',
                    flex: 1,
                    text: 'Претензионная работа',
                    filter: {
                        xtype: 'combobox',
                        operator: CondExpr.operands.eq,
                        displayField: 'Display',
                        store: claimWorkStore,
                        valueField: 'Value'
                    },
                    renderer: function (val) {
                        return val === true ? 'Ведется' : 'Не ведется';
                    }
                },
                { text: 'Имя пользователя', dataIndex: 'UserName', flex: 1, hidden: true, filter: { xtype: 'textfield' } }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    itemId: 'toptoolbar',
                    overflowX: 'auto',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            title: 'Действия',
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    text: 'Сформировать ',
                                    action: 'Create',
                                    iconCls: 'icon-page-forward'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    text: 'Очистить реестр',
                                    action: 'Clear',
                                    iconCls: 'icon-page-forward'
                                },
                                {
                                    text: 'Начать претензионную работу',
                                    action: 'CreateClaimWork',
                                    iconCls: 'icon-page-forward'
                                },
                                {
                                    text: 'Обновить судебные учреждения',
                                    action: 'UpdateJurInstitution',
                                    iconCls: 'icon-arrow-refresh'
                                },
                                {
                                    text: 'Создать ПИРы по списку номеров ЛС',
                                    action: 'CreateClaimWorksByAccNum',
                                    iconCls: 'icon-table'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            columns: 2,
                            height: 100,
                            defaults: {
                                margin: 2,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Муниципальный район',
                                    name: 'Municipality',
                                    store: 'B4.store.dict.municipality.MoArea',
                                    editable: false,
                                    labelWidth: 150,
                                    selectionMode: 'MULTI',
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.owner.PersonalAccountOwner',
                                    name: 'Owner',
                                    selectionMode: 'MULTI',
                                    fieldLabel: 'Абонент',
                                    editable: false,
                                    columns: [
                                        {
                                            text: 'Тип абонента',
                                            dataIndex: 'OwnerType',
                                            flex: 0.8,
                                            filter: {
                                                xtype: 'b4combobox',
                                                items: B4.enums.regop.PersonalAccountOwnerType.getItemsWithEmpty([null, '-']),
                                                editable: false,
                                                operator: CondExpr.operands.eq,
                                                valueField: 'Value',
                                                displayField: 'Display'
                                            },
                                            renderer: function(val) { return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val); }
                                        },
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]  
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.StateByType',
                                    fieldLabel: 'Статус',
                                    name: 'State',
                                    editable: false,
                                    labelWidth: 150,
                                    selectionMode: 'MULTI',
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }],
                                    listeners: {
                                        beforeload: function (store, options) {
                                            var params = { entityTypeId: 'gkh_regop_personal_account' };
                                            Ext.apply(options.params, params);
                                        }  
                                    }
                                },
                               {
                                   xtype: 'b4selectfield',
                                   store: 'B4.store.dict.ProgramCr',
                                   name: 'ProgramCr',
                                   selectionMode: 'MULTI',
                                   fieldLabel: 'Программа КР',
                                   editable: false,
                                   columns: [
                                       { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                   ]
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