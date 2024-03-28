Ext.define('B4.view.heatinputperiod.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.heatinputperiodEditWindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.view.HeatInputPeriodGrid',
        'B4.store.HeatInputPeriod',
        'B4.store.dict.municipality.ListByParamAndOperator'
    ],

    layout: 'form',
    modal: true,
    width: 500,
    bodyPadding: 5,
    title: 'Добавить новый',

    initComponent: function () {
        var me = this;

        var yearStore = Ext.create('Ext.data.Store', {
            fields: ['num', 'name'],
            data: [
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
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Муниципальное образование',
                    name: 'Municipality',
                    store: 'B4.store.dict.municipality.ListByParamAndOperator',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '7 0 10 2',
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        flex: 1
                    },
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'label',
                            text: 'Отчетный период:'
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Год',
                            hideLabel: true,
                            allowBlank: false,
                            emptyText: 'Выберите год',
                            index: 'Year',
                            queryMode: 'local',
                            valueField: 'num',
                            displayField: 'name',
                            editable: false,
                            store: yearStore,
                            name: 'Year'
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Месяц',
                            hideLabel: true,
                            allowBlank: false,
                            emptyText: 'Выберите месяц',
                            index: 'Month',
                            queryMode: 'local',
                            valueField: 'num',
                            displayField: 'name',
                            editable: false,
                            store: monthStore,
                            name: 'Month'
                        }
                    ]
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
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});