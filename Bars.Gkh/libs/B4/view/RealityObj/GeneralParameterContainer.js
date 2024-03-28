Ext.define('B4.view.realityobj.GeneralParameterContainer', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.realityobjgeneralparametercontainer',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Общие характеристики',

    defaults: {
        flex: 1
    },

    margin: '0 5',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
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
                                    flex: 4,
                                    items: [
                                        {
                                            xtype: 'container',
                                            name: 'CadastreNumber',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CadastreNumber',
                                                    fieldLabel: 'Кадастровый номер земельного участка',
                                                    maxLength: 300,
                                                    itemId: 'tfCadastreNumber',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'CadastralHouseNumber',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CadastralHouseNumber',
                                                    fieldLabel: 'Кадастровый номер дома',
                                                    maskRe: /[0-9:]/,
                                                    maxLength: 300,
                                                    itemId: 'tfCadastralHouseNumber',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'TotalBuildingVolume',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'TotalBuildingVolume',
                                                    fieldLabel: 'Общий строительный объем (куб.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    itemId: 'nfTotalBuildingVolume',
                                                    minValue: 0,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaMkd',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaMkd',
                                                    fieldLabel: 'Общая площадь (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaOwned',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaOwned',
                                                    fieldLabel: 'Площадь частной собственности (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaOwned',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaOwned'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaMunicipalOwned',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaMunicipalOwned',
                                                    fieldLabel: 'Площадь муниципальной собственности (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaMunicipalOwned',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaMunicipalOwned'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaGovernmentOwned',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaGovernmentOwned',
                                                    fieldLabel: 'Площадь государственной собственности (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaGovernmentOwned',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaGovernmentOwned'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaLivingNotLivingMkd',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaLivingNotLivingMkd',
                                                    fieldLabel: 'Общая площадь жилых и нежилых помещений (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaLivingNotLivingMkdRealityObject',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaLivingNotLivingMkd'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaLiving',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaLiving',
                                                    fieldLabel: 'В т.ч. жилых всего (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaLivingRealityObject',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaLiving'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaNotLivingPremises',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaNotLivingPremises',
                                                    fieldLabel: 'В т. ч. нежилых всего (кв. м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    itemId: 'nfAreaNotLivingPremises',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: true,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillAreaNotLivingPremises'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaLivingOwned',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaLivingOwned',
                                                    fieldLabel: 'В т.ч. жилых, находящихся в собственности граждан (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    itemId: 'nfAreaLivingOwnedRealityObject',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaNotLivingFunctional',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaNotLivingFunctional',
                                                    fieldLabel: 'Общая площадь помещений, входящих в состав общего имущества (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    itemId: 'nfAreaNotLivingFunctional',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaCommonUsage',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaCommonUsage',
                                                    fieldLabel: 'Площадь помещений общего пользования',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    itemId: 'nfAreaCommonUsage',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'AreaCleaning',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'AreaCleaning',
                                                    fieldLabel: 'Уборочная площадь (кв.м.)',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    decimalSeparator: ',',
                                                    itemId: 'nfAreaCleaning',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'NecessaryConductCr',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 350
                                            },
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    floating: false,
                                                    name: 'NecessaryConductCr',
                                                    fieldLabel: 'Требовалось проведение КР на дату приватизации первого жилого помещения',
                                                    labelAlign: 'right',
                                                    displayField: 'Display',
                                                    store: B4.enums.YesNoNotSet.getStore(),
                                                    valueField: 'Value',
                                                    itemId: 'cbNecessaryConductCr',
                                                    flex: 1
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 170
                                    },
                                    margin: '0 0 0 30',
                                    flex: 3,
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'MaximumFloors',
                                            fieldLabel: 'Максимальная этажность',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfMaximumFloorsRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'Floors',
                                            fieldLabel: 'Минимальная этажность',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfFloorsRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'FloorHeight',
                                            fieldLabel: 'Высота этажа (м)',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfFloorHeight',
                                            decimalSeparator: ',',
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberEntrances',
                                            fieldLabel: 'Количество подъездов',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfNumberEntrancesRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberApartments',
                                            fieldLabel: 'Количество квартир',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfNumberApartmentsRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberNonResidentialPremises',
                                            fieldLabel: 'Количество нежилых помещений',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfNumberNonResidentialPremises',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberLiving',
                                            fieldLabel: 'Количество проживающих',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfNumberLivingRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberLifts',
                                            fieldLabel: 'Количество лифтов',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            itemId: 'nfNumberLiftsRealityObject',
                                            allowDecimals: false,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.RoofingMaterial',
                                            name: 'RoofingMaterial',
                                            fieldLabel: 'Материал кровли',
                                            editable: false,
                                            itemId: 'sfRoofingMaterial'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.WallMaterial',
                                            name: 'WallMaterial',
                                            fieldLabel: 'Материал стен',
                                            editable: false,
                                            itemId: 'sfWallMaterialRealityObject'
                                        },
                                        {
                                            xtype: 'combobox',
                                            name: 'TypeRoof',
                                            fieldLabel: 'Тип кровли',
                                            displayField: 'Display',
                                            store: B4.enums.TypeRoof.getStore(),
                                            valueField: 'Value',
                                            itemId: 'cbTypeRoofRealityObject',
                                            editable: false
                                        },
                                        {
                                            xtype: 'container',
                                            name: 'PercentDebt',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'middle'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 170
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'PercentDebt',
                                                    fieldLabel: 'Собираемость платежей %',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    allowDecimals: true,
                                                    itemId: 'nfPercentDebt',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'button',
                                                    margin: '0 0 0 5',
                                                    text: 'Заполнить',
                                                    action: 'FillPercentDebt'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'combobox',
                                            name: 'HeatingSystem',
                                            fieldLabel: 'Система отопления',
                                            displayField: 'Display',
                                            store: B4.enums.HeatingSystem.getStore(),
                                            valueField: 'Value',
                                            itemId: 'cbHeatingSystem',
                                            editable: false
                                        },
                                        {
                                            xtype: 'combobox',
                                            name: 'HasJudgmentCommonProp',
                                            fieldLabel: 'Наличие судебного решения по проведению КР общего имущества',
                                            displayField: 'Display',
                                            store: B4.enums.YesNo.getStore(),
                                            valueField: 'Value',
                                            itemId: 'cbHasJudgment',
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'TechPassportScanFile',
                                            fieldLabel: 'Технический паспорт объекта',
                                            itemId: 'ffTechPassportScanFile'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            name: 'NoteContainer',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 350,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    fieldLabel: 'Примечание',
                                    name: 'Notation',
                                    rows: 5
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
