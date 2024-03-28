Ext.define('B4.view.normconsumption.HeatingGrid', {
    extend: 'Ext.form.Panel',
    alias: 'widget.normconsheatinggrid',
    bodyStyle: Gkh.bodyStyle,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        //'B4.aspects.StateContextButtonMenu',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.store.normconsumption.NormConsumption',
        'B4.enums.YesNo',
        'B4.enums.TypeHotWaterSystem',
        'B4.enums.TypeRisers',
        'B4.form.EnumCombo',
        'B4.store.dict.PeriodNormConsumption'
    ],

    title: 'Сведения по нормативам потребления Подогрев',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.normconsumption.NormConsumptionHeating');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    margin: 5,
                    border: null,
                    padding: 0,
                    layout: {
                        type: 'hbox',
                        align: 'strecth'
                    },
                    items: [
                        
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'leftstrecth'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Municipality',
                                    readOnly: true,
                                    editable: true,
                                    fieldLabel: 'Муниципальный район',
                                    labelWidth: 175,
                                    width: 300
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Period',
                                    readOnly: true,
                                    editable: true,
                                    fieldLabel: 'Период',
                                    labelWidth: 125,
                                    width: 300
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Норматив потребления',
                                    enumName: 'B4.enums.NormConsumptionType',
                                    name: 'Type',
                                    readOnly: true,
                                    labelWidth: 175,
                                    width: 400
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    store: store,
                    cls: 'x-large-head',
                    columnLines: true,
                    enableLocking: true,
                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            text: 'Муниципальный район',
                            locked: true,
                            width: 200,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Address',
                            text: 'Адрес',
                            locked: true,
                            width: 200,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'FloorNumber',
                            text: 'Количество этажей',
                            lockable: false,
                            width: 70,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'BuildYear',
                            text: 'Год постройки',
                            width: 70,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'GenerealBuildingHeatMeters',
                            width: 70,
                            header: 'Наличие общедомового прибора учета тепловой энергии на подогрев',
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'TechnicalCapabilityOpu',
                            width: 70,
                            header: 'Наличие технической возможности установки ОПУ',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            text: 'Общая площадь дома, кв.м.',
                            dataIndex: 'AreaHouse',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaLivingRooms',
                            text: 'Общая площадь жилых помещений (квартиры), кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaNotLivingRooms',
                            text: 'Общая площадь нежилых помещений, кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaOtherRooms',
                            text: 'Общая площадь помещений, входящих в состав имущества, кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsIpuNotLivingPermises',
                            width: 70,
                            header: 'Наличие отдельного ипу у нежилых помещений',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            dataIndex: 'HeatEnergyConsumptionInPeriod',
                            text: 'Расход по показаниям ИПУ нежилых помещений суммарный расход тепловой энергии на подогрев ХВ, по показаниям ИПУ нежилых помещений за отопительный период, Гкал',
                            width: 120,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'HotWaterConsumptionInPeriod',
                            text: 'Расход по показаниям ИПУ нежилых помещений суммарный расход горячей воды по показаниям ИПУ нежилых помещений за отопительный период, куб.м',
                            width: 120,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                            }
                        },
                        {
                            dataIndex: 'TypeHotWaterSystemStr',
                            text: 'Вид системы горячего водоснабжения (открытая, закрытая)',
                            width: 100
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.TypeHotWaterSystem',
                            dataIndex: 'TypeHotWaterSystem',
                            width: 100,
                            header: 'Вид системы горячего водоснабжения (с наружной сетью ГВС, без наружной сети ГВС)',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.TypeHotWaterSystem.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.TypeHotWaterSystem.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsHeatedTowelRail',
                            width: 80,
                            header: 'Наличие полотенце сушителей',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.TypeRisers',
                            dataIndex: 'Risers',
                            width: 100,
                            header: 'Наличие стояков',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.TypeRisers.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.TypeRisers.displayRenderer(val);
                            }
                        },
                        {
                            dataIndex: 'HeatEnergyConsumptionNotLivInPeriod',
                            text: 'Для домов с ОПУ суммарный расход тепловой энергии на подогрев ХВ, по показаниям ОПУза отопительный период, Гкал',
                            width: 100,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'HotWaterConsumptionNotLivInPeriod',
                            text: 'Для домов с ОПУ суммарный расход горячей воды  по показаниям ОПУза отопительный период, куб.м',
                            width: 100,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'HeatingPeriod',
                            text: 'Отопительный период, дни',
                            width: 80,
                            editor: {
                                xtype: 'numberfield',
                                allowDecimals: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'AvgTempColdWater',
                            text: 'Средняя температура холодной воды в сети водопровода, по данным Гидрометеослужбы (при наличии сведений)',
                            width: 80,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'WearIntrahouseUtilites',
                            text: 'Износ внутридомовых инженерных сетей %',
                            width: 80,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'OverhaulDate',
                            text: 'Дата проведения капитального ремонта',
                            format: 'd.m.Y',
                            width: 80
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ],
                    normalGridConfig: {
                        plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                         Ext.create('Ext.grid.plugin.CellEditing', {
                             clicksToEdit: 1,
                             pluginId: 'cellEditing'
                         })]
                    },
                    lockedGridConfig: {
                        plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')]
                    },
                    viewConfig: {
                        loadMask: true
                    }
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
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                xtype: 'button',
                                text: 'Печать',
                                action: 'Export',
                                iconCls: 'icon-page-excel'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function () {
                                        this.up('normconsheatinggrid').close();
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