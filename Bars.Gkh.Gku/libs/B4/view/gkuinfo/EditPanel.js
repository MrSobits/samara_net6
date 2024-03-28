Ext.define('B4.view.gkuinfo.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    alias: 'widget.gkuinfoeditpanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
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

        'B4.enums.HeatingSystem',
        'B4.enums.TypeRoof',
        'B4.view.dict.typeproject.Grid',
        'B4.view.dict.capitalgroup.Grid',
        'B4.view.gkuinfo.TarifsGrid'
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие характеристики',
                    minWidth: 700,
                    layout: 'column',
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.45,
                            layout: 'anchor',
                            defaults: {
                                labelWidth: 300,
                                labelAlign: 'right',
                                anchor: '100%',
                                readOly: true
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'FiasAddress',
                                    fieldLabel: 'Адрес',
                                    flatIsVisible: false,
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
                                    xtype: 'combobox',
                                    editable: false,
                                    floating: false,
                                    name: 'TypeHouse',
                                    fieldLabel: 'Тип дома',
                                    displayField: 'Display',
                                    store: B4.enums.TypeHouse.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    name: 'ConditionHouse',
                                    fieldLabel: 'Состояние дома',
                                    displayField: 'Display',
                                    store: B4.enums.ConditionHouse.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaMkd',
                                    fieldLabel: 'Общая площадь МКД (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaLivingNotLivingMkd',
                                    fieldLabel: 'Общая площадь жилых и нежилых помещений (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaLiving',
                                    fieldLabel: 'В т.ч. жилых всего (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    anchor: '100%',
                                    name: 'AreaLivingOwned',
                                    fieldLabel: 'В т.ч. жилых, находящихся в собственности граждан (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaNotLivingFunctional',
                                    fieldLabel: 'В т.ч. нежилых помещений, функционального назначения (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    allowBlank: true,
                                    minValue: 0
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.WallMaterial',
                                    name: 'WallMaterial',
                                    fieldLabel: 'Материал стен',
                                    editable: false
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.RoofingMaterial',
                                    name: 'RoofingMaterial',
                                    fieldLabel: 'Материал кровли',
                                    editable: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'TypeRoof',
                                    fieldLabel: 'Тип кровли',
                                    displayField: 'Display',
                                    store: B4.enums.TypeRoof.getStore(),
                                    valueField: 'Value',
                                    editable: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'HeatingSystem',
                                    fieldLabel: 'Система отопления',
                                    displayField: 'Display',
                                    store: B4.enums.HeatingSystem.getStore(),
                                    valueField: 'Value',
                                    editable: false
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.55,
                            layout: 'anchor',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                anchor: '100%',
                                readOly: true
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'BuildYear',
                                    fieldLabel: 'Год постройки',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    minValue: 1800,
                                    maxValue: 2100,
                                    negativeText: 'Значение не может быть отрицательным'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateCommissioning',
                                    fieldLabel: 'Дата сдачи в эксплуатацию'
                                },
                                {
                                    xtype: 'hiddenfield',
                                    name: 'Id'
                                },
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'PrivatizationDateFirstApartment',
                                    fieldLabel: 'Дата приватизации первого жилого помещения'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'MaximumFloors',
                                    fieldLabel: 'Максимальная этажность',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
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
                                    allowDecimals: false,
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Floors',
                                    fieldLabel: 'Количество этажей',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowDecimals: false,
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberEntrances',
                                    fieldLabel: 'Количество подъездов',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
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
                                    allowDecimals: false,
                                    minValue: 0
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.CapitalGroup',
                                    name: 'CapitalGroup',
                                    fieldLabel: 'Группа капитальности'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Серия, тип проекта',
                                    name: 'TypeProject',
                                   

                                    store: 'B4.store.dict.TypeProject',
                                    textProperty: 'Name',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
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
                                    decimalSeparator: ","
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberLifts',
                                    fieldLabel: 'Количество лифтов',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowDecimals: false,
                                    minValue: 0
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        align: 'stretch',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            flex: 1,
                            title: 'Количество счетов физичексих лиц',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                anchor: '100%',
                                readOly: true
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'IndividualAccountsCount',
                                    fieldLabel: 'Общее',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'IndividualTenantAccountsCount',
                                    fieldLabel: 'Собственников',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'IndividualOwnerAccountsCount',
                                    fieldLabel: 'Нанимателей',
                                    hideTrigger: true
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            flex: 1,
                            margin: '0 0 0 10',
                            title: 'Количество счетов юридических лиц',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                anchor: '100%',
                                readOly: true
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'LegalAccountsCount',
                                    fieldLabel: 'Общее',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'LegalTenantAccountsCount',
                                    fieldLabel: 'Собственников',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'LegalOwnerAccountsCount',
                                    fieldLabel: 'Арендаторов',
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    flex: 1,
                    xtype: 'gkuinfotarifgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});