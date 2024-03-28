Ext.define('B4.view.HeatInputPeriodGrid', {
    extend: 'B4.ux.grid.Panel',
    title: 'Информация о подаче тепла',
    alias: 'widget.heatinputperiodgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.Url'
    ],

    store: 'HeatInputPeriod',
    closable: true,

    months: {
        '1': 'Январь',
        '2': 'Февраль',
        '3': 'Март',
        '4': 'Апрель',
        '5': 'Май',
        '6': 'Июнь',
        '7': 'Июль',
        '8': 'Август',
        '9': 'Сентябрь',
        '10': 'Октябрь',
        '11': 'Ноябрь',
        '12': 'Декабрь'
    },

    initComponent: function () {
        var me = this;

        var yearStore = Ext.create('Ext.data.Store', {
            fields: ['num', 'name'],
            data: [
                { "num": null, "name": " - " },
                { "num": 2014, "name": "2014" },
                { "num": 2015, "name": "2015" },
                { "num": 2016, "name": "2016" },
                { "num": 2017, "name": "2017" },
                { "num": 2018, "name": "2018" },
                { "num": 2019, "name": "2019" },
                { "num": 2020, "name": "2020" },
                { "num": 2021, "name": "2021" },
                { "num": 2022, "name": "2022" },
                { "num": 2023, "name": "2023" },
                { "num": 2024, "name": "2024" }
            ]
        });

        var monthStore = Ext.create('Ext.data.Store', {
            fields: ['num', 'name'],
            data: [
                { "num": null, "name": " - " },
                { "num": 1, "name": "Январь" },
                { "num": 2, "name": "Февраль" },
                { "num": 3, "name": "Март" },
                { "num": 4, "name": "Апрель" },
                { "num": 5, "name": "Май" },
                { "num": 6, "name": "Июнь" },
                { "num": 7, "name": "Июль" },
                { "num": 8, "name": "Август" },
                { "num": 9, "name": "Сентябрь" },
                { "num": 10, "name": "Октябрь" },
                { "num": 11, "name": "Ноябрь" },
                { "num": 12, "name": "Декабрь" }
            ]
        });

        Ext.applyIf(me, {
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
                    flex: 2,
                    itemId: 'hipYear',
                    filter: {
                        xtype: 'combobox',
                        operand: CondExpr.operands.eq,
                        index: 'Year',
                        queryMode: 'local',
                        valueField: 'num',
                        displayField: 'name',
                        editable: false,
                        store: yearStore,
                        name: 'Year'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Month',
                    text: 'Месяц',
                    flex: 2,
                    itemId: 'hipMonth',
                    renderer: function (value) {
                        return me.months[value];
                    },
                    filter: {
                        xtype: 'combobox',
                        operand: CondExpr.operands.eq,
                        index: 'Month',
                        queryMode: 'local',
                        valueField: 'num',
                        displayField: 'name',
                        editable: false,
                        store: monthStore,
                        name: 'Month'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    flex: 3,
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
                        //'->',
                        //{
                        //    xtype: 'label',
                        //    text: 'Отчетный период:'
                        //},
                        //{
                        //    xtype: 'combobox',
                        //    index: 'Year',
                        //    queryMode: 'local',
                        //    valueField: 'num',
                        //    displayField: 'name',
                        //    editable: false,
                        //    store: yearStore,
                        //    name: 'Year'
                        //},
                        //{
                        //    xtype: 'combobox',
                        //    index: 'Month',
                        //    queryMode: 'local',
                        //    valueField: 'num',
                        //    displayField: 'name',
                        //    editable: false,
                        //    store: monthStore,
                        //    name: 'Month'
                        //}
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