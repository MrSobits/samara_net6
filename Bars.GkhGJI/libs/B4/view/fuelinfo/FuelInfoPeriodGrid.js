Ext.define('B4.view.fuelinfo.FuelInfoPeriodGrid', {

    extend: 'B4.ux.grid.Panel',

    title: 'Сведения о наличии и расходе топлива',

    alias: 'widget.fuelinfoperiodgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.enums.Month'
    ],

    closable: true,

    initComponent: function () {
        var me = this;

        var store = Ext.create('B4.store.fuelinfo.FuelInfoPeriod');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Год',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq,
                        maskRe: /\d/,
                        name: 'Year'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Month',
                    text: 'Месяц',
                    flex: 2,
                    renderer: function (val) {
                        return B4.enums.Month.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display',
                        editable: false,
                        store: B4.enums.Month.getStore(),
                        emptyItem: { Display: '-' },
                        name: 'Month'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    flex: 5,
                    renderer: function (val) { return val.Name },
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
                            border: 'false',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Добавить новый'
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