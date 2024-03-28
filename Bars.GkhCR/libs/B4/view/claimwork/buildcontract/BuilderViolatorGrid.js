Ext.define('B4.view.claimwork.buildcontract.BuilderViolatorGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'Ext.ux.CheckColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo',
        
        'B4.store.dict.Municipality',
        'B4.store.claimwork.BuilderViolator',
        
        'B4.view.Control.GkhButtonImport',
        'B4.enums.BuildContractCreationType'
    
    ],

    title: 'Подрядчики, нарушившие условия договора',
    cls: 'x-large-head',
    alias: 'widget.builderviolatorgrid',

    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.BuilderViolator');

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
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
                        url: '/Municipality/ListSettlementWithoutPaging'
                    }
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
                    filter: { xtype: 'numberfield', hideTrigger: true },
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
                    falseText: 'Не ведется',
                    trueText: 'Ведется',
                    dataIndex: 'IsClaimWorking',
                    flex: 1,
                    text: 'Претензионная работа',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        store: Ext.create('Ext.data.Store', {
                            fields: ['value', 'name'],
                            data: [
                                { 'value': true, 'name': 'Ведется' },
                                { 'value': false, 'name': 'Не ведется' }
                            ]
                        }),
                        valueField: 'value',
                        displayField: 'name',
                        queryMode: 'local'
                    }
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
                                    actionName: 'builderViolatorAdd',
                                    iconCls: 'icon-accept',
                                    text: 'Добавить'
                                },
                                {
                                    text: 'Сформировать ',
                                    action: 'Create',
                                    iconCls: 'icon-page-forward'
                                },
                                {
                                    text: 'Очистить реестр',
                                    action: 'Clear',
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
                                    text: 'Начать претензионную работу',
                                    action: 'CreateClaimWork',
                                    iconCls: 'icon-page-forward'
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

