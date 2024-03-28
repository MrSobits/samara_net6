Ext.define('B4.view.dict.templateservice.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 800,
    height: 450,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Услуга',
    itemId: 'templateServiceEditWindow',
    layout: { type: 'vbox', align: 'stretch' },
    closable: false,

    requires: [
        'B4.Date',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.view.dict.templateservice.OptionFieldsGrid',
        'B4.enums.TypeGroupServiceDi',
        'B4.enums.TypeCommunalResource',
        'B4.enums.TypeHousingResource',
        'B4.enums.KindServiceDi'
    ],

    initComponent: function () {
        var me = this,
            years = Ext.Array.map(B4.Date.rangeYears(new Date('2011')), function(a) {return [a, a]});

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Characteristic',
                            fieldLabel: 'Характеристика',
                            maxLength: 300
                        },
                        {
                            xtype: 'container',
                            anchor: '100%',
                            margin: '0 0 10px 0',
                            defaults: {
                                xtype: 'container',
                                layout: 'anchor',
                                flex: 1,
                                defaults: {
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    labelWidth: 100
                                }
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'UnitMeasure',
                                            fieldLabel: 'Ед. измерения',
                                            anchor: '100%',
                                            store: 'B4.store.dict.UnitMeasure'
                                        },
                                        {
                                            xtype: 'combobox', editable: false,
                                            fieldLabel: 'Группа услуги',
                                            store: B4.enums.TypeGroupServiceDi.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'TypeGroupServiceDi'
                                        },
                                        {
                                            xtype: 'combobox', editable: false,
                                            fieldLabel: 'Вид услуги',
                                            store: B4.enums.KindServiceDi.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'KindServiceDi',
                                            itemId: 'cbKindServiceDi'
                                        },
                                        {
                                            xtype: 'b4combobox', editable: false,
                                            fieldLabel: 'Вид коммунального ресурса',
                                            store: B4.enums.TypeCommunalResource.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'CommunalResourceType',
                                            emptyItem: { Display: 'Не задано' },
                                            hidden: true,
                                        },
                                        {
                                            xtype: 'b4combobox', editable: false,
                                            fieldLabel: 'Вид жилищной услуги',
                                            store: B4.enums.TypeHousingResource.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'HousingResourceType',
                                            emptyItem: { Display: 'Не задано' },
                                            hidden: true,
                                        }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'Changeable',
                                            boxLabel: 'Изменяемая',
                                            margin: '0 0 0 100px'
                                        },
                                         {
                                             xtype: 'checkbox',
                                             name: 'IsMandatory',
                                             boxLabel: 'Является обязательной',
                                             margin: '0 0 0 100px'
                                         },
                                        {
                                            xtype: 'checkbox',
                                            name: 'IsConsiderInCalc',
                                            boxLabel: 'Учитывать при расчете процента',
                                            margin: '0 0 0 100px'
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'b4combobox', 
                                                items: years,
                                                labelAlign: 'right',
                                                flex: 2, 
                                                allowblank: true
                                            },
                                            items: [
                                                {
                                                    name: 'ActualYearStart',
                                                    fieldLabel: 'С'
                                                },
                                                {
                                                    name: 'ActualYearEnd',
                                                    fieldLabel: 'По'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'templateserviceoptionfieldsgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
