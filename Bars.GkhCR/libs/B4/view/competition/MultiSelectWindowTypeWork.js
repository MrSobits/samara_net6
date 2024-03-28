Ext.define('B4.view.competition.MultiSelectWindowTypeWork', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr'
    ],

    itemId: 'multiSelectWindowTypeWork',
    closeAction: 'destroy',
    height: 500,
    width: 900,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
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
                    xtype: 'container',
                    margin: '5 5',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ProgramCr',
                            flex: 2,
                            textProperty: 'Name',
                            fieldLabel: 'Программа КР',
                            lableWidth: 170,
                            width: 400,
                            store: 'B4.store.dict.ProgramCr',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', filter: 'textfield', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'component',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    flex:1,
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
                    padding: '0 0 5 0',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton', text: 'Применить' }]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});