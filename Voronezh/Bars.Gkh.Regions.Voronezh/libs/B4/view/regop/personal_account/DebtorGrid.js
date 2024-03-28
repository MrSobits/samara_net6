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
        'B4.enums.YesNo',
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

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.Debtor'),
            claimWorkStore = Ext.create('Ext.data.Store', {
                fields: ['Display', 'Value'],
                data: [
                    {"Display": 'Не ведется', "Value": false},
                    {"Display": 'Ведется', "Value": true}
                ]
            });

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                checkOnly: false
            }),
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    text: 'ПИР',
                    itemId: 'gotoprotocol',
                    action: 'gotoprotocol',
                    width: 50,
                    renderer: function (value, metadata, record) {
                        if (record.get('ClaimworkId') > 0) {
                            this.items[0].icon = B4.Url.content('content/img/icons/arrow_right.png');
                            this.items[0].tooltip = 'Переход в ПИР';
                        } else {
                            this.items[0].icon = B4.Url.content('content/img/icons/delete.png');
                            this.items[0].tooltip = 'ПИР не найден';
                        }
                    }
                },
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
                        emptyItem: {Name: '-'},
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
                        emptyItem: {Name: '-'},
                        url: '/Municipality/ListSettlement'
                    }
                },
                { text: 'Адрес', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerArea',
                    flex: 0.7,
                    text: 'Площадь л/с',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomArea',
                    flex: 0.7,
                    text: 'Площадь помещения',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Лс разделен',
                    flex: 0.7,
                    dataIndex: 'Separate',
                    filter: {
                        xtype: 'combobox',
                        operator: CondExpr.operands.eq,
                        displayField: 'Display',
                        store: B4.enums.YesNo.getStore(),
                        valueField: 'Value'
                    },
                    renderer: function (val) {
                        return val === 10 ? 'Да' : 'Нет';
                    }
                },                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус ЛС',
                    width: 150,
                    renderer: function (val) {
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
                                    field.getStore().insert(0, {Id: null, Name: '-'});
                                }
                            }
                        }
                    },
                    scope: this
                },
                {text: 'Номер ЛС', dataIndex: 'PersonalAccountNum', flex: 1, filter: {xtype: 'textfield'}},
                {text: 'Абонент', dataIndex: 'AccountOwner', flex: 1, filter: {xtype: 'textfield'}},
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
                    renderer: function (val) {
                        return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtBaseTariffSum',
                    flex: 1,
                    text: 'Сумма текущей задолженности',
                    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LastClwDebt',
                    flex: 1,
                    text: 'Сумма последней ПИР',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                { text: 'Период последней ПИР', dataIndex: 'LastPirPeriod', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentsSum',
                    flex: 1,
                    text: 'Оплачено после последней ПИР',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MewClaimDebt',
                    flex: 1,
                    text: 'Новая задолженность',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'DebtDecisionTariffSum',
                //    flex: 1,
                //    text: 'Сумма задолженности по тарифу решения',
                //    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'DebtSum',
                //    flex: 1,
                //    text: 'Сумма задолженности',
                //    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyDebt',
                    flex: 1,
                    text: 'Сумма задолженности по пени',
                    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ExpirationDaysCount',
                //    flex: 1,
                //    text: 'Количество дней просрочки оплаты',
                //    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExpirationMonthCount',
                    flex: 1,
                    text: 'Количество месяцев просрочки оплаты',
                    filter: {xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq}
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    text: 'Тип учреждения',
                //    dataIndex: 'CourtType',
                //    enumName: 'B4.enums.ClaimWork.CourtType',
                //    flex: 1,
                //    filter: true
                //},
                //{
                //    text: 'Краткое наименование учреждения',
                //    dataIndex: 'JurInstitution',
                //    flex: 1,
                //    filter: { xtype: 'textfield' },
                //    hidden: true
                //},
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
                {
                    xtype: 'gridcolumn',
                    text: 'Наличие выписки',
                    flex: 0.7,
                    dataIndex: 'ExtractExists',
                    filter: {
                        xtype: 'combobox',
                        operator: CondExpr.operands.eq,
                        displayField: 'Display',
                        store: B4.enums.YesNo.getStore(),
                        valueField: 'Value'
                    },
                    renderer: function (val) {
                        return val === 10 ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExtractDate',
                    text: 'Дата выписки',
                    format: 'd.m.Y',
                    flex: 0.7
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Данные соответствуют',
                    flex: 0.7,
                    dataIndex: 'AccountRosregMatched',
                    filter: {
                        xtype: 'combobox',
                        operator: CondExpr.operands.eq,
                        displayField: 'Display',
                        store: B4.enums.YesNo.getStore(),
                        valueField: 'Value'
                    },
                    renderer: function(val) {
                        return val === 10 ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'ЛС обрабатывается агентом',
                    flex: 0.7,
                    dataIndex: 'ProcessedByTheAgent',
                    filter: {
                        xtype: 'combobox',
                        operator: CondExpr.operands.eq,
                        displayField: 'Display',
                        store: B4.enums.YesNo.getStore(),
                        valueField: 'Value'
                    },
                    renderer: function (val) {
                        return val === 10 ? 'Да' : 'Нет';
                    }
                }
                //{text: 'Имя пользователя', dataIndex: 'UserName', flex: 1, hidden: true, filter: {xtype: 'textfield'}}
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
                                            renderer: function (val) {
                                                return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val);
                                            }
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
                                            var params = {entityTypeId: 'gkh_regop_personal_account'};
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
                                        {text: 'Наименование', dataIndex: 'Name', flex: 1}
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            title: 'Выписки',
                            height: 100,
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {}
                                ,
                                {
                                    text: 'Получить сведения о выписках',
                                    action: 'UpdateExtractInfo',

                                    iconCls: 'icon-page'
                                }
                                , {}
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
                loadMask: true,
                getRowClass: function (record, index, par) {
                    var underage = record.get('Underage');
                    if (underage) {
                        return 'back-coralpurple';
                    }
                    var extractexists = record.get('AccountRosregMatched');
                    var hasclaimwork = record.get('HasClaimWork');
                    var hasContragent = record.get('ProcessedByTheAgent');
                    var isSeparate = record.get('Separate');
                    
                    if (hasContragent === 10) {
                        return 'back-coralred';
                    }
                    if (extractexists === 10 && hasclaimwork) {
                        return 'back-coralblue';
                    }
                    if (extractexists === 10) {
                        return 'back-coralgreen';
                    }
                    if (hasclaimwork) {
                        return 'back-coralyellow';
                    }
                    if (isSeparate === 10) {
                        return 'back-orange';
                    }
                    return '';
                }
            }
        });

        me.callParent(arguments);
    }
});