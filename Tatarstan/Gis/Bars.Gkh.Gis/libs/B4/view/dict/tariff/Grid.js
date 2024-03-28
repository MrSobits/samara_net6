Ext.define('B4.view.dict.tariff.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.enums.GisTariffKind',
        'B4.store.dict.GisTariff',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Тарифы',
    alias: 'widget.tariffdictgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.GisTariff');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            layout: 'fit',
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    text: 'Наименование услуги',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    text: 'Поставщик',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentInn',
                    text: 'ИНН поставщика',
                    filter: { xtype: 'textfield' },
                    width: 100
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TariffKind',
                    text: 'Вид тарифа',
                    enumName: 'B4.enums.GisTariffKind',
                    filter: true,
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ZoneCount',
                    text: 'Количество зон',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format:'0.00',
                    dataIndex: 'TariffValue',
                    text: 'Тариф',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    dataIndex: 'TariffValue1',
                    text: 'Тариф 1',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    dataIndex: 'TariffValue2',
                    text: 'Тариф 2',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    dataIndex: 'TariffValue3',
                    text: 'Тариф 3',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    header: 'Дата начала',
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'StartDate',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    header: 'Дата окончания',
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'EndDate',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    header: 'Дата загрузки в ЕИАС',
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'EiasUploadDate',
                    width: 120,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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