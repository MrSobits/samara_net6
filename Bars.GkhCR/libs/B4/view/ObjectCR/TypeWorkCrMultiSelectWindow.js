Ext.define('B4.view.objectcr.TypeWorkCrMultiSelectWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.typeworkcrmultiselectwindow',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.store.dict.FinanceSource',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'typeWorkCrMultiSelectWindow',
    closeAction: 'destroy',
    height: 500,
    width: 600,
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
    selModelMode: 'SINGLE', //по умолчанию аспект передает 'MULTI'

    // чтобы не сохранялись настройки колонок в localStorage
    provideStateId: Ext.emptyFn,
    stateful: false,
    
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
                            flex: 1,
                            layout: 'fit',
                            border: false,
                            items: [
                                {
                                    xtype: 'b4grid',
                                    itemId: 'multiSelectGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    border: false,
                                    // чтобы не сохранялись настройки колонок в localStorage
                                    provideStateId: Ext.emptyFn,
                                    stateful: false,
                                    
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
                                                    items: [
                                                        {
                                                            xtype: 'b4selectfield',
                                                            name: 'FinanceSource',
                                                            labelWidth: 150,
                                                            width: 500,
                                                            flex:1,
                                                            labelAlign: 'right',
                                                            fieldLabel: 'Разрез финансирования',
                                                            store: 'B4.store.dict.FinanceSource',
                                                            allowBlank: true
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
                            hidden: true, //попросили скрыть правый грид поскольку выбирается только одна запись 
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
                                    // чтобы не сохранялись настройки колонок в localStorage
                                    provideStateId: Ext.emptyFn,
                                    stateful: false,
                                    
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
                                    }
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
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Сохранить'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
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