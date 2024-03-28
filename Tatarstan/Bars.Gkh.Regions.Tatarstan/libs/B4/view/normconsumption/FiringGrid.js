Ext.define('B4.view.normconsumption.FiringGrid', {
    extend: 'Ext.form.Panel',
    alias: 'widget.normconsfiringgrid',
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
        'B4.form.EnumCombo',
        'B4.store.dict.PeriodNormConsumption'
    ],

    title: 'Сведения по нормативам потребления Отопление',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.normconsumption.NormConsumptionFiring');

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
                            dataIndex: 'GenerealBuildingFiringMeters',
                            width: 70,
                            header: 'Наличие общедомового прибора учета тепловой энергии на подогрев',
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
                            dataIndex: 'AreaNotLivingIpu',
                            text: 'Площадь нежилых помещений при наличии отдельного ипу у нежилых помещений, кв.м',
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
                            dataIndex: 'AmountHeatEnergyNotLivingIpu',
                            text: 'Количество тепловой энергии на отопление по нежилым помещениям при наличии ипу Гкал',
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
                            dataIndex: 'AmountHeatEnergyNotLivInPeriod',
                            text: 'Для домов с ОПУ Количество тепловой энергии на отопление жилых домов за отопительный период (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) Гкал',
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
                            dataIndex: 'WallMaterial',
                            text: 'Материал стен  (кирпич, панели, блоки, дерево, смеш и др. материалы)',
                            width: 80
                        },
                        {
                            dataIndex: 'RoofMaterial',
                            text: 'Материал крыши',
                            width: 80
                        },
                        {
                            dataIndex: 'HourlyHeatLoadForPassport',
                            text: 'Часовая тепловая нагрузка, Ккал/час по паспорту здания',
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
                            dataIndex: 'HourlyHeatLoadForDocumentation',
                            text: 'Часовая тепловая нагрузка, Ккал/час по проектной документации',
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
                                        this.up('normconsfiringgrid').close();
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