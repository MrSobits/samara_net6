Ext.define('B4.view.heatseason.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    maximized: true,
    resizable: true,
    itemId: 'heatSeasonEditWindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.HeatSeasonPeriodGji',
        'B4.store.RealityObject',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.HeatingSystem',
        'B4.enums.TypeHouse'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'panel',
                            height: 186,
                            layout: 'form',
                            split: false,
                            collapsible: false,
                            border: false,
                            padding: '5 5 5 5',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelWidth: 180
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                    name: 'Period',
                                    fieldLabel: 'Период отопительного сезона',
                                   

                                    store: 'B4.store.dict.HeatSeasonPeriodGji',
                                    editable: false,
                                    readOnly: true
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            labelAlign: 'right',
                                            itemId: 'realityObjectSelectField',
                                            name: 'RealityObject',
                                            textProperty: 'Address',
                                            fieldLabel: 'Жилой дом',
                                            isGetOnlyIdProperty: false,
                                            labelWidth: 180,
                                            flex: 1,
                                           

                                            store: 'B4.store.RealityObject',
                                            columns: [
                                                { text: 'Адрес', dataIndex: 'Address', flex: 1 }
                                            ],
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Жилой дом',
                                            itemId: 'btnEditRealityObj',
                                            width: 90,
                                            margin: '0 0 0 10',
                                            iconCls: 'icon-pencil-go'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 180
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateHeat',
                                            fieldLabel: 'Дата пуска тепла в дом',
                                            labelAlign: 'right',
                                            width: 300,
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'HeatingSystem',
                                            fieldLabel: 'Система отопления',
                                            displayField: 'Display',
                                            store: B4.enums.HeatingSystem.getStore(),
                                            valueField: 'Value',
                                            itemId: 'heatingSystemCombobox',
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        readOnly: true,
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 180,
                                        width: 300
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NumberEntrances',
                                            fieldLabel: 'Количество подъездов',
                                            itemId: 'tfNumberEntrances'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'AreaMkd',
                                            fieldLabel: 'Общая площадь МКД (кв.м.)',
                                            itemId: 'tfAreaMkd'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        readOnly: true,
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 180
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'MaximumFloors',
                                            fieldLabel: 'Максимальная этажность',
                                            itemId: 'tfMaximumFloors'
                                        },
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            name: 'TypeHouse',
                                            fieldLabel: 'Тип дома',
                                            itemId: 'cbTypeHouse',
                                            hideTrigger: true,
                                            displayField: 'Display',
                                            store: B4.enums.TypeHouse.getStore(),
                                            valueField: 'Value'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'anchor',
                                    defaults: {
                                        readOnly: true,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Floors',
                                            fieldLabel: 'Минимальная этажность',
                                            labelWidth: 180,
                                            itemId: 'tfFloors',
                                            anchor: '50%'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            padding: '5 5 5 5',
                            flex: 1,
                            defaults: {
                                bodyStyle: 'backrgound-color:transparent;'
                            },
                            items: [
                                {
                                    xtype: 'heatingseasondocgrid'
                                },
                                {
                                    xtype: 'heatingseasoninspectiongrid'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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