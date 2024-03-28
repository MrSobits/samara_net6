Ext.define('B4.view.providerpassport.myoki.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.okiprovpassportwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.view.dict.municipality.Grid',
        'B4.store.dict.Municipality',
        
        'B4.form.SelectField',
        
        'B4.store.PeriodYear',
        
        'B4.view.passport.StructGrid',
        'B4.store.passport.PassportStruct'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    minWidth: 400,
    minHeight: 150,
    bodyPadding: 5,
    title: 'Добавить новый',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this,
            cbMonth = Ext.create('Ext.form.ComboBox', {
                flex: 1,
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
                flex: 1,
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
                labelWidth: 175
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    allowBlank: false,
                    fieldLabel: 'Муниципальное образование',
                    //

                    store: 'B4.store.passport.OkiMunicipality',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'displayfield',
                    value: 'Выберите, на какой отчетный период добавить паспорт:'
                },
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
