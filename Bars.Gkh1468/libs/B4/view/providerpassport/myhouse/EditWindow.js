Ext.define('B4.view.providerpassport.myhouse.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.houseprovpassportwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.view.realityobj.Grid',
        'B4.store.realityobj.ForPassport',
        'B4.store.PeriodYear',
        
        'B4.form.SelectField',

        'B4.view.passport.StructGrid',
        'B4.store.passport.PassportStruct'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Добавить новый',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this,
            cbMonth = Ext.create('Ext.form.ComboBox', {
                index: 'Month',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                allowBlank: false,
                editable: false,
                store: Ext.create('Ext.data.Store', {
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
                })
            }),
            cbYear = Ext.create('Ext.form.ComboBox', {
                index: 'Year',
                queryMode: 'local',
                valueField: 'num',
                allowBlank: false,
                editable: false,
                displayField: 'name',
                store: Ext.create('B4.store.PeriodYear')
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 125
            },
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Выберите, на какой отчетный период добавить паспорт:',
                    items: [
                        {
                            xtype: 'fieldcontainer',
                            fieldLabel: 'Отчетный период',
                            layout: 'hbox',
                            items: [
                                cbYear,
                                {
                                    xtype: 'splitter'
                                },
                                cbMonth
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Информация по дому',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            anchor: '100%',
                            allowBlank: false,
                            fieldLabel: 'Жилой дом',
                           

                            store: 'B4.store.realityobj.ForPassport',
                            editable: false,
                            textProperty: 'Address',
                            columns: [
                                { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }]
                        },
                        {
                            xtype: 'displayfield',
                            index: 'info',
                            value: 'Нет ни одной структуры паспорта, действующей на данный период. Для создания паспорта необходимо создать структуру.',
                            hidden: true
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Отчетный период',
                            name: 'OldPaspPeriods',
                            allowBlank: true,
                            queryMode: 'local',
                            hidden: true,
                            valueField: 'Id',
                            displayField: 'Period',
                            store: Ext.create('Ext.data.ArrayStore', {
                                autoDestroy: true,
                                fields: ['Id', 'Period']
                            }),
                            editable: false
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
