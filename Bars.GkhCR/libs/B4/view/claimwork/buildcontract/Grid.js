Ext.define('B4.view.claimwork.buildcontract.Grid', {
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
        'B4.store.claimwork.BuildContract',
        'B4.ux.grid.filter.YesNo',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'B4.enums.BuildContractCreationType',
        'B4.view.Control.GkhButtonPrint'
    ],

    title: 'Подрядчики, нарушившие условия договора',
    cls: 'x-large-head',
    alias: 'widget.buildcontractclaimworkgrid',

    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.BuildContract');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    menuText: 'Статус нарушения',
                    text: 'Статус нарушения',
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
                                options.params.typeId = 'clw_buildctr_claim_work';
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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    width: 160,
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                { text: 'Подрядчик', dataIndex: 'Builder', flex: 1, filter: { xtype: 'textfield' } },
                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                { text: 'Номер договора', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDateFrom',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата договора',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEndWork',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Срок выполнения работ',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountDaysDelay',
                    flex: 1,
                    text: 'Количество дней просрочки',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
                    }
                },
                {
                    text: 'Способ формирования',
                    dataIndex: 'CreationType',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.BuildContractCreationType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) { return B4.enums.BuildContractCreationType.displayRenderer(val); }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsDebtPaid',
                    text: 'Нарушения устранены',
                    flex: 1,
                    maxWidth: 100,
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        store: Ext.create('Ext.data.Store', {
                            fields: ['value', 'name'],
                            data: [
                                { 'value': null, 'name': '-' },
                                { 'value': true, 'name': 'Да' },
                                { 'value': false, 'name': 'Нет' }
                            ]
                        }),
                        valueField: 'value',
                        displayField: 'name',
                        queryMode: 'local'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DebtPaidDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Дата устранения',
                    filter: { xtype: 'datefield' }
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
                                    text: 'Обновить',
                                    textAlign: 'left',
                                    actionName: 'updState',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh'
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
                                    xtype: 'gkhbuttonprint'
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
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});

