Ext.define('B4.view.claimwork.AccountDetailGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [

           'Ext.ux.CheckColumn',
           'B4.ux.grid.column.Edit',
           'B4.ux.grid.toolbar.Paging',
           'B4.ux.grid.plugin.HeaderFilters',
           'B4.form.GridStateColumn',
           'B4.view.Control.GkhButtonPrint',
           'B4.form.ComboBox',
           'B4.store.dict.Municipality',
           'B4.store.claimwork.AccountDetail'
           ],

    title: 'Список лицевых счетов абонента', 
    alias: 'widget.claimworkaccountdetailgrid',  

    minHeight: 400,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.AccountDetail');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'actioncolumn',
                    text: 'Переход к ЛС',
                    iconCls: 'icon-zoom',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'goToAccount', record);
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
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersAccState',
                    menuText: 'Статус ЛС',
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
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    scope: this
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
                    text: 'Сумма текущей задолженности',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CurrPenaltyDebt',
                    flex: 1,
                    text: 'Сумма текущей задолженности по пени',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountDaysDelay',
                    flex: 1,
                    text: 'Количество дней просрочки оплаты',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountMonthDelay',
                    flex: 1,
                    text: 'Количество месяцев просрочки оплаты',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'PrintAccountReport',
                    text: 'Отчет по ЛС',
                    iconCls: 'icon-arrow-right',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'printAccountReport', record);
                    }
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'PrintAccountClaimworkReport',
                    text: 'Отчет по ЛС (ПИР)',
                    iconCls: 'icon-arrow-right',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'printAccountClaimworkReport', record);
                    }
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{
                                xtype: 'gkhbuttonprint',
                                action: 'Print',
                                name: 'Print'
                            }
                            ]
                        }
                    ]
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

