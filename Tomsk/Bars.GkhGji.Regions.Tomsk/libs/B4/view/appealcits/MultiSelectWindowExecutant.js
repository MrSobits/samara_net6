Ext.define('B4.view.appealcits.MultiSelectWindowExecutant', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'multiSelectWindowExecutant',
    closeAction: 'hide',
    height: 500,
    width: 900,
    layout: 'fit',
    mixins: ['B4.mixins.window.ModalMask'],
    maximizable: true,
    trackResetOnLoad: true,
    title: 'Выбор элементов',
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
                                    selModel: Ext.create('Ext.selection.CheckboxModel', { mode: this.selModelMode }),
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
                    items: [
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            columns: 2,
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'PerformanceDate',
                                    itemId: 'dfPerformanceDate',
                                    fieldLabel: 'Срок исполнения',
                                    format: 'd.m.Y',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Author',
                                    itemId: 'sflAuthor',
                                    allowBlank: false,
                                    fieldLabel: 'Поручитель',
                                    flex: 1,
                                    textProperty: 'Fio',
                                    store: 'B4.store.dict.Inspector',
                                    columns: [
                                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    listeners: {
                                        beforeload: function (cmp, options) {
                                            options.params.headOnly = true;
                                        }
                                    }
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