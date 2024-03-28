Ext.define('B4.view.longtermprobject.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    alias: 'widget.longtermprobjectEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.form.field.plugin.InputMask',

        'B4.store.dict.Municipality',
        'B4.store.dict.CapitalGroup',
        'B4.store.dict.TypeOwnership',
        'B4.store.dict.TypeProject',

        'B4.ux.button.Save',

        'B4.enums.TypeHouse',
        'B4.enums.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeRoof',
        'B4.enums.ConditionHouse',
        'B4.enums.MethodFormFundCr',
        'B4.enums.HeatingSystem',

        'B4.view.realityobj.RealityObjToolbar'
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Паспорт дома',
                                    textAlign: 'left',
                                    name: 'btnShowHouseDetails'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'container',
                    margin: '15 5 0 5',
                    autoScroll: true,
                    minWidth: 900,
                    layout: { type: 'hbox' },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 300,
                                labelAlign: 'right',
                                editable: false,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'TypeHouse',
                                    fieldLabel: 'Тип дома',
                                    displayField: 'Display',
                                    store: B4.enums.TypeHouse.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbTypeHouseRealityObject'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'BuildYear',
                                    fieldLabel: 'Год постройки',
                                    margin: '10 0 10 0',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    minValue: 1800,
                                    maxValue: 2100,
                                    negativeText: 'Значение не может быть отрицательным',
                                    itemId: 'nfBuildYear'
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
                                    itemId: 'nfAreaLivingNotLivingMkdRealityObject',
                                    decimalSeparator: ',',
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaLivingOwned',
                                    fieldLabel: 'В т.ч. жилых, находящихся в собственности граждан (кв.м.)',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    itemId: 'nfAreaLivingOwnedRealityObject',
                                    minValue: 0
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 330,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'ConditionHouse',
                                    fieldLabel: 'Состояние дома',
                                    displayField: 'Display',
                                    store: B4.enums.ConditionHouse.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    margin: '10 0 10 0',
                                    name: 'DateCommissioning',
                                    maxWidth: 430,
                                    fieldLabel: 'Дата сдачи в эксплуатацию',
                                    itemId: 'dfDateComissioningRealityObject'
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    margin: '5 0 5 0',
                                    maxWidth: 430,
                                    name: 'PrivatizationDateFirstApartment',
                                    fieldLabel: 'Дата приватизации первого жилого помещения',
                                    itemId: 'dfPrivatizationDateFirstApartment'
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    width: 270,
                                    name: 'DateDemolition',
                                    fieldLabel: 'Дата сноса',
                                    labelWidth: 170,
                                    itemId: 'dfDateDemolutionRealityObject',
                                    hidden: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaLiving',
                                    fieldLabel: 'В т.ч. жилых всего (кв.м.)',
                                    hideTrigger: true,
                                    margin: '9 0 8 0',
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    itemId: 'nfAreaLivingRealityObject',
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
                                    margin: '9 0 8 0',
                                    decimalSeparator: ',',
                                    itemId: 'nfAreaNotLivingFunctional',
                                    allowBlank: true,
                                    minValue: 0
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