Ext.define('B4.view.normconsumption.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.normconsumptiongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.store.normconsumption.NormConsumption',
        'B4.enums.NormConsumptionType',
        'B4.store.dict.PeriodNormConsumption'
    ],

    title: 'Нормативы потребления',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.normconsumption.NormConsumption');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    sortable: false,
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_norm_consumption';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechangesmr', record);
                        }
                    },
                    scope: this
                },
                {
                    dataIndex: 'Period',
                    flex: 1,
                    text: 'Период'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 500,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Вид норматива потребления',
                    renderer: function (val) {
                        return B4.enums.NormConsumptionType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'hbox',
                        align: 'strecth'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            margin: '3px 0 0 0',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5px',
                            layout: {
                                type: 'hbox',
                                align: 'leftstrecth'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    operand: CondExpr.operands.eq,
                                    width: 300,
                                    margin: '0 10px 0 0',
                                    storeAutoLoad: false,
                                    fieldLabel: 'Период',
                                    editable: false,
                                    itemId: 'period',
                                    valueField: 'Id',
                                    emptyItem: { Name: '-' },
                                    store: Ext.create('B4.store.dict.PeriodNormConsumption')
                                },
                                {
                                    xtype: 'b4combobox',
                                    operand: CondExpr.operands.eq,
                                    width: 400,
                                    storeAutoLoad: false,
                                    fieldLabel: 'Муниципальный район',
                                    editable: false,
                                    itemId: 'municipality',
                                    valueField: 'Id',
                                    emptyItem: { Name: '-' },
                                    url: '/Municipality/ListWithoutPaging'
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
            ]
        });
        me.callParent(arguments);
    }
});