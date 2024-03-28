Ext.define('B4.view.dict.service.BilTarifStorageGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.MonthPicker',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Тариф',
    alias: 'widget.biltarifstoragedictgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.service.BilTarifStorage');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Municipality',
                        flex: 1,
                        text: 'Город/район',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Address',
                        flex: 1,
                        text: 'Адрес дома',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ServiceName',
                        flex: 1,
                        text: 'Услуга',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ServiceTypeName',
                        flex: 1,
                        text: 'Тип услуги',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'SupplierName',
                        flex: 1,
                        text: 'Поставщик',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TarifValue',
                        flex: 1,
                        text: 'Значение',
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TarifTypeName',
                        flex: 1,
                        text: 'Тип тарифа',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'FormulaName',
                        flex: 1,
                        text: 'Методика расчета',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'LsCount',
                        flex: 1,
                        text: 'Кол-во ЛС',
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            operand: CondExpr.operands.eq
                        }
                    }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'b4monthpicker',
                            name: 'Period',
                            width: 200,
                            fieldLabel: 'Период',
                            editable: false,
                            labelWidth: 40,
                            labelAlign: 'right',
                            format: 'F, Y'
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