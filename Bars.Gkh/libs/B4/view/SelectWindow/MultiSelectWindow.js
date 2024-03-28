Ext.define('B4.view.SelectWindow.MultiSelectWindow', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.Panel',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'multiSelectWindow',
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

    //#region extending
    leftGridConfig: {},
    leftTopToolbarConfig: {},
    rightGridConfig: {},
    saveButtonText: null,
    toolbarItems: [],
    isPaginable: true,
    //#endregion

    initComponent: function () {
        var me = this,
            isExistDelColumn = false;

        me.addEvents(
            'selectedgridrowactionhandler'
        );

        if (!me.columnsGridSelected) {
            me.columnsGridSelected = [];
        }

        Ext.Array.each(me.columnsGridSelected, function (value) {
            if (value.xtype == 'b4deletecolumn') {
                isExistDelColumn = true;
            }
        });

        if (!isExistDelColumn) {
            me.columnsGridSelected.push({
                xtype: 'b4deletecolumn',
                handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                    me.fireEvent('selectedgridrowactionhandler', 'delete', rec);
                }
            });
        }
        
        if (!me.leftTopToolbarConfig) {
            me.leftTopToolbarConfig = {
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
            };
        }

        var dockedItems = [me.leftTopToolbarConfig];
        if (me.isPaginable) {
            dockedItems.push(
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: me.storeSelect,
                dock: 'bottom'
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
                        Ext.merge({
                            xtype: 'b4grid',
                            flex: 1.3,
                            allowDeselect: true,
                            rowLines: false,
                            itemId: 'multiSelectGrid',
                            title: me.titleGridSelect,
                            style: 'border-right: solid 1px #99bce8;',
                            border: false,
                            // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
                            provideStateId: Ext.emptyFn,
                            stateful: false,
                            store: me.storeSelect,
                            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {
                                mode: me.selModelMode,
                                checkOnly: true,
                                deselectAllOnDeselect: false,
                                ignoreRightMouseSelection: true,
                                selectAll : function() {
                                    var me = this,
                                        window = me.view.up('window'),
                                        selectedStore = window.storeSelected,
                                        selections = me.store.getRange(),
                                        start = me.getSelection().length;

                                    me.bulkChange = true;

                                    me.doSelect(selections, true, true);

                                    delete me.bulkChange;

                                    me.maybeFireSelectionChange(me.getSelection().length !== start);

                                    // сделано так для производительности
                                    // иначе, при удалении каждой записи обновляется грид через события
                                    if (selectedStore) {
                                        selectedStore.removeAll();
                                        for (var i = 0; i < selections.length; i++) {
                                            selectedStore.insert(i, selections[i]);
                                        }
                                    }

                                    this.isSelectedAll = true;
                                }
                            }),
                            columns: me.columnsGridSelect,
                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                            viewConfig: {
                                loadMask: true
                            },
                            dockedItems: dockedItems
                        }, me.leftGridConfig),
                        Ext.merge({
                            xtype: 'b4grid',
                            flex: 0.7,
                            itemId: 'multiSelectedGrid',
                            border: false,
                            // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
                            provideStateId: Ext.emptyFn,
                            stateful: false,
                            title: me.titleGridSelected,
                            store: me.storeSelected,
                            columns: me.columnsGridSelected,
                            listeners: {
                                afterrender: function () {
                                    var store = this.getStore();
                                    if (store) {
                                        store.pageSize = store.getCount();
                                    }
                                }
                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'tbfill',
                                            margin: 13
                                        },
                                        {
                                            xtype: 'buttongroup',
                                            items: [
                                                {
                                                    xtype: 'b4deletebutton',
                                                    action: 'deselectAll',
                                                    text: 'Удалить все'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'toolbar',
                                    dock: 'bottom',
                                    items: [
                                        '->',
                                        {
                                            xtype: 'tbtext',
                                            margin: 3,
                                            ref: 'status',
                                            text: 'Нет записей'
                                        }
                                    ]
                                }
                            ]
                        }, me.rightGridConfig)
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
                            columns: 1 + me.toolbarItems.length,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: me.saveButtonText || 'Применить'
                                }
                            ].concat(me.toolbarItems)
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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