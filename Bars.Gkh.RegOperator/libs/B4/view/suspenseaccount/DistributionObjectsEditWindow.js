Ext.define('B4.view.suspenseaccount.DistributionObjectsEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.distributionobjectseditwindow',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.MemoryHeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.form.FileField'
    ],

    title: 'Распределение платежа',

    modal: true,

    width: 950,
    height: 500,
    closeAction: 'destroy',

    gridColumns: [],
    gridStore: null,
    enableCellEdit: false,

    initComponent: function () {
        var me = this,
            store = null;

        if (typeof(me.gridStore) === "object") {
            store = me.gridStore;
        } else if (typeof(me.gridStore) === "string") {
            store = Ext.create(me.gridStore);
        }

        Ext.apply(me, {
            cls: 'x-large-head',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            border: 0,
            items: [
                {
                    xtype: 'panel',
                    enableCellEdit: me.enableCellEdit,
                    layout: 'fit',
                    border: false,
                    sum: me.sum,
                    flex: 1,
                    items: [
                        {
                            xtype: 'b4grid',
                            type: 'distribObjects',
                            closeAction: 'destroy',
                            store: store,
                            columns: me.gridColumns,
                            emptyText: 'После изменения распределяемой суммы необходимо выполнить распределение',
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
                                                    xtype: 'button',
                                                    action: 'Accept',
                                                    iconCls: 'icon-accept',
                                                    text: 'Применить распределение'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'Balance',
                                                    fieldLabel: 'Остаток',
                                                    value: me.sum,
                                                    readOnly: true,
                                                    labelWidth: 40,
                                                    margin: '1 0 1 7',
                                                    width: 100,
                                                    labelAlign: 'right'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'DistrSum',
                                                    fieldLabel: 'Распределяемая сумма',
                                                    value: me.sum,
                                                    hideTrigger: true,
                                                    margin: '1 0 1 7',
                                                    labelWidth: 130,
                                                    width: 190,
                                                    labelAlign: 'right'
                                                },
                                                {
                                                    xtype: 'button',
                                                    action: 'Distribute',
                                                    iconCls: 'icon-accept',
                                                    text: 'Распределить'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            fieldLabel: 'Документ',
                                            labelWidth: 60,
                                            name: 'Document'
                                        },
                                        '->',
                                        {
                                            xtype: 'buttongroup',
                                            items: [
                                                {
                                                    xtype: 'b4closebutton',
                                                    listeners: {
                                                        click: function(btn) {
                                                            btn.up('window').close();
                                                        }
                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    store: store,
                                    dock: 'bottom'
                                }
                            ],
                            plugins: [
                                Ext.create('B4.ux.grid.plugin.MemoryHeaderFilters'),
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEdit'
                                })
                            ],
                            features: [{
                                ftype: 'summary',
                                getSummary: function (st, type, field, group) {
                                    if (type) {
                                        if (Ext.isFunction(type)) {
                                            return st.aggregate(type, null, group);
                                        }

                                        switch (type) {
                                            case 'sum':
                                                return this.sumFilteredStore(st, field, group);
                                            default:
                                                return group ? {} : '';
                                        }
                                    }
                                },
                                // считает сумму по отфильтрованным строкам на всех страницах
                                sumFilteredStore: function (st, field, group) {
                                    var proxy = st.getProxy(),
                                        reader = proxy.getReader(),
                                        result = reader.read(proxy.data),
                                        filters,
                                        totalSum = 0;

                                    // Фильтрация. Сделано также как в 
                                    // методе - read : function(operation, callback, scope)
                                    // класса - Ext.ux.data.PagingMemoryProxy
                                    filters = st.filters.items;

                                    if (filters.length > 0) {
                                        Ext.each(result.records, function(record) {
                                            var isMatch = true,
                                                length = filters.length,
                                                i;

                                            for (i = 0; i < length; i++) {
                                                var filter = filters[i],
                                                    fn = filter.filterFn,
                                                    scope = filter.scope;

                                                isMatch = isMatch && fn.call(scope, record);
                                            }
                                            if (isMatch) {
                                                totalSum = totalSum + record.get(field);
                                            }
                                        }, this);
                                    } else {
                                        totalSum = st.sum(field, group);
                                    }
                                    
                                    return totalSum;
                                }
                            }],
                            viewConfig: {
                                loadMask: true
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});