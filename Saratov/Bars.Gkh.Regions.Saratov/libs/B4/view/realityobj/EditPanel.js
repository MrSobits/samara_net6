Ext.define('B4.view.realityobj.EditPanel', {
    extend: 'Ext.form.Panel',
    moduleName: 'Gkh.Regions.Saratov',
    alias: 'widget.realityobjEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    frame: true,
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.store.dict.CapitalGroup',
        'B4.store.dict.TypeOwnership',
        'B4.store.dict.TypeProject',
        'B4.ux.button.Save',
        'B4.enums.TypeRoof',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.MethodFormFundCr',
        'B4.view.realityobj.RealityObjToolbar',
        'B4.store.dict.RoofingMaterial',
        'B4.store.dict.WallMaterial',
        'B4.form.field.plugin.InputMask',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'realityobjtoolbar'
                },
                {
                    //Верхний блок
                    xtype: 'container',
                    margin: '5 5 0 5',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'center'
                            },
                            columns: 3,
                            items:[
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
                            flex: 1,
                            padding: '0 0 5 0',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'FiasAddress',
                                    fieldLabel: 'Адрес',
                                    flatIsVisible: false,
                                    allowBlank: false,
                                    itemId: 'fiasAddressRealityObject',
                                    fieldsRegex: {
                                        tfHouse: {
                                            regex: /^\d+\/{0,1}\d*([А-Яа-я]{0,1})?$/,
                                            regexText: 'В это поле можно вводить значение следующих форматов: 33/22, 33/А, 33/22А, 33А'
                                        },
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
                                    xtype: 'combobox',
                                    editable: false,
                                    floating: false,
                                    name: 'TypeHouse',
                                    fieldLabel: 'Тип дома',
                                    displayField: 'Display',
                                    store: B4.enums.TypeHouse.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    itemId: 'cbTypeHouseRealityObject',
                                    maxWidth: 553
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            forPermission: 'ConditionHouse',
                                            name: 'ConditionHouse',
                                            fieldLabel: 'Состояние дома',
                                            displayField: 'Display',
                                            store: B4.enums.ConditionHouse.getStore(),
                                            valueField: 'Value',
                                            labelWidth: 180,
                                            width: 280,
                                            itemId: 'cbConditionHouseRealityObject'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'ResidentsEvicted',
                                            fieldLabel: 'Жильцы выселены',
                                            labelWidth: 255,
                                            store: B4.enums.ConditionHouse.getStore(),
                                            itemId: 'cbResidentsEvictedRealityObject'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'BuildYear',
                                            padding: '0 0 0 80',
                                            fieldLabel: 'Год постройки',
                                            hideTrigger: true,
                                            allowDecimals: false,
                                            minValue: 1800,
                                            maxValue: 2100,
                                            maxWidth: 280,
                                            width: 200,
                                            allowBlank: false,
                                            negativeText: 'Значение не может быть отрицательным',
                                            itemId: 'nfBuildYear'
                                        },
                                        {
                                            xtype: 'datefield',
                                            allowBlank: false,
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            width: 275,
                                            name: 'DateDemolition',
                                            fieldLabel: 'Дата сноса',
                                            labelWidth: 170,
                                            itemId: 'dfDateDemolutionRealityObject'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'datefield',
                                        labelAlign: 'right',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            width: 280,
                                            allowBlank: false,
                                            name: 'DateCommissioning',
                                            fieldLabel: 'Дата сдачи в эксплуатацию',
                                            labelWidth: 180,
                                            itemId: 'dfDateComissioningRealityObject'
                                        },
                                        {
                                            xtype: 'datefield',
                                            padding: '0 0 5 0',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            width: 275,
                                            name: 'DateTechInspection',
                                            fieldLabel: 'Дата тех. обследования',
                                            labelWidth: 170,
                                            itemId: 'dfDateTechInspectionRealityObject'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'datefield',
                                        labelAlign: 'right',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'HasPrivatizedFlats',
                                            itemId: 'hasPrivatizedFlats',
                                            fieldLabel: 'Наличие приватизированных квартир',
                                            width: 280,
                                            labelWidth: 260
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'IsNotInvolvedCr',
                                            labelAlign: 'right',
                                            fieldLabel: 'Дом не участвует в программе КР',
                                            labelWidth: 255
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelAlign: 'right',
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            allowBlank: false,
                            name: 'PrivatizationDateFirstApartment',
                            fieldLabel: 'Дата приватизации первого жилого помещения',
                            labelWidth: 330,
                            width: 430,
                            itemId: 'dfPrivatizationDateFirstApartment'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsRepairInadvisable',
                            fieldLabel: 'Ремонт нецелесообразен',
                            labelAlign: 'right',
                            labelWidth: 255,
                            padding: '4 0 0 0'
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    name: 'MethodFormFundCr',
                    fieldLabel: 'Предполагаемый способ формирования фонда КР',
                    displayField: 'Display',
                    store: B4.enums.MethodFormFundCr.getStore(),
                    valueField: 'Value',
                    labelAlign: 'right',
                    labelWidth: 330,
                    maxWidth: 703,
                    padding: '7 0 0 0'
                }
            ],
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие сведения',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FederalNum',
                                    fieldLabel: 'Федеральный номер',
                                    itemId: 'tfFederalNumRealityObject',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.TypeOwnership',
                                    name: 'TypeOwnership',
                                    fieldLabel: 'Форма собственности',
                                    allowBlank: false,
                                    editable: false,
                                    itemId: 'sfTypeOwnershipRealityObject'
                                }
                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'GkhCode',
                                    fieldLabel: 'Код дома',
                                    itemId: 'tfGkhCodeRealityObject',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsInsuredObject',
                                    fieldLabel: 'Объект застрахован',
                                    itemId: 'cbIsInsuredObjectRealityObject'
                                }
                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Серия, тип проекта',
                                    name: 'TypeProject',
                                    listView: 'B4.view.dict.typeproject.Grid',
                                    store: 'B4.store.dict.TypeProject',
                                    textProperty: 'Name',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    itemId: 'sfTypeProject'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'PhysicalWear',
                                    fieldLabel: 'Физический износ (%)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    maxValue: 100,
                                    minValue: 0,
                                    itemId: 'nfPhysicalWearRealityObject',
                                    decimalSeparator: ","
                                }
                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1,
                                allowBlank: false,
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.CapitalGroup',
                                    name: 'CapitalGroup',
                                    fieldLabel: 'Группа капитальности',
                                    itemId: 'sfCapitalGroupRealityObject'
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    store: 'B4.store.dict.BuildingFeature',
                                    name: 'BuildingFeature',
                                    allowBlank: true,
                                    fieldLabel: 'Особые отметки дома',
                                    itemId: 'sfBuildingFeatureRealityObject'
                                },
                                {
                                    xtype: 'combobox',
                                    floating: false,
                                    name: 'IsBuildSocialMortgage',
                                    fieldLabel: 'Построен по соц. ипотеке',
                                    displayField: 'Display',
                                    store: B4.enums.YesNo.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbIsBuildSocialMortgageRealObj'
                                }
                            ]
                        },
                        {
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1,
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'WebCameraUrl',
                                    fieldLabel: 'Адрес веб-камеры',
                                    itemId: 'tfWebCameraUrlRealityObject',
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CodeErc',
                                    fieldLabel: 'Код ЕРЦ',
                                    itemId: 'tfCodeErcRealityObject',
                                    maxLength: 250
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Общие характеристики',
                    autoScroll: true,
                    minWidth: 900,
                    layout: 'column',
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.55,
                            layout: 'anchor',
                            defaults: {
                                labelWidth: 300,
                                labelAlign: 'right',
                                anchor: '100%',
                                allowBlank: false
                            },
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
                                        labelWidth: 300
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'CadastreNumber',
                                            fieldLabel: 'Кадастровый номер земельного участка',
                                            maxLength: 300,
                                            itemId: 'tfCadastreNumber',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
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
                                        labelWidth: 300
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
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
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
                                        labelWidth: 300
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
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
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
                                        labelWidth: 300
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
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5',
                                            text: 'Заполнить',
                                            action: 'FillAreaOwned'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5',
                                            text: 'Заполнить',
                                            action: 'FillAreaMunicipalOwned'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5',
                                            text: 'Заполнить',
                                            action: 'FillAreaGovernmentOwned'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5',
                                            text: 'Заполнить',
                                            action: 'FillAreaLivingNotLivingMkd'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5',
                                            text: 'Заполнить',
                                            action: 'FillAreaLiving'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67,
                                            hidden: true
                                        }]
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
                                        labelWidth: 300
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
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
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
                                        labelWidth: 300
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
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
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
                                        labelWidth: 300
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
                                        },
                                        {
                                            xtype: 'label',
                                            html: '&nbsp;',
                                            margin: '0 0 0 5',
                                            width: 67
                                        }]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.45,
                            layout: 'anchor',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'MaximumFloors',
                                    fieldLabel: 'Максимальная этажность',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowBlank: false,
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
                                    allowBlank: false,
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
                                    minValue: 0,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberApartments',
                                    fieldLabel: 'Количество квартир',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowBlank: false,
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
                                    allowBlank: true,
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
                                    minValue: 0,
                                    allowBlank: false
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
                                    itemId: 'sfRoofingMaterialRealityObject'
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
                                        labelWidth: 200
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
