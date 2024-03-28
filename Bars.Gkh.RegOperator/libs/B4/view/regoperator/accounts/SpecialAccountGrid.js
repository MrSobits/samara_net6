Ext.define('B4.view.regoperator.accounts.SpecialAccountGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.regopspecaccgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.calcaccount.Special'
    ],

    title: 'Специальные счета',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.calcaccount.Special');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountNumber',
                    width: 140,
                    text: 'Номер счета',
                    filter: {
                        xtype: 'textfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentCreditOrg',
                    flex: 1,
                    text: 'Кредитная организация',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
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
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DateOpen',
                    width: 100,
                    text: 'Дата открытия',
                    renderer: function (v) {
                        if (!v || new Date(v).getFullYear() === 1) {
                            return null;
                        }
                        return Ext.Date.format(new Date(v), "d.m.Y");
                    },
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateClose',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата закрытия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Debt',
                    width: 140,
                    text: 'Итого по Дебету',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Credit',
                    width: 140,
                    text: 'Итого по кредиту',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Saldo',
                    width: 140,
                    text: 'Сальдо',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
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
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        me.getStore().load();
                                    }
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
            ]
        });

        me.callParent(arguments);
    }
});