Ext.define('B4.view.boilerroom.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.form.FiasSelectAddress',
        'B4.base.Store',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.base.Store',
        'B4.model.boilerroom.HeatingPeriod',
        'B4.model.boilerroom.UnactivePeriod'
    ],

    alias: 'widget.boilerroomedit',

    modal: true,
    width: 750,
    height: 450,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Редактирование котельной',

    initComponent: function () {
        var me = this,
            heatingPeriodStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                model: 'B4.model.boilerroom.HeatingPeriod'
            }),
            unactivePeriodStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                model: 'B4.model.boilerroom.UnactivePeriod'
            });

        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5'
            },
            items: [
                {
                    xtype: 'hidden',
                    margin: '0 0 0 0',
                    name: 'Id'
                },
                {
                    xtype: 'b4fiasselectaddress',
                    name: 'Address',
                    fieldLabel: 'Адрес',
                    labelWidth: 180,
                    allowBlank: false,
                    fieldsRegex: {
                        tfHousing: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        },
                        tfBuilding: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        }
                    },
                    fieldsToHideNames: ['tfFlat', 'tfCoordinate']
                },
                {
                    xtype: 'textfield',
                    labelWidth: 180,
                    readOnly: true,
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование'
                },
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4grid',
                            flex: 1,
                            disabled: true,
                            name: 'HeatingPeriod',
                            store: heatingPeriodStore,
                            title: 'Период подачи тепла',
                            columns: [
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    text: 'Дата начала',
                                    dataIndex: 'Start',
                                    editor: {
                                        xtype: 'datefield'
                                    },
                                    flex: 1
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    text: 'Дата окончания',
                                    dataIndex: 'End',
                                    editor: {
                                        xtype: 'datefield'
                                    },
                                    flex: 1
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ],
                            plugins: [
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEditing'
                                })
                            ],
                            viewConfig: {
                                loadMask: true
                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            columns: 4,
                                            items: [
                                                {
                                                    xtype: 'b4addbutton'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: false,
                                    store: heatingPeriodStore,
                                    dock: 'bottom'
                                }
                            ]
                        },
                        {
                            xtype: 'b4grid',
                            flex: 1,
                            disabled: true,
                            name: 'UnactivePeriod',
                            store: unactivePeriodStore,
                            title: 'Период не активности',
                            columns: [
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    text: 'Дата начала',
                                    dataIndex: 'Start',
                                    editor: {
                                        xtype: 'datefield'
                                    },
                                    flex: 1
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    text: 'Дата окончания',
                                    dataIndex: 'End',
                                    editor: {
                                        xtype: 'datefield'
                                    },
                                    flex: 1
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ],
                            plugins: [
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEditing'
                                })
                            ],
                            viewConfig: {
                                loadMask: true
                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            columns: 4,
                                            items: [
                                                {
                                                    xtype: 'b4addbutton'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: false,
                                    store: unactivePeriodStore,
                                    dock: 'bottom'
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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