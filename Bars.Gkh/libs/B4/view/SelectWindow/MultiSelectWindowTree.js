Ext.define('B4.view.SelectWindow.MultiSelectWindowTree', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.Panel',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'multiSelectWindowTree',
    closeAction: 'hide',
    height: 500,
    width: 950,
    layout: 'fit',
    maximizable: true,
    mixins: [ 'B4.mixins.window.ModalMask' ],
    trackResetOnLoad: true,
    title: 'Выбор элементов',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    //storeSelect должен extend Ext.data.TreeStore
    storeSelect: null,
    storeSelected: null,
    columnsGridSelected: [],

    initComponent: function () {
        var me = this,
            tbar,
            isExistDelColumn = false;
        debugger;
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

        if (me.isTbar) {
            tbar = me.tbarCmp || [
                {
                    xtype: 'textfield',
                    itemId: 'tfSearchName',
                    emptyText: 'Поиск'
                },
                {
                    xtype: 'b4updatebutton',
                    itemId: 'btnUpdateTree'
                },
                {
                    xtype: 'tbseparator'
                },
                {
                    iconCls: 'icon-accept',
                    text: 'Выбрать все',
                    itemId: 'btnMarkAll'
                },
                {
                    iconCls: 'icon-decline',
                    text: 'Снять все отметки',
                    itemId: 'btnUnmarkAll'
                }
            ];
        }

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox', 
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'treepanel',
                            itemId: 'tpSelect',
                            border: false,
                            // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
                            provideStateId: Ext.emptyFn,
                            stateful: false,
                            columns: me.columnsGridSelect,
                            animate: true,
                            autoScroll: true,
                            useArrows: true,
                            containerScroll: true,
                            flex: 1,
                            loadMask: true,
                            store: me.storeSelect,
                            rootVisible: false,
                            title: me.titleGridSelect,
                            viewConfig: {
                                loadMask: true
                            },
                            tbar: tbar
                        },
                        {
                            xtype: 'b4grid',
                            border: false,
                            // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
                            provideStateId: Ext.emptyFn,
                            stateful: false,
                            flex: 1,
                            style: 'border-left: 1px solid #99bce8',
                            itemId: 'multiSelectedGrid',
                            border: false,
                            title: me.titleGridSelected,
                            store: me.storeSelected,
                            columns: me.columnsGridSelected
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
                                    xtype: 'b4savebutton',
                                    text: 'Применить'
                                }
                            ]
                        },
                        '->',
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