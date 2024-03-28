Ext.define('B4.view.baseplanaction.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn'
    ],

    alias: 'widget.basePlanActionGrid',

    store: 'BasePlanAction',
    itemId: 'basePlanActionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
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
                    width: 175,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_inspection_planaction';
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
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'MoSettlement',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlaceName',
                    width: 160,
                    text: 'Населенный пункт',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubjectName',
                    flex: 1,
                    text: 'Субъект наблюдения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Plan',
                    width: 170,
                    text: 'План',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Начало',
                    format: 'd.m.Y',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Окончание',
                    format: 'd.m.Y',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            xtype: 'b4addbutton'
                        },
                        /*
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Экспорт',
                            textAlign: 'left',
                            itemId: 'btnExport'
                        },*/
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseInspections',
                            boxLabel: 'Показать закрытые проверки',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});