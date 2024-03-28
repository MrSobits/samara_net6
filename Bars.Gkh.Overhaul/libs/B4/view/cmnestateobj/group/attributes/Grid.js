Ext.define('B4.view.cmnestateobj.group.attributes.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.groupattributesgrid',
    title: 'Характеристики группы конструктивного элемента',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',

        'B4.store.cmnestateobj.StructuralElementGroupAttribute'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            store: Ext.create('B4.store.cmnestateobj.StructuralElementGroupAttribute'),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AttributeType',
                    width: 150,
                    text: 'Тип',
                    renderer: function (val) { return B4.enums.AttributeType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsNeeded',
                    width: 150,
                    text: 'Обязательность',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Hint',
                    flex: 3,
                    text: 'Подсказка'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
                //,
                //{
                //    xtype: 'b4pagingtoolbar',
                //    displayInfo: true,
                //    store: this.store,
                //    dock: 'bottom'
                //}
            ]
        });

        me.callParent(arguments);
    }
});