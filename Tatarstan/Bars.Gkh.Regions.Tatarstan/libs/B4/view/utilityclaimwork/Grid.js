Ext.define('B4.view.utilityclaimwork.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.form.SelectField',
        'B4.store.utilityclaimwork.UtilityDebtor',
        'B4.enums.OwnerType',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'Ext.ux.grid.FilterBar',

        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    title: 'Реестр неплательщиков ЖКУ',
    cls: 'x-large-head',
    alias: 'widget.utilitydebtorclaimworkgrid',

    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.utilityclaimwork.UtilityDebtor');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
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
                                options.params.typeId = 'clw_utility_debtor_claim_work';
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
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }, 
                {
                    text: 'Абонент',
                    dataIndex: 'AccountOwner',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.OwnerType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operator: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) { return B4.enums.OwnerType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargeDebt',
                    flex: 1,
                    text: 'Сумма долга',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyDebt',
                    flex: 1,
                    text: 'Сумма долга по пени',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    renderer: function (val) {
                        return val == 0 ? '' : val;
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                { 
                                    xtype: 'gkhbuttonimport',
                                    importId: 'Bars.Gkh.Regions.Tatarstan.Import.UtilityDebtor.UtilityDebtorImport'
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
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});