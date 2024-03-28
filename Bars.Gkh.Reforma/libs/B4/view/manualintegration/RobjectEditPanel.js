Ext.define('B4.view.manualintegration.RobjectEditPanel', {
    extend: 'Ext.form.Panel',

    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    width: 1100,
    height: 600,
    bodyStyle: Gkh.bodyStyle,
    itemId: 'manualintegrationRobjectPanel',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging'
    ],

    storeSelect: null,
    storeSelected: null,
    columnsGridSelect: [],
    columnsGridSelected: [],
    selModelMode: 'MULTI',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    rightGridToolbar: [],
    leftGridToolbar: [
        {
            xtype: 'button',
            text: 'Провести интеграцию',
            itemId: 'btnIntegrate',
            iconCls: 'icon-application-get'
        },
        {
            xtype: 'button',
            itemId: 'btnSelectAll',
            text: 'Выбрать всё',
            iconCls: 'icon-basket-add',
            action: 'integrateAll'
        }
    ],

    initComponent: function() {
        var me = this;

        me.addEvents('selectedgridrowactionhandler');
        me.storeSelect = Ext.create('B4.store.manualintegration.RefRealityObject'),
        me.storeSelected = Ext.create('B4.store.manualintegration.RefRealityObjectSelected'),
        me.relayEvents(me.storeSelect, ['beforeload'], 'storeSelect.');

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    flex: 1,
                    width: 1100,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4grid',
                            name: 'selectGrid',
                            store: me.storeSelect,
                            type: 'multiSelectGrid',
                            width: 550,
                            style: 'border-top: solid #99bce8 1px; border-right: solid #99bce8 1px;',
                            title: "Выбрать записи",
                            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: me.selModelMode }),
                            itemId: 'realityObjIntegrationGrid',
                            columnLines: true,
                            columns: [
                                {
                                    header: 'Номер',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ExternalId',
                                    width: 70,
                                    filter: { xtype: 'numberfield', hideTrigger: true }
                                },
                                {
                                    header: 'Адрес дома',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Address',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
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
                                            columns: me.leftGridToolbar.length,
                                            items: me.leftGridToolbar
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    store: me.storeSelect,
                                    dock: 'bottom'
                                }
                            ]
                        },
                        {
                            xtype: 'b4grid',
                            flex: 1,
                            style: 'border-top: solid #99bce8 1px;',
                            type: 'multiSelectedGrid',
                            name: 'selectedGrid',
                            border: false,
                            title: 'Выбранные дома',
                            store: me.storeSelected,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ExternalId',
                                    width: 100,
                                    text: 'Номер'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Address',
                                    flex: 1,
                                    text: 'Адрес'
                                },
                                {
                                    xtype: 'b4deletecolumn',
                                    handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                                        me.fireEvent('selectedgridrowactionhandler', 'delete', rec);
                                    }
                                }
                            ],
                            listeners: {
                                afterrender: function() {
                                    var store = this.getStore();
                                    if (store) {
                                        store.pageSize = store.getCount();
                                    }
                                }
                            },
                            dockedItems: me.rightGridToolbar
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: []
                }
            ]
        });

        me.callParent(arguments);
    }
});
