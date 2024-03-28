Ext.define('B4.view.dict.mocoefficient.MultiSelectWindowMunicipality', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.cscalculation.CategoryCSMKD'
    ],

    itemId: 'mocoefficientmultiSelectWindowMunicipality',
    closeAction: 'hide',
    height: 500,
    width: 850,
    layout: 'fit',
    mixins: ['B4.mixins.window.ModalMask'],
    maximizable: true,
    trackResetOnLoad: true,
    title: 'Выбор муниципальных образований',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    storeSelect: null,
    storeSelected: null,
    columnsGridSelect: [],
    columnsGridSelected: [],
    selModelMode: null, //по умолчанию аспект передает 'MULTI'

    initComponent: function () {
        var me = this;

        me.addEvents(
            'selectedgridrowactionhandler'
        );

        if (!this.columnsGridSelected)
            this.columnsGridSelected = [];

        var isExistDelColumn = false;
        Ext.Array.each(this.columnsGridSelected, function (value) {
            if (value.xtype == 'b4deletecolumn') {
                isExistDelColumn = true;
            }
        });
        if (!isExistDelColumn) {
            this.columnsGridSelected.push(
                {
                    xtype: 'b4deletecolumn',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        me.fireEvent('selectedgridrowactionhandler', 'delete', rec);
                    }
                });
        }

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            flex: 2,
                            layout: 'fit',
                            border: false,
                            items: [
                                {
                                    xtype: 'b4grid',
                                    itemId: 'multiSelectGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    title: this.titleGridSelect,
                                    border: false,
                                    store: this.storeSelect,
                                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {
                                        mode: me.selModelMode,
                                        checkOnly: true,
                                        ignoreRightMouseSelection: true
                                    }),
                                    columns: this.columnsGridSelect,
                                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                                    columns: 2,
                                                    items: [
                                                        {
                                                            xtype: 'b4updatebutton'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: this.storeSelect,
                                            dock: 'bottom'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            itemId: 'multiSelectedPanel',
                            split: false,
                            collapsible: false,
                            border: false,
                            height: 200,
                            flex: 1,
                            style: {
                                border: 'solid #99bce8',
                                borderWidth: '0 0 0 1px'
                            },
                            layout: 'fit',
                            items: [
                                {
                                    xtype: 'b4grid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    itemId: 'multiSelectedGrid',
                                    border: false,
                                    title: this.titleGridSelected,
                                    store: this.storeSelected,
                                    columns: this.columnsGridSelected,
                                    listeners: {
                                        afterrender: function () {
                                            var store = this.getStore();
                                            if (store)
                                                store.pageSize = store.getCount();
                                        }
                                    },
                                    dockedItems: [
                                        {
                                            xtype: 'toolbar',
                                            dock: 'bottom',
                                            items: [
                                                {
                                                    xtype: 'tbtext',
                                                    ref: 'status',
                                                    height: 16,
                                                    margin: 3,
                                                    text: '0 записи'
                                                }
                                            ]
                                        }
                                    ]
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
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            columns: 2,
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    flex:2,
                                    allowBlank: false,
                                    itemId: 'tfName',
                                    hidden: false,
                                    fieldLabel: 'Наименование'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Категория',
                                    name: 'CategoryCSMKD',
                                    itemId: 'sfCategoryCSMKD',
                                    flex: 1,
                                    store: 'B4.store.cscalculation.CategoryCSMKD',
                                    editable: false,
                                    allowBlank: true,
                                    columns: [
                                        {
                                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            columns: 3,
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    allowBlank: false,
                                    itemId: 'tfCode',
                                    hidden: false,
                                    fieldLabel: 'Код'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Value',
                                    allowBlank: false,
                                    itemId: 'tfValue',
                                    decimalPrecision: 4,
                                    fieldLabel: 'Значение'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'UnitMeasure',
                                    itemId: 'tfUnitMeasure',
                                    allowBlank: true,
                                    hidden: false,
                                    fieldLabel: 'Ед.изм.'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            columns:2,
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateFrom',
                                    fieldLabel: 'Дата с',
                                    format: 'd.m.Y',
                                    itemId: 'dfDateFrom',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateTo',
                                    fieldLabel: 'по',
                                    format: 'd.m.Y',
                                    itemId: 'dfDateTo',
                                    allowBlank: true
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Применить'
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