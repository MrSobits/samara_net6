Ext.define('B4.view.constructionobject.typework.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.TypeWork'
    ],

    alias: 'widget.constructionobjtypeworkgrid',

    title: 'Виды работ',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.constructionobject.TypeWork');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Тип работы',
                    renderer: function(val) {
                        return B4.enums.TypeWork.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 3,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasExpertise',
                    flex: 1,
                    text: 'Наличие экспертизы',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasPsd',
                    flex: 1,
                    text: 'Наличие ПСД',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasureName',
                    width: 80,
                    text: 'Ед. изм.'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    flex: 1,
                    text: 'Объем',
                    width: 80,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    width: 100,
                    text: 'Сумма (руб.)',
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
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
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});