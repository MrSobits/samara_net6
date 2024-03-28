Ext.define('B4.view.cmnestateobj.group.structelement.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.groupelementsgrid',
    title: 'Конструктивные элементы',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',

        'B4.store.cmnestateobj.StructuralElement'
    ],

    initComponent: function () {
        var me = this,
            nameRenderer = function (val) {
                return val && val.Name ? val.Name : '';
            };

        Ext.applyIf(me, {
            store: Ext.create('B4.store.cmnestateobj.StructuralElement'),
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
                    dataIndex: 'Code',
                    width: 200,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReformCode',
                    text: 'Код реформы ЖКХ',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    width: 150,
                    text: 'Единица измерения',
                    renderer: nameRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LifeTime',
                    width: 150,
                    text: 'Срок эксплуатации'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LifeTimeAfterRepair',
                    width: 150,
                    text: 'Срок эксплуатации после ремонта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeDoc',
                    width: 200,
                    text: 'Нормативный документ',
                    renderer: nameRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MutuallyExclusiveGroup',
                    flex: 1,
                    text: 'Группа взаимоисключаемости'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CalculateBy',
                    flex: 1,
                    text: 'Считать по',
                    renderer: function (val) {
                        return B4.enums.PriceCalculateBy.displayRenderer(val);
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