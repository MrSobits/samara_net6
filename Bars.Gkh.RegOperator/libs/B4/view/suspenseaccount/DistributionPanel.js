Ext.define('B4.view.suspenseaccount.DistributionPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.suspaccdistribpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.form.EnumCombo',
        'B4.enums.SuspenseAccountDistributionParametersView'
    ],

    titleGrid: 'Выбор элементов',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    storeSelect: null,
    storeSelected: null,
    columnsGridSelect: [],
    columnsGridSelected: [],
    selModelMode: 'MULTI',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,
    title: 'Распределение',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    closeAction: 'destroy',

    leftGridToolbar: [],
    rightGridToolbar: [],

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

        Ext.apply(me, {
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Вид распределения',
                    labelWidth: 115,
                    margin: 5,
                    labelAlign: 'right',
                    store: B4.enums.SuspenseAccountDistributionParametersView.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'DistributionView',
                    value: 10
                },
                {
                    xtype: 'panel',
                    border: false,
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4grid',
                            name: 'selectGrid',
                            type: 'multiSelectGrid',
                            style: 'border-top: solid #99bce8 1px; border-right: solid #99bce8 1px;',
                            flex: 1,
                            title: "Выбрать записи",
                            border: false,
                            store: me.storeSelect,
                            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: me.selModelMode }),
                            columns: me.columnsGridSelect,
                            plugins: [],
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
                                            columns: 1,
                                            items: [
                                                {
                                                    xtype: 'b4updatebutton'
                                                }
                                            ]
                                        }
                                    ].concat(me.leftGridToolbar)
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
                            dockedItems: me.rightGridToolbar
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Продолжить',
                                    action: 'NextStep'
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