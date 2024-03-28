Ext.define('B4.view.workwintercondition.CopyWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.workwinterConditionCopyWindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.view.workwintercondition.WorkWinterConditionGrid',
        'B4.store.HeatInputPeriod',
        'B4.store.dict.municipality.ListByParamAndOperator'
    ],

    layout: 'form',
    modal: true,
    width: 600,
    bodyPadding: 5,
    title: 'Копировать данные из другого периода',

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
                labelAlign: 'left',
                labelWidth: 177
                },
            items: [
                {
                    xtype: 'fieldset',
                    border:0,
                    title: 'Выберите отчетный период, из которого будут скопированы данные',

                    items: [
                        {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        padding: '0 0 5 0',
                        defaults: {
                            labelAlign: 'right',
                            labelWidth: 180
                        },
                        items: [
                            {
                                xtype: 'label',
                                text: 'Отчетный период:',
                                width: 120
                            },
                            {
                                xtype: 'combobox',
                                fieldLabel: 'Год',
                                hideLabel: true,
                                allowBlank: false,
                                emptyText: 'Выберите год',
                                index: 'Year',
                                queryMode: 'local',
                                itemId: 'yearWorkWinterCombo',
                                valueField: 'num',
                                displayField: 'name',
                                editable: false,
                                store: yearStore,
                                name: 'Year',
                                flex: 1,
                                margin: '0 20 0 0'
                            },
                            {
                                xtype: 'combobox',
                                fieldLabel: 'Месяц',
                                hideLabel: true,
                                allowBlank: false,
                                emptyText: 'Выберите месяц',
                                index: 'Month',
                                queryMode: 'local',
                                itemId: 'monthWorkWinterCombo',
                                valueField: 'num',
                                displayField: 'name',
                                editable: false,
                                store: monthStore,
                                flex: 1,
                                name: 'Month'
                            }
                         ]
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