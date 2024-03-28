Ext.define('B4.view.realityobj.LiftWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjectliftwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    minWidth: 900,
    minHeight: 500,
    bodyPadding: 5,
    bodyStyle: 'none 0px 0px repeat scroll transparent',
    title: 'Сведения о лифтовом оборудовании',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.store.dict.TypeLift',
        'B4.store.dict.TypeLiftDriveDoors',
        'B4.store.dict.TypeLiftMashineRoom',
        'B4.store.dict.TypeLiftShaft',
        'B4.store.dict.ModelLift',
        'B4.store.dict.CabinLift',
        'B4.store.Contragent',
        'B4.enums.LiftAvailabilityDevices',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    margins: -1,
                    defaults: {
                        
                        border: true,
                        bodyPadding: 5,
                        margins: -1,
                        frame: true
                    },
                    items: [
                        {
                            title: 'Основная информация',

                            layout: 'anchor',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Общие сведения',
                                    layout: 'hbox',
                                    defaults: {
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PorchNum',
                                                    allowBlank: false,
                                                    fieldLabel: 'Номер подъезда',
                                                    maxLength: 100,
                                                    regex: /^\d+$/,
                                                    regexText: 'В это поле можно вводить только цифры'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'LiftNum',
                                                    fieldLabel: 'Номер лифта',
                                                    allowBlank: false,
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FactoryNum',
                                                    fieldLabel: 'Заводской номер',
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegNum',
                                                    fieldLabel: 'Регистрационный номер',
                                                    allowBlank: false,
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'YearInstallation',
                                                    fieldLabel: 'Год установки',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    minValue: 1900,
                                                    maxValue: 2100
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'YearLastUpgradeRepair',
                                                    fieldLabel: 'Год последней модернизации/восстановительного ремонта',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    minValue: 1900,
                                                    maxValue: 2100
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'YearExploitation',
                                                    fieldLabel: 'Год ввода в эксплуатацию',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    minValue: 1900,
                                                    maxValue: 2100
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'StopCount',
                                                    fieldLabel: 'Количество остановок',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'NumberOfStoreys',
                                                    fieldLabel: 'Этажность',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным'
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'TypeLift',
                                                    fieldLabel: 'Тип лифта',
                                                    store: 'B4.store.dict.TypeLift',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'ModelLift',
                                                    fieldLabel: 'Модель лифта',
                                                    store: 'B4.store.dict.ModelLift',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'CabinLift',
                                                    fieldLabel: 'Лифт (кабина)',
                                                    store: 'B4.store.dict.CabinLift',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'TypeLiftMashineRoom',
                                                    fieldLabel: 'Расположение машинного помещения',
                                                    store: 'B4.store.dict.TypeLiftMashineRoom',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    fieldLabel: 'Наличие устройства для автом. опускания',
                                                    name: 'AvailabilityDevices',
                                                    xtype: 'b4enumcombo',
                                                    enumName: 'B4.enums.LiftAvailabilityDevices',
                                                    allowBlank: false,
                                                    editable: false,
                                                    includeEmpty: false,
                                                    enumItems: [],
                                                    hideTrigger: false
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 0 30',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    decimalSeparator: ',',
                                                    name: 'Capacity',
                                                    fieldLabel: 'Грузоподъемность',
                                                    hideTrigger: true,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным'
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    decimalSeparator: ',',
                                                    name: 'SpeedRise',
                                                    fieldLabel: 'Скорость подъема (м/сек)',
                                                    hideTrigger: true,
                                                    minValue: 0,
                                                    negativeText: 'Значение не может быть отрицательным'
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'TypeLiftShaft',
                                                    fieldLabel: 'Шахта лифта',
                                                    store: 'B4.store.dict.TypeLiftShaft',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    title: 'Габариты шахты (мм)',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 200
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'DepthLiftShaft',
                                                            fieldLabel: 'Глубина',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'WidthLiftShaft',
                                                            fieldLabel: 'Ширина',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'HeightLiftShaft',
                                                            fieldLabel: 'Высота',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'TypeLiftDriveDoors',
                                                    fieldLabel: 'Привод дверей кабины',
                                                    store: 'B4.store.dict.TypeLiftDriveDoors',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    emptyText: 'Не задано',
                                                    emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
                                                    columns: [
                                                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                },
                                                {
                                                    xtype: 'fieldset',
                                                    title: 'Габариты кабины (мм)',
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 200
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'DepthCabin',
                                                            fieldLabel: 'Глубина',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'WidthCabin',
                                                            fieldLabel: 'Ширина',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'HeightCabin',
                                                            fieldLabel: 'Высота',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            decimalSeparator: ',',
                                                            name: 'WidthOpeningCabin',
                                                            fieldLabel: 'Ширина проема в свету',
                                                            hideTrigger: true,
                                                            minValue: 0,
                                                            negativeText: 'Значение не может быть отрицательным'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OwnerLift',
                                                    fieldLabel: 'Владелец лифтового оборудования',
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'Contragent',
                                                    fieldLabel: 'Обслуживающая организация',
                                                    store: 'B4.store.Contragent',
                                                    columns: [
                                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                        {
                                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    editable: false
                                                }
                                            ]
                                        }
                                    ]
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});