Ext.define('B4.view.regop.special_account.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox',

        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.grid.feature.Summary',

        'B4.store.calcaccount.SpecialRegister'
    ],

    title: 'Реестр специальных счетов',

    alias: 'widget.specaccgrid',

    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.calcaccount.SpecialRegister');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Номер счета',
                    filter: {
                        xtype: 'textfield'
                    },
                    dataIndex: 'AccountNumber',
                    flex: 1,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Муниципальный район',
                    dataIndex: 'Municipality',
                    flex: 1,
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
                    filter: {
                        xtype: 'textfield'  
                    },
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 1
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    text: 'Начислено',
                    dataIndex: 'ChargeTotal',
                    flex: 1,
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    text: 'Уплачено',
                    dataIndex: 'PaidTotal',
                    flex: 1,
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    text: 'Задолженность',
                    dataIndex: 'Debt',
                    flex: 1,
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    text: 'Сальдо',
                    dataIndex: 'Saldo',
                    flex: 1,
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    filter: {
                        xtype: 'textfield'
                    },
                    text: 'Владелец счета',
                    dataIndex: 'AccountOwner',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                //{
                                //    xtype: 'b4addbutton'
                                //},
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        //{
                        //    xtype: 'datefield',
                        //    fieldLabel: 'Период с',
                        //    labelWidth: 70
                        //},
                        //{
                        //    xtype: 'datefield',
                        //    fieldLabel: 'по',
                        //    labelWidth: 30
                        //},
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Показать все счета',
                            action: 'showall'
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
            features: [
                {
                    ftype: 'b4_summary'
                }
            ]
        });

        me.callParent(arguments);
    }
});