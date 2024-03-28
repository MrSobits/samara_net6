Ext.define('B4.view.administration.risdataexport.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'form',
    },
    width: 950,
    bodyPadding: 10,
    closeAction: 'destroy',

    title: 'Экспорт данных системы в РИС ЖКХ',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.administration.risdataexport.FormatDataExportSection'
    ],

    alias: 'widget.risdataexportaddwindow',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    padding: '10 5',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'EntityGroupCodeList',
                            fieldLabel: 'Тип выгрузки',
                            labelWidth: 120,
                            store: 'B4.store.administration.risdataexport.FormatDataExportSection',
                            selectionMode: 'MULTI',
                            flex: 1,
                            idProperty: 'Code',
                            textProperty: 'Description',
                            columns: [
                                { header: 'Название секции', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                        },
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'radiogroup',
                            name: 'PeriodType',
                            vertical: true,
                            columns: 1,
                            flex: 1,
                            border: '0 1 0 0',
                            style: {
                                borderStyle: 'solid',
                                borderColor: '#99bbe8'
                            },
                            padding: '5',
                            defaults: {
                                padding: '5',
                                name: 'PeriodType'
                            },
                            items: [
                                { boxLabel: 'Периодичность отсутствует', inputValue: 0, checked: true },
                                { boxLabel: 'Ежедневно', inputValue: 1 },
                                { boxLabel: 'Еженедельно', inputValue: 2 },
                                { boxLabel: 'Ежемесячно', inputValue: 3 }
                            ],
                        },
                        {
                            xtype: 'container',
                            flex: 6,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            padding: '5 5 5 15',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    padding: '0 0 10 0',
                                    defaults: {
                                        labelWidth: 160,
                                        padding: '5 15 5 0'
                                    },
                                    name: 'DateInterval',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'StartDate',
                                            fieldLabel: 'Дата начала действия'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EndDate',
                                            fieldLabel: 'Дата окончания действия'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'StartNow',
                                            fieldLabel: 'Запустить сейчас',
                                            margin: '5 20 0 0',
                                            labelWidth: 110
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            name: 'TimeInterval',
                                            items: [
                                                {
                                                    xtype: 'label',
                                                    text: 'Время запуска:',
                                                    padding: '5 0',
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'StartTimeHour',
                                                    width: 60,
                                                    padding: '0 10',
                                                    allowDecimals: false,
                                                    maxValue: 23,
                                                    minValue: 0,
                                                },
                                                {
                                                    xtype: 'label',
                                                    text: 'час.',
                                                    padding: '5 0'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'StartTimeMinutes',
                                                    width: 60,
                                                    padding: '0 10',
                                                    allowDecimals: false,
                                                    maxValue: 59,
                                                    minValue: 0,
                                                },
                                                {
                                                    xtype: 'label',
                                                    text: 'мин.',
                                                    padding: '5 0'
                                                }
                                            ],
                                        },
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'каждую неделю по',
                                    margin: '15 0',
                                    name: 'StartDayOfWeekList',
                                    allowBlank: false,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartDayOfWeekList',
                                    },
                                    items: [
                                        { boxLabel: 'Пн', inputValue: 1 },
                                        { boxLabel: 'Вт', inputValue: 2 },
                                        { boxLabel: 'Ср', inputValue: 3 },
                                        { boxLabel: 'Чт', inputValue: 4 },
                                        { boxLabel: 'Пт', inputValue: 5 },
                                        { boxLabel: 'Сб', inputValue: 6 },
                                        { boxLabel: 'Вс', inputValue: 7 }
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'в течение следующих месяцев',
                                    columns: 6,
                                    margin: '15 0',
                                    name: 'StartMonthList',
                                    allowBlank: false,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartMonthList',
                                    },
                                    items: [
                                        { boxLabel: 'Январь', inputValue: 1 },
                                        { boxLabel: 'Февраль', inputValue: 2 },
                                        { boxLabel: 'Март', inputValue: 3 },
                                        { boxLabel: 'Апрель', inputValue: 4 },
                                        { boxLabel: 'Май', inputValue: 5 },
                                        { boxLabel: 'Июнь', inputValue: 6 },
                                        { boxLabel: 'Июль', inputValue: 7 },
                                        { boxLabel: 'Август', inputValue: 8 },
                                        { boxLabel: 'Сентябрь', inputValue: 9 },
                                        { boxLabel: 'Октябрь', inputValue: 10 },
                                        { boxLabel: 'Ноябрь', inputValue: 11 },
                                        { boxLabel: 'Декабрь', inputValue: 12 }
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'каждый день месяца по',
                                    columns: 8,
                                    margin: '15 0',
                                    name: 'StartDaysList',
                                    allowBlank: false,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartDaysList',
                                    },
                                    items: [
                                        { boxLabel: '1', inputValue: 1 },
                                        { boxLabel: '2', inputValue: 2 },
                                        { boxLabel: '3', inputValue: 3 },
                                        { boxLabel: '4', inputValue: 4 },
                                        { boxLabel: '5', inputValue: 5 },
                                        { boxLabel: '6', inputValue: 6 },
                                        { boxLabel: '7', inputValue: 7 },
                                        { boxLabel: '8', inputValue: 8 },
                                        { boxLabel: '9', inputValue: 9 },
                                        { boxLabel: '10', inputValue: 10 },
                                        { boxLabel: '11', inputValue: 11 },
                                        { boxLabel: '12', inputValue: 12 },
                                        { boxLabel: '13', inputValue: 13 },
                                        { boxLabel: '14', inputValue: 14 },
                                        { boxLabel: '15', inputValue: 15 },
                                        { boxLabel: '16', inputValue: 16 },
                                        { boxLabel: '17', inputValue: 17 },
                                        { boxLabel: '18', inputValue: 18 },
                                        { boxLabel: '19', inputValue: 19 },
                                        { boxLabel: '20', inputValue: 20 },
                                        { boxLabel: '21', inputValue: 21 },
                                        { boxLabel: '22', inputValue: 22 },
                                        { boxLabel: '23', inputValue: 23 },
                                        { boxLabel: '24', inputValue: 24 },
                                        { boxLabel: '25', inputValue: 25 },
                                        { boxLabel: '26', inputValue: 26 },
                                        { boxLabel: '27', inputValue: 27 },
                                        { boxLabel: '28', inputValue: 28 },
                                        { boxLabel: '29', inputValue: 29 },
                                        { boxLabel: '30', inputValue: 30 },
                                        { boxLabel: '31', inputValue: 31 },
                                        { lastDay: true, boxLabel: 'посл. день', inputValue: 0 }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tbfill',
                    height: 10
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
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        'click' : function (button) { button.up('window').close(); }
                                    }
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