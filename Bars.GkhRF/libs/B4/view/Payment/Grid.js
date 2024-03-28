Ext.define('B4.view.payment.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhButtonImport',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality'
    ],

    title: 'Реестр оплат капитального ремонта',
    store: 'Payment',
    alias: 'widget.paymentGrid',
    closable: true,
    features: [{
        ftype: 'summary'
    }],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
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
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePopulationCr',
                    flex: 1,
                    text: 'Капрем. начисл.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function(val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidPopulationCr',
                    flex: 1,
                    text: 'Капрем. опл',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePopulationHireRf',
                    flex: 1,
                    text: 'Найм начисл.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidPopulationHireRf',
                    flex: 1,
                    text: 'Найм оплач.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePopulationCr185',
                    flex: 1,
                    text: '185ФЗ Начисл.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidPopulationCr185',
                    flex: 1,
                    text: '185ФЗ Оплач.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePopulationBldRepair',
                    flex: 1,
                    text: 'Рем.здания начисл.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcPaidPopulationBldRepair',
                    dataIndex: 'PaidPopulationBldRepair',
                    flex: 1,
                    text: 'Рем.здания оплач.',
                    hidden: true,
                    summaryType: 'sum',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryRenderer: function (val) {
                        return Ext.util.Format.round(val, 2);
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowSum',
                                    checked: false,
                                    boxLabel: 'Показать суммы',
                                    width: 110

                                },
                                {
                                    margin: '0 0 0 20px',
                                    xtype: 'gkhbuttonimport'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnPaymentExport'
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