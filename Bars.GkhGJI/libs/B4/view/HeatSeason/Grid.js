Ext.define('B4.view.heatseason.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.HeatSeasonPeriodGji',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.HeatingSystem',
        'B4.enums.TypeHouse'
    ],

    title: 'Документы по подготовке к отопительному сезону',
    store: 'view.HeatSeason',
    alias: 'widget.heatSeasonGrid',
    closable: true,
    enableColumnHide: true,

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
                    dataIndex: 'MU',
                    flex: 2,
                    text: 'МО',
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
                    flex: 4,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatSys',
                    flex: 2,
                    text: 'Система отопления',
                    renderer: function (val) {
                        return B4.enums.HeatingSystem.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatingSystem.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    flex: 2,
                    text: 'Тип дома',
                    renderer: function (val) {
                        return B4.enums.TypeHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MaxFl',
                    flex: 1,
                    text: 'Макс. этажность',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MinFl',
                    flex: 1,
                    text: 'Мин. этажность',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumEntr',
                    flex: 1,
                    text: 'Кол-во подъездов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaMkd',
                    flex: 1,
                    text: 'Общая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateHeat',
                    flex: 1,
                    text: 'Дата пуска тепла',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActF',
                    flex: 1,
                    text: 'Акт промывки',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActP',
                    flex: 1,
                    text: 'Акт опрессовки',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActV',
                    flex: 1,
                    text: 'Акт проверки вентиляции',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActC',
                    flex: 1,
                    text: 'Акт проверки дымоходов',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Passport',
                    flex: 1,
                    text: 'Паспорт готовности',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Morg',
                    flex: 2,
                    text: 'УО',
                    filter: { xtype: 'textfield' }
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
                    layout: 'vbox',
                    padding: 5,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                    name: 'HeatingPeriod',
                                    fieldLabel: 'Период отопительного сезона',
                                    width: 480,
                                   

                                    store: 'B4.store.dict.HeatSeasonPeriodGji',
                                    itemId: 'heatPeriodSelectField',
                                    editable: false,
                                    labelWidth: 160
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    margin: '0 10px 0 10px'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbIndividualHeatingSystem',
                                    boxLabel: 'Показать дома с индивидуальной системой отопления',
                                    labelAlign: 'right',
                                    margin: '10px 10px 0 0'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbBlockedObjects',
                                    boxLabel: 'Показать дома с типом блокированной застройки',
                                    margin: '10px 10px 0 0'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowIndividual',
                                    boxLabel: 'Показать индивидуальные дома',
                                    margin: '10px 0 0 0'
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