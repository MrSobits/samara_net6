Ext.define('B4.view.dict.workprice.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.workpricepanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.form.ComboBox',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Расценки по работам',
    store: 'dict.WorkPrice',
    closable: true,

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
                    dataIndex: 'Job',
                    flex: 2,
                    text: 'Наименование работы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CapitalGroup',
                    flex: 1,
                    text: 'Группа капитальности',
                    filter: { xtype: 'textfield' }
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
                        url: '/Municipality/ListByParam'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    width: 100,
                    text: 'Единица измерения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeCost',
                    width: 100,
                    text: 'Стоимость на единицу объема КЭ (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SquareMeterCost',
                    flex: 1,
                    text: 'Стоимость на единицу площади МКД (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    width: 100,
                    text: 'Год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'export',
                                    itemId: 'btnExport'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            margin: '0 10',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Добавить массово',
                                    iconCls: 'icon-add',
                                    cmd: 'massAddition'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Копировать',
                                    tooltip: 'Копировать',
                                    iconCls: 'icon-accept',
                                    menu: {
                                        items: [
                                            {
                                                text: "Расценки по работам",
                                                action: 'copyworkprice'
                                            }
                                        ]
                                    }
                                }]
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