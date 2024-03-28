Ext.define('B4.view.outdoor.element.ElementBeforeCrGrid', {
    extend: 'B4.ux.grid.Panel',
    closable: true,
    alias: 'widget.outdoorelementgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.ConditionElementOutdoor',
        'B4.enums.ElementOutdoorGroup'
    ],

    title: 'Элементы двора до благоустройства',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.outdoor.Element');

       me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store:store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Group',
                    text: 'Группа элемента',
                    enumName: 'B4.enums.ElementOutdoorGroup',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Element',
                    flex: 3,
                    text: 'Наименование элемента',
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    width: 100,

                    text: 'Объем',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    width: 200,
                    text: 'Единица измерения',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Condition',
                    text: 'Состояние',
                    enumName: 'B4.enums.ConditionElementOutdoor',
                    filter: true
                 
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: { loadMask: true },
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
                                    xtype: 'b4addbutton'
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