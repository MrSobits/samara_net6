Ext.define('B4.view.cmnestateobj.group.structelement.WorkGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.groupelementworksgrid',

    title: 'Работы по конструктивному элементу',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.store.cmnestateobj.StructuralElementWork'
    ],
    
    itemId: 'groupElementWorksGrid',
    store: 'cmnestateobj.StructuralElementWork',

    initComponent: function () {
        var me = this,
            nameRenderer = function(val) {
                return val && val.Name ? val.Name : '';
            };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Job',
                    flex: 1,
                    text: 'Наименование',
                    renderer: nameRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 1,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    width: 150,
                    text: 'Единица измерения',
                    renderer: nameRenderer
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