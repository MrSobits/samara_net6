Ext.define('B4.view.realityobj.GeneralInfoContainer', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.realityobjgeneralinfocontainer',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Общие сведения',

    defaults: {
        flex: 1
    },

    margin: '0 5',

    requires: [
        'B4.form.FileField'
    ],

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
                    flex: 4,
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            width: 170,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                padding: '2 0 3 0'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'ResidentsEvicted',
                                    fieldLabel: 'Жильцы выселены',
                                    itemId: 'cbResidentsEvictedRealityObject'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsInsuredObject',
                                    fieldLabel: 'Объект застрахован',
                                    itemId: 'cbIsInsuredObjectRealityObject'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsNotInvolvedCr',
                                    fieldLabel: 'Дом не участвует в КР'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsRepairInadvisable',
                                    fieldLabel: 'Ремонт нецелесообразен'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsInvolvedCrTo2',
                                    fieldLabel: 'Участвует в программе КР ТО №2'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsSubProgram',
                                    fieldLabel: 'Дом в подпрограмме',
                                    disabled: true
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IncludeToSubProgram',
                                    fieldLabel: 'Включить в подпрограмму'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'ExportedToPortal',
                                    fieldLabel: 'Экспортировать на портал'
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
                                    xtype: 'datefield',
                                    allowBlank: false,
                                    format: 'd.m.Y',
                                    name: 'DateDemolition',
                                    fieldLabel: 'Дата сноса',
                                    itemId: 'dfDateDemolutionRealityObject'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FederalNum',
                                    fieldLabel: 'Федеральный номер',
                                    itemId: 'tfFederalNumRealityObject',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GkhCode',
                                    fieldLabel: 'Код дома',
                                    itemId: 'tfGkhCodeRealityObject',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'AddressCode',
                                    fieldLabel: 'Код адреса',
                                    itemId: 'tfAddressCodeRealityObject',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CodeErc',
                                    fieldLabel: 'Код ЕРЦ',
                                    itemId: 'tfCodeErcRealityObject',
                                    maxLength: 250
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
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'VtscpCode',
                                    fieldLabel: 'Код дома для ВЦКП',
                                    maxLength: 20,
                                    regex: /^[0-9]+:[0-9]+$/
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margins: '0 0 5 0',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'Iscluttered',
                                            fieldLabel: 'МОП захламлены'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            labelWidth: 180,
                                            name: 'HasVidecam',
                                            fieldLabel: 'Наличие видеонаблюдения'
                                        },
                                        {
                                            xtype: 'component',
                                            flex: 0.1
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Уведомить ГЖИ',
                                            iconCls: 'icon-application-go',
                                            textAlign: 'left',
                                            itemId: 'btnPrescr',
                                            flex: 0.3
                                        },
                                        {
                                            xtype: 'component',
                                            flex: 0.5
                                        },
                                        
                                    ]
                                },              
                                {
                                    xtype: 'combobox',
                                    floating: false,
                                    name: 'BuiltOnResettlementProgram',
                                    fieldLabel: 'Построен по программе переселения',
                                    displayField: 'Display',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    valueField: 'Value',
                                    itemId: 'builtOnResettlementProgram'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'TechPassportFile',
                                    fieldLabel: 'Электронный паспорт дома',
                                    hidden: true
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
                    margin: '0 0 0 30',
                    flex: 3,
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                        
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            store: 'B4.store.dict.BuildingFeature',
                            name: 'BuildingFeature',
                            allowBlank: true,
                            fieldLabel: 'Категория благоустройства',
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
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.TypeOwnership',
                            name: 'TypeOwnership',
                            fieldLabel: 'Форма собственности',
                            allowBlank: false,
                            editable: false,
                            itemId: 'sfTypeOwnershipRealityObject'
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
                            ],
                            itemId: 'sfTypeProject'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.RealEstateType',
                            name: 'RealEstateType',
                            fieldLabel: 'Классификация дома',
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'RealEstateTypeNames',
                            readOnly: true,
                            fieldLabel: 'Классификация дома для расчета начислений'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.CapitalGroup',
                            name: 'CapitalGroup',
                            fieldLabel: 'Группа капитальности',
                            itemId: 'sfCapitalGroupRealityObject'
                        },
                        {
                            xtype: 'textfield',
                            name: 'WebCameraUrl',
                            fieldLabel: 'Адрес веб-камеры',
                            itemId: 'tfWebCameraUrlRealityObject',
                            maxLength: 1000
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileInfo',
                            fieldLabel: 'Справка из БТИ'
                        },
                        {
                            xtype: 'combobox',
                            name: 'ObjectConstruction',
                            itemId: 'objectConstruction',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            fieldLabel: 'Объект строительства'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
