Ext.define('B4.view.cmnestateobj.group.formula.ParamGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.groupformulaparamsgrid',
    title: 'Параметры формулы',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.store.FormulaParam'
    ],
    
    initComponent: function () {
        var me = this,
            nameRenderer = function (val, md, rec) {
                return val && val.Name ? val.Name : rec.get('ValueResolverName');
            };

        Ext.applyIf(me, {
            store: Ext.create('B4.store.FormulaParam'),
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
                    dataIndex: 'Attribute',
                    flex: 1,
                    text: 'Характеристика',
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