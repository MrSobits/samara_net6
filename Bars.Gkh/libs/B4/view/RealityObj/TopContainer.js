Ext.define('B4.view.realityobj.TopContainer', {
    extend: 'Ext.container.Container',
    alias: 'widget.realityobjtopcontainer',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.realityobj.RealityObjectConditionHouse'
    ],

    margin: '5 0 0 5',

    defaults: {
        flex: 1
    },

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
                                type: 'vbox',
                                align: 'stretch'
                            },
                            margin: '5 10',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 350,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'FiasAddress',
                                    fieldLabel: 'Адрес дома',
                                    flatIsVisible: false,
                                    allowBlank: false,
                                    itemId: 'fiasAddressRealityObject',
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    }
                                },
                                {
                                    fieldLabel: 'Код ФИАС',
                                    name: 'FiasHauseGuid',
                                    itemId: 'fiasHauseGuidRealityObject',
                                    readOnly: true,
                                    xtype: 'textfield'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1
                            },
                            margin: '0 10',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    flex: 4,
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'center'
                                            },
                                            width: 170,
                                            items: [
                                                {
                                                    // Картинка - аватар
                                                    xtype: 'container',
                                                    height: 140,
                                                    width: 145,
                                                    cls: 'photo-holder',
                                                    style: {
                                                        'border': '1px solid #dadada'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'image',
                                                            name: 'Avatar',
                                                            height: 140,
                                                            width: 145
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'buttongroup',
                                                    name: 'bgrAvatar',
                                                    columns: 1,
                                                    hidden: true,
                                                    buttonAlign: 'center',
                                                    margin: '5 0 0 0',
                                                    defaults: {
                                                        hidden: true
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'button',
                                                            name: 'btnAddPhoto',
                                                            iconCls: 'icon-add',
                                                            text: 'Добавить',
                                                            tooltip: 'Добавить фотографию объекта'
                                                        },
                                                        {
                                                            xtype: 'button',
                                                            name: 'btnDeletePhoto',
                                                            iconCls: 'icon-delete',
                                                            text: 'Удалить',
                                                            tooltip: 'Удалить фотографию'
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
                                            margin: '0 0 0 10',
                                            flex: 1,
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    floating: false,
                                                    name: 'TypeHouse',
                                                    fieldLabel: 'Тип дома',
                                                    displayField: 'Display',
                                                    store: B4.enums.TypeHouse.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    itemId: 'cbTypeHouseRealityObject'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'BuildYear',
                                                    fieldLabel: 'Год постройки',
                                                    hideTrigger: true,
                                                    allowDecimals: false,
                                                    minValue: 1800,
                                                    maxValue: 2100,
                                                    allowBlank: false,
                                                    negativeText: 'Значение не может быть отрицательным',
                                                    itemId: 'nfBuildYear'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    readOnly: true,
                                                    name: 'PublishDate',
                                                    fieldLabel: 'Дата включения в ДПКР'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    itemId: 'dfDateLastOverhaul',
                                                    format: 'd.m.Y',
                                                    allowBlank: true,
                                                    name: 'DateLastOverhaul',
                                                    fieldLabel: 'Дата последнего кап. ремонта'
                                                },
                                                {
                                                    xtype: 'combobox',
                                                    name: 'HasPrivatizedFlats',
                                                    itemId: 'hasPrivatizedFlats',
                                                    displayField: 'Display',
                                                    store: B4.enums.YesNoNotSet.getStore(),
                                                    valueField: 'Value',
                                                    fieldLabel: 'Наличие приватизированных квартир'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateTechInspection',
                                                    fieldLabel: 'Дата тех. обследования',
                                                    itemId: 'dfDateTechInspectionRealityObject'
                                                },
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    name: 'MethodFormFundCr',
                                                    fieldLabel: 'Предполагаемый способ формирования фонда КР',
                                                    displayField: 'Display',
                                                    store: B4.enums.MethodFormFundCr.getStore(),
                                                    valueField: 'Value'
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
                                    flex: 3,
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 170
                                    },
                                    margin: '0 0 0 30',
                                    items: [
                                        {
                                            xtype: 'realityobjectconditionhousecmp',
                                            editable: false,
                                            name: 'ConditionHouse',
                                            fieldLabel: 'Состояние дома'
                                        },
                                        {
                                            xtype: 'datefield',
                                            allowBlank: false,
                                            name: 'DateCommissioning',
                                            fieldLabel: 'Дата сдачи в эксплуатацию',
                                            itemId: 'dfDateComissioningRealityObject'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'UnpublishDate',
                                            fieldLabel: 'Дата исключения из ДПКР'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            allowBlank: false,
                                            name: 'DateCommissioningLastSection',
                                            fieldLabel: 'Дата сдачи в эксплуатацию последней секции дома',
                                            itemId: 'dfDateComissioningRealityObjectLastSection'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            allowBlank: false,
                                            name: 'PrivatizationDateFirstApartment',
                                            fieldLabel: 'Дата приватизации первого жилого помещения',
                                            itemId: 'dfPrivatizationDateFirstApartment'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'LatestTechnicalMonitoring',
                                            fieldLabel: 'Дата последнего тех. мониторинга',
                                            itemId: 'dfLatestTechnicalMonitoringRealityObject'
                                        }
                                    ]
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
