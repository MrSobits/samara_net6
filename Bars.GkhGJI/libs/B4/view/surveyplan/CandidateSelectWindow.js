Ext.define('B4.view.surveyplan.CandidateSelectWindow', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.store.dict.AuditPurposeGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.OrganizationForm'
    ],

    alias: 'widget.surveyPlanCandidateSelectWindow',

    minHeight: 700,
    minWidth: 1000,
    maximized: true,
    maximizable: true,

    layout: 'border',
    mixins: ['B4.mixins.window.ModalMask'],
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
    toolbarItems: [],
    //#endregion

    initComponent: function() {
        var me = this,
            isExistDelColumn = false;

        me.addEvents(
            'selectedgridrowactionhandler'
        );

        if (!me.columnsGridSelected) {
            me.columnsGridSelected = [];
        }

        Ext.Array.each(me.columnsGridSelected, function(value) {
            if (value.xtype == 'b4deletecolumn') {
                isExistDelColumn = true;
            }
        });

        if (!isExistDelColumn) {
            me.columnsGridSelected.push({
                xtype: 'b4deletecolumn',
                handler: function(gridView, rowIndex, colIndex, el, e, rec) {
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

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    border: false,
                    region: 'north',
                    padding: 5,
                    defaults: {
                        labelWidth: 200
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            width: 700,
                            name: 'AuditPurpose',
                            fieldLabel: 'Цель проверки',
                            store: 'B4.store.dict.AuditPurposeGji',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            width: 700,
                            name: 'Year',
                            fieldLabel: 'Год'
                        },
                        {
                            xtype: 'b4selectfield',
                            width: 700,
                            name: 'Municipality',
                            fieldLabel: 'Муниципальный район',
                            store: 'B4.store.dict.Municipality',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    labelWidth: 200,
                                    width: 700,
                                    name: 'Okopf',
                                    fieldLabel: 'Организационно-правовая форма',
                                    store: 'B4.store.dict.OrganizationForm',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    text: 'Обновить',
                                    margin: '0 0 0 5'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'center',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            layout: 'fit',
                            flex: 1,
                            autoScroll: true,
                            border: false,
                            style: 'border-right: solid 1px #99bce8;',
                            items: [
                                Ext.merge({
                                    xtype: 'b4grid',
                                    allowDeselect: true,
                                    rowLines: false,
                                    itemId: 'multiSelectGrid',
                                    title: me.titleGridSelect,
                                    border: false,
                                    provideStateId: Ext.emptyFn,
                                    stateful: false,
                                    store: me.storeSelect,
                                    selModel: Ext.create('Ext.selection.CheckboxModel', {
                                        mode: me.selModelMode,
                                        checkOnly: true,
                                        ignoreRightMouseSelection: true
                                    }),
                                    columns: me.columnsGridSelect,
                                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                                    viewConfig: {
                                        loadMask: true
                                    },
                                    dockedItems: [
                                        me.leftTopToolbarConfig,
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: me.storeSelect,
                                            dock: 'bottom'
                                        }
                                    ]
                                }, me.leftGridConfig)
                            ]
                        },
                        {
                            xtype: 'panel',
                            layout: 'fit',
                            flex: 1,
                            autoScroll: true,
                            border: false,
                            items: [
                                Ext.merge({
                                    xtype: 'b4grid',
                                    itemId: 'multiSelectedGrid',
                                    border: false,
                                    provideStateId: Ext.emptyFn,
                                    stateful: false,
                                    title: me.titleGridSelected,
                                    store: me.storeSelected,
                                    columns: me.columnsGridSelected,
                                    listeners: {
                                        afterrender: function() {
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
                                    text: 'Применить'
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