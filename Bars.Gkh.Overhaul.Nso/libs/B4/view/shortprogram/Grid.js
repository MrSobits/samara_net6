Ext.define('B4.view.shortprogram.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.shortprogramgrid',

    requires: [
        'B4.form.ComboBox',
        'CondExpr',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Краткосрочная программа',
    store: 'ShortProgramRecord',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
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
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес МКД',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'PlanYear',
                    flex: 1,
                    text: 'Срок ремонта (год)',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: false,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'CeoName',
                    flex: 1,
                    text: 'Вид ремонта',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Стоимость ремонта (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BudgetOwners',
                    flex: 1,
                    text: 'Из средств собственников (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BudgetRegion',
                    flex: 1,
                    text: 'Из регионального бюджета (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BudgetMunicipality',
                    flex: 1,
                    text: 'Из муниципального бюджета (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BudgetFcr',
                    flex: 1,
                    text: 'Из ГК ФСР ЖКХ (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BudgetOtherSource',
                    flex: 1,
                    text: 'Из иных источников (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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
            ]
        });
        me.callParent(arguments);
    }
});